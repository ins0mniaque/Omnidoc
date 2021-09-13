using System;
using System.Collections.Generic;
using System.IO;

using Omnidoc.IO;
using Omnidoc.Services;

namespace Omnidoc.Image
{
    public class ImageDocumentTypeReader : IDocumentTypeReader
    {
        public static IReadOnlyCollection < byte [ ] > MagicNumbers { get; } = new [ ]
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

        public IReadOnlyCollection < DocumentType > Types { get; } = new [ ] { DocumentTypes.Bmp, DocumentTypes.Gif, DocumentTypes.Jpeg, DocumentTypes.Png, DocumentTypes.Tiff };

        public DocumentType? ReadDocumentType ( Stream stream )
        {
            if ( stream is null )
                throw new ArgumentNullException ( nameof ( stream ) );

            return stream.Match ( MagicNumbers ) switch
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