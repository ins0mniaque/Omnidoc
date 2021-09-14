using System;
using System.Globalization;
using System.IO;
using System.Linq;

using Omnidoc.IO;
using Omnidoc.Services;

namespace Omnidoc
{
    public interface IDocumentEngine
    {
        IDocumentServiceProvider Services { get; }

        Stream PrepareStream ( Stream stream )
        {
            if ( stream is null )
                throw new ArgumentNullException ( nameof ( stream ) );

            return stream.CanSeek ? stream : new SeekableReadOnlyStream ( stream );
        }

        DocumentType? DetectType ( Stream stream )
        {
            return Services.GetServices ( )
                           .OfType < IDocumentTypeDetector > ( )
                           .Select         ( reader      => DetectTypeAndSeekBack ( reader, stream ) )
                           .FirstOrDefault ( contentType => contentType != null );
        }

        private static DocumentType? DetectTypeAndSeekBack ( IDocumentTypeDetector reader, Stream stream )
        {
            if ( stream is null )
                throw new ArgumentNullException ( nameof ( stream ) );

            var contentType = reader.DetectType ( stream );

            if ( ! stream.CanSeek )
                throw new NotSupportedException ( string.Format ( CultureInfo.InvariantCulture, Strings.Error_StreamMustBeSeekable, $"{ nameof ( DocumentEngine ) }.{ nameof ( IDocumentEngine.PrepareStream ) }" ) );

            stream.Seek ( 0, SeekOrigin.Begin );

            return contentType;
        }
    }
}