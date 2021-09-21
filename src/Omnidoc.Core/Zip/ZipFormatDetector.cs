using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Omnidoc.Core;
using Omnidoc.Core.Disposables;
using Omnidoc.IO;
using Omnidoc.Services;

namespace Omnidoc.Zip
{
    public class ZipFormatDetector : AsyncDisposable, IFileFormatDetector
    {
        private static readonly IServiceDescriptor descriptor = new ServiceDescriptor
        (
            new [ ] { FileFormats.Zip }
        );

        private static readonly FileSignature [ ] signatures = new [ ]
        {
            new FileSignature ( (byte) 'P', (byte) 'K', 0x03, 0x04 )
        };

        public IServiceDescriptor Descriptor => descriptor;

        public async Task < FileFormat? > DetectAsync ( Stream input, CancellationToken cancellationToken = default )
        {
            if ( input is null )
                throw new ArgumentNullException ( nameof ( input ) );

            return await input.MatchAsync ( signatures, cancellationToken ).ConfigureAwait ( false ) switch
            {
                0 => FileFormats.Zip,
                _ => null
            };
        }
    }
}