using System;
using System.IO;

using Omnidoc.IO;
using Omnidoc.Services;

namespace Omnidoc.Image
{
    public class ImageDocumentTypeDetector : IDocumentTypeDetector
    {
        private static readonly IDocumentServiceDescriptor descriptor = new DocumentServiceDescriptor
        (
            new [ ] { DocumentTypes.Bmp, DocumentTypes.Gif, DocumentTypes.Jpeg, DocumentTypes.Png, DocumentTypes.Tiff }
        );

        private static readonly byte [ ] [ ] magicNumbers = new [ ]
        {
            MagicNumber.From ( "BM" ),
            MagicNumber.From ( "GIF87a" ),
            MagicNumber.From ( "GIF89a" ),
            MagicNumber.From ( 0xFF, 0xD8 ),
            MagicNumber.From ( 0x89, (byte) 'P', (byte) 'N', (byte) 'G', 0x0D, 0x0A, 0x1A, 0x0A ),
            MagicNumber.From ( "I I"  ),
            MagicNumber.From ( "II*." ),
            MagicNumber.From ( "MM.*" ),
            MagicNumber.From ( "MM.+" )
        };

        public IDocumentServiceDescriptor Descriptor => descriptor;

        public DocumentType? DetectType ( Stream stream )
        {
            if ( stream is null )
                throw new ArgumentNullException ( nameof ( stream ) );

            return stream.Match ( magicNumbers ) switch
            {
                0 => DocumentTypes.Bmp,
                1 => DocumentTypes.Gif,
                2 => DocumentTypes.Gif,
                3 => DocumentTypes.Jpeg,
                4 => DocumentTypes.Png,
                5 => DocumentTypes.Tiff,
                6 => DocumentTypes.Tiff,
                7 => DocumentTypes.Tiff,
                8 => DocumentTypes.Tiff,
                _ => null
            };
        }
    }
}