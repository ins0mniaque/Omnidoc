using System;
using System.IO;

using Omnidoc.IO;
using Omnidoc.Services;

namespace Omnidoc.HtmlToPdf
{
    public class HtmlToPdfDocumentTypeDetector : IDocumentTypeDetector
    {
        private static readonly IDocumentServiceDescriptor descriptor = new DocumentServiceDescriptor
        (
            new [ ] { DocumentTypes.Html, DocumentTypes.Pdf }
        );

        private static readonly byte [ ] [ ] magicNumbers = new [ ]
        {
            MagicNumber.From ( "<" ),
            MagicNumber.From ( "%PDF" )
        };

        public IDocumentServiceDescriptor Descriptor => descriptor;

        public DocumentType? DetectType ( Stream stream )
        {
            if ( stream is null )
                throw new ArgumentNullException ( nameof ( stream ) );

            // TODO: Improve HTML detection
            return stream.Match ( magicNumbers ) switch
            {
                0 => DocumentTypes.Html,
                1 => DocumentTypes.Pdf,
                _ => null
            };
        }
    }
}