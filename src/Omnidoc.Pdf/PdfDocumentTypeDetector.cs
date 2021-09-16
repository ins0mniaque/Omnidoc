using System;
using System.IO;

using Omnidoc.IO;
using Omnidoc.Services;

namespace Omnidoc.Pdf
{
    public class PdfDocumentTypeDetector : IDocumentTypeDetector
    {
        private static readonly IDocumentServiceDescriptor descriptor = new DocumentServiceDescriptor
        (
            new [ ] { DocumentTypes.Pdf }
        );

        private static readonly byte [ ] [ ] magicNumbers = new [ ]
        {
            MagicNumber.From ( "%PDF" )
        };

        public IDocumentServiceDescriptor Descriptor => descriptor;

        public DocumentType? DetectType ( Stream stream )
        {
            if ( stream is null )
                throw new ArgumentNullException ( nameof ( stream ) );

            return stream.Match ( magicNumbers ) switch
            {
                0 => DocumentTypes.Pdf,
                _ => null
            };
        }
    }
}