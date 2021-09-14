using System;
using System.Collections.Generic;
using System.IO;

using Omnidoc.IO;
using Omnidoc.Services;

namespace Omnidoc.Pdf
{
    public class PdfDocumentTypeDetector : IDocumentTypeDetector
    {
        public static IReadOnlyCollection < byte [ ] > MagicNumbers { get; } = new [ ] { MagicNumber.From ( "%PDF" ) };

        public IReadOnlyCollection < DocumentType > Types { get; } = new [ ] { DocumentTypes.Pdf };

        public DocumentType? DetectType ( Stream stream )
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