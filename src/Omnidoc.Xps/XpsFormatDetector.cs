using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Omnidoc.Core;
using Omnidoc.IO;
using Omnidoc.Services;

namespace Omnidoc.Xps
{
    public class XpsFormatDetector : IFileFormatDetector
    {
        private static readonly IServiceDescriptor descriptor = new ServiceDescriptor
        (
            new [ ] { FileFormats.Xps, FileFormats.Oxps }
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

            // TODO: Detect non-xps zip files and oxps files
            return await file.MatchAsync ( signatures ).ConfigureAwait ( false ) switch
            {
                0 => FileFormats.Xps,
                _ => null
            };
        }
    }
}