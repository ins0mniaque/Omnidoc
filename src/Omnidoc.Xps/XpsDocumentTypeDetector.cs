using System;
using System.IO;

using Omnidoc.IO;
using Omnidoc.Services;

namespace Omnidoc.Xps
{
    public class XpsDocumentTypeDetector : IDocumentTypeDetector
    {
        private static readonly IDocumentServiceDescriptor descriptor = new DocumentServiceDescriptor
        (
            new [ ] { DocumentTypes.Xps, DocumentTypes.Oxps }
        );

        private static byte [ ] [ ] magicNumbers = new [ ]
        {
            MagicNumber.From ( (byte) 'P', (byte) 'K', 0x03, 0x04 )
        };

        public IDocumentServiceDescriptor Descriptor => descriptor;

        public DocumentType? DetectType ( Stream stream )
        {
            if ( stream is null )
                throw new ArgumentNullException ( nameof ( stream ) );

            // TODO: Detect non-xps zip files and oxps files
            return stream.Match ( magicNumbers ) switch
            {
                0 => DocumentTypes.Xps,
                _ => null
            };
        }
    }
}