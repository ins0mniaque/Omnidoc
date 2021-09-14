using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Tga;
using SixLabors.ImageSharp.Formats.Tiff;

using Omnidoc.Services;

namespace Omnidoc.Image
{
    public class ImageDocumentConverter : IDocumentConverter
    {
        public IReadOnlyCollection < DocumentType > Types       { get; } = new [ ] { DocumentTypes.Bmp, DocumentTypes.Gif, DocumentTypes.Jpeg, DocumentTypes.Png, DocumentTypes.Tiff };
        public IReadOnlyCollection < DocumentType > OutputTypes { get; } = new [ ] { DocumentTypes.Bmp, DocumentTypes.Gif, DocumentTypes.Jpeg, DocumentTypes.Png, DocumentTypes.Tiff };

        public async Task ConvertAsync ( Stream document, Stream output, OutputOptions options, CancellationToken cancellationToken = default )
        {
            if ( document is null ) throw new ArgumentNullException ( nameof ( document ) );
            if ( output   is null ) throw new ArgumentNullException ( nameof ( output   ) );
            if ( options  is null ) throw new ArgumentNullException ( nameof ( options  ) );

            var encoder = options.Type == DocumentTypes.Bmp  ? new BmpEncoder  ( ) :
                          options.Type == DocumentTypes.Gif  ? new GifEncoder  ( ) :
                          options.Type == DocumentTypes.Jpeg ? new JpegEncoder ( ) :
                          options.Type == DocumentTypes.Png  ? new PngEncoder  ( ) :
                          options.Type == DocumentTypes.Tga  ? new TgaEncoder  ( ) :
                          options.Type == DocumentTypes.Tiff ? new TiffEncoder ( ) :
                                               (IImageEncoder) new PngEncoder  ( );

            using var reader = await SixLabors.ImageSharp.Image.LoadAsync ( document ).ConfigureAwait ( false );

            await reader.SaveAsync ( output, encoder ).ConfigureAwait ( false );
        }
    }
}