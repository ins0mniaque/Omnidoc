using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Omnidoc.Core;
using Omnidoc.IO;
using Omnidoc.Services;

namespace Omnidoc.Zip
{
    public class ZipFormatDetector : IFileFormatDetector
    {
        private static readonly IServiceDescriptor descriptor = new ServiceDescriptor
        (
            new [ ] { FileFormats.Zip }
        );

        private static FileSignature [ ] signatures = new [ ]
        {
            new FileSignature ( (byte) 'P', (byte) 'K', 0x03, 0x04 )
        };

        public IServiceDescriptor Descriptor => descriptor;

        public async Task < FileFormat? > DetectAsync ( Stream file, CancellationToken cancellationToken = default )
        {
            if ( file is null )
                throw new ArgumentNullException ( nameof ( file ) );

            return await file.MatchAsync ( signatures ).ConfigureAwait ( false ) switch
            {
                0 => FileFormats.Zip,
                _ => null
            };
        }
    }
}