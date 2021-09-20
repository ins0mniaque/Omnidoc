using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Omnidoc.Core;
using Omnidoc.Core.Disposables;
using Omnidoc.IO;
using Omnidoc.Services;

namespace Omnidoc.Image
{
    public class ImageFormatDetector : AsyncDisposable, IFileFormatDetector
    {
        private static readonly IServiceDescriptor descriptor = new ServiceDescriptor
        (
            new [ ] { FileFormats.Bmp, FileFormats.Gif, FileFormats.Jpeg, FileFormats.Png, FileFormats.Tiff }
        );

        private static readonly FileSignature [ ] signatures = new [ ]
        {
            new FileSignature ( "BM" ),
            new FileSignature ( "GIF87a" ),
            new FileSignature ( "GIF89a" ),
            new FileSignature ( 0xFF, 0xD8 ),
            new FileSignature ( 0x89, (byte) 'P', (byte) 'N', (byte) 'G', 0x0D, 0x0A, 0x1A, 0x0A ),
            new FileSignature ( "I I"  ),
            new FileSignature ( "II*." ),
            new FileSignature ( "MM.*" ),
            new FileSignature ( "MM.+" )
        };

        public IServiceDescriptor Descriptor => descriptor;

        public async Task < FileFormat? > DetectAsync ( Stream input, CancellationToken cancellationToken = default )
        {
            if ( input is null )
                throw new ArgumentNullException ( nameof ( input ) );

            return await input.MatchAsync ( signatures, cancellationToken ).ConfigureAwait ( false ) switch
            {
                0 => FileFormats.Bmp,
                1 => FileFormats.Gif,
                2 => FileFormats.Gif,
                3 => FileFormats.Jpeg,
                4 => FileFormats.Png,
                5 => FileFormats.Tiff,
                6 => FileFormats.Tiff,
                7 => FileFormats.Tiff,
                8 => FileFormats.Tiff,
                _ => null
            };
        }
    }
}