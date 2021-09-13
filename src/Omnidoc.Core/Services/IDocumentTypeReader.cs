using System;
using System.Globalization;
using System.IO;

namespace Omnidoc.Services
{
    public interface IDocumentTypeReader : IDocumentService
    {
        DocumentType? ReadDocumentType            ( Stream stream );
        DocumentType? ReadDocumentTypeAndSeekBack ( Stream stream )
        {
            if ( stream is null )
                throw new ArgumentNullException ( nameof ( stream ) );

            var contentType = ReadDocumentType ( stream );

            if ( ! stream.CanSeek )
                throw new NotSupportedException ( string.Format ( CultureInfo.InvariantCulture, Strings.Error_StreamMustBeSeekable, $"{ nameof ( DocumentEngine ) }.{ nameof ( IDocumentEngine.PrepareStream ) }" ) );

            stream.Seek ( 0, SeekOrigin.Begin );

            return contentType;
        }
    }
}