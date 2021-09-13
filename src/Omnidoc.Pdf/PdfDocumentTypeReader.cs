using System;
using System.Collections.Generic;
using System.IO;

using Omnidoc.Services;

namespace Omnidoc.Pdf
{
    public class PdfDocumentTypeReader : IDocumentTypeReader
    {
        public IReadOnlyCollection < DocumentType > Types { get; } = new [ ] { DocumentTypes.Pdf };

        public DocumentType? ReadDocumentType ( Stream stream )
        {
            if ( stream is null )
                throw new ArgumentNullException ( nameof ( stream ) );

            var signature = new byte [ 4 ];

            if ( stream.Read ( signature, 0, 4 ) != 4 )
                return null;

            if ( signature [ 0 ] == 0x25 && signature [ 1 ] == 0x50 && signature [ 2 ] == 0x44 && signature [ 3 ] == 0x46 )
                return DocumentTypes.Pdf;

            return null;
        }
    }
}