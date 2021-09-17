using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Omnidoc.Core;
using Omnidoc.IO;
using Omnidoc.Services;

namespace Omnidoc.Image
{
    public class ImageFormatDetector : IFileFormatDetector
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

        public async Task < FileFormat? > DetectAsync ( Stream file, CancellationToken cancellationToken = default )
        {
            if ( file is null )
                throw new ArgumentNullException ( nameof ( file ) );

            return await file.MatchAsync ( signatures ).ConfigureAwait ( false ) switch
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