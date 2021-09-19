using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Tga;
using SixLabors.ImageSharp.Formats.Tiff;

using Omnidoc.Core;
using Omnidoc.Core.Disposables;
using Omnidoc.Services;

namespace Omnidoc.Image
{
    using static SixLabors.ImageSharp.Image;

    public class ImageFormatConverter : AsyncDisposable, IFileFormatConverter
    {
        private static readonly IServiceDescriptor descriptor = new ServiceDescriptor
        (
            new [ ] { FileFormats.Bmp, FileFormats.Gif, FileFormats.Jpeg, FileFormats.Png, FileFormats.Tiff },
            new [ ] { FileFormats.Bmp, FileFormats.Gif, FileFormats.Jpeg, FileFormats.Png, FileFormats.Tiff }
        );

        public IServiceDescriptor Descriptor => descriptor;

        public async Task ConvertAsync ( Stream file, Stream output, OutputOptions options, CancellationToken cancellationToken = default )
        {
            if ( file    is null ) throw new ArgumentNullException ( nameof ( file    ) );
            if ( output  is null ) throw new ArgumentNullException ( nameof ( output  ) );
            if ( options is null ) throw new ArgumentNullException ( nameof ( options ) );

            var encoder = options.Format == FileFormats.Bmp  ? new BmpEncoder  ( ) :
                          options.Format == FileFormats.Gif  ? new GifEncoder  ( ) :
                          options.Format == FileFormats.Jpeg ? new JpegEncoder ( ) :
                          options.Format == FileFormats.Png  ? new PngEncoder  ( ) :
                          options.Format == FileFormats.Tga  ? new TgaEncoder  ( ) :
                          options.Format == FileFormats.Tiff ? new TiffEncoder ( ) :
                                               (IImageEncoder) new PngEncoder  ( );

            using var reader = await LoadAsync ( Configuration.Default, file, cancellationToken ).ConfigureAwait ( false );

            await reader.SaveAsync ( output, encoder, cancellationToken ).ConfigureAwait ( false );
        }
    }
}