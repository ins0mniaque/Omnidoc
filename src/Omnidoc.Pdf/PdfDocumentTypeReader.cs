using System;
using System.Collections.Generic;
using System.IO;

using Omnidoc.IO;
using Omnidoc.Services;

namespace Omnidoc.Pdf
{
    public class PdfDocumentTypeReader : IDocumentTypeReader
    {
        public static IReadOnlyCollection < byte [ ] > MagicNumbers { get; } = new [ ] { MagicNumber.From ( "%PDF" ) };

        public IReadOnlyCollection < DocumentType > Types { get; } = new [ ] { DocumentTypes.Pdf };

        public DocumentType? ReadDocumentType ( Stream stream )
        {
            if ( stream is null )
                throw new ArgumentNullException ( nameof ( stream ) );

            return stream.Match ( MagicNumbers ) switch
            {
                0 => DocumentTypes.Pdf,
                _ => null
            };
        }
    }
}