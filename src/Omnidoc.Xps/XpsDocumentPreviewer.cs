using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

using Omnidoc.Core;
using Omnidoc.Services;

namespace Omnidoc.Xps
{
    public class XpsDocumentPreviewer : IDocumentPreviewer
    {
        private static readonly IServiceDescriptor descriptor = new ServiceDescriptor
        (
            new [ ] { FileFormats.Xps, FileFormats.OpenXps }
        );

        private static readonly XNamespace xmlns         = "http://schemas.openxmlformats.org/package/2006/relationships";
        private const           string     thumbnailType = "http://schemas.openxmlformats.org/package/2006/relationships/metadata/thumbnail";

        public IServiceDescriptor Descriptor => descriptor;

        public async Task PreviewAsync ( Stream document, Stream output, RenderingOptions options, CancellationToken cancellationToken = default )
        {
            using ( var archive = new ZipArchive ( document ) )
            {
                var relsEntry = archive.GetEntry ( "_rels/.rels" );
                if ( relsEntry is null )
                    throw new InvalidOperationException ( Strings.Error_InvalidXpsDocument );

                var rels = await XDocument.LoadAsync      ( relsEntry.Open ( ), LoadOptions.None, cancellationToken )
                                          .ConfigureAwait ( false );

                var thumbnailPath = rels.Element  ( xmlns + "Relationships" )
                                        .Elements ( xmlns + "Relationship"  )
                                        .Where    ( element => element.Attribute ( "Type"   ).Value == thumbnailType )
                                        .Select   ( element => element.Attribute ( "Target" ).Value.TrimStart ( '/' ) )
                                        .FirstOrDefault ( );
                if ( thumbnailPath is null )
                    return;

                var thumbnailEntry = archive.GetEntry ( thumbnailPath );
                if ( thumbnailEntry is null )
                    throw new InvalidOperationException ( Strings.Error_InvalidXpsDocument );

                using var thumbnail = thumbnailEntry.Open ( );

                await thumbnail.CopyToAsync    ( output, cancellationToken )
                               .ConfigureAwait ( false );
            }
        }
    }
}