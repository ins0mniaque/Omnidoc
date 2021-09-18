using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

using Omnidoc.IO;

namespace Omnidoc.Zip.Opc
{
    public static class OpcPackage
    {
        public static async Task < FileFormat [ ]? > TryReadContentTypesAsync ( this ZipArchive archive, CancellationToken cancellationToken = default )
        {
            if ( archive is null )
                throw new ArgumentNullException ( nameof ( archive ) );

            var contentTypes = archive.GetEntry ( OpcPath.ContentTypes );
            if ( contentTypes is null )
                return null;

            var xml = await XDocument.LoadAsync      ( contentTypes.Open ( ), LoadOptions.None, cancellationToken )
                                     .ConfigureAwait ( false );

            return xml.Element  ( OpcSchema.ContentTypes + "Types"   )
                      .Elements ( OpcSchema.ContentTypes + "Default" )
                      .Select   ( ParseContentType )
                      .ToArray  ( );

            FileFormat ParseContentType ( XElement relationship )
            {
                var contentType = relationship.Attribute ( "ContentType" ).Value;
                var extension   = relationship.Attribute ( "Extension"   ).Value;

                return new FileFormat ( extension, contentType, extension );
            }
        }

        public static async Task < OpcRelationship [ ]? > TryReadRelationshipsAsync ( this ZipArchive archive, CancellationToken cancellationToken = default )
        {
            if ( archive is null )
                throw new ArgumentNullException ( nameof ( archive ) );

            var relationships = archive.GetEntry ( OpcPath.Relationships );
            if ( relationships is null )
                return null;

            var xml = await XDocument.LoadAsync      ( relationships.Open ( ), LoadOptions.None, cancellationToken )
                                     .ConfigureAwait ( false );

            return xml.Element  ( OpcSchema.Relationships + "Relationships" )
                      .Elements ( OpcSchema.Relationships + "Relationship"  )
                      .Select   ( ParseRelationship )
                      .ToArray  ( );

            OpcRelationship ParseRelationship ( XElement relationship )
            {
                var id     = relationship.Attribute ( "Id"     ).Value;
                var type   = relationship.Attribute ( "Type"   ).Value;
                var target = relationship.Attribute ( "Target" ).Value;
                var entry  = archive.GetEntry ( target.TrimStart ( '/' ) );

                return new OpcRelationship ( id, type, target, entry );
            }
        }

        public static async Task < FileMetadata? > TryReadMetadataAsync ( this ZipArchive archive, CancellationToken cancellationToken = default )
        {
            if ( archive is null )
                throw new ArgumentNullException ( nameof ( archive ) );

            var relationships = await TryReadRelationshipsAsync ( archive, cancellationToken ).ConfigureAwait ( false );
            if ( relationships is null )
                return null;

            return await relationships.TryReadMetadataAsync ( cancellationToken ).ConfigureAwait ( false );
        }

        public static async Task < FileMetadata? > TryReadMetadataAsync ( this IEnumerable < OpcRelationship > relationships, CancellationToken cancellationToken = default )
        {
            if ( relationships is null )
                throw new ArgumentNullException ( nameof ( relationships ) );

            var properties = relationships.FirstOrDefault ( relationship => relationship.Type == OpcSchema.Metadata );
            if ( properties is null || properties.Entry is null )
                return null;

            var xml = await XDocument.LoadAsync      ( properties.Entry.Open ( ), LoadOptions.None, cancellationToken )
                                     .ConfigureAwait ( false );

            var metadata = new FileMetadata ( );
            foreach ( var property in xml.Element ( OpcSchema.Metadata + "coreProperties" ).Elements ( ) )
                metadata [ property.Name.LocalName ] = property.Value;

            return metadata;
        }

        public static async Task < Stream? > TryOpenThumbnailAsync ( this ZipArchive archive, CancellationToken cancellationToken = default )
        {
            if ( archive is null )
                throw new ArgumentNullException ( nameof ( archive ) );

            var relationships = await TryReadRelationshipsAsync ( archive, cancellationToken ).ConfigureAwait ( false );
            if ( relationships is null )
                return null;

            return relationships.TryOpenThumbnail ( );
        }

        public static Stream? TryOpenThumbnail ( this IEnumerable < OpcRelationship > relationships )
        {
            if ( relationships is null )
                throw new ArgumentNullException ( nameof ( relationships ) );

            var thumbnail = relationships.FirstOrDefault ( relationship => relationship.Type == OpcSchema.Thumbnail );
            if ( thumbnail is null || thumbnail.Entry is null )
                return null;

            return thumbnail.Entry.Open ( );
        }
    }
}