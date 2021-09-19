using System;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;

using Omnidoc.Core;
using Omnidoc.Core.Disposables;
using Omnidoc.IO;
using Omnidoc.Services;

namespace Omnidoc.Zip
{
    public abstract class ZipContainerFormatDetector : AsyncDisposable, IFileFormatDetector
    {
        private static FileSignature [ ] signatures = new [ ]
        {
            new FileSignature ( (byte) 'P', (byte) 'K', 0x03, 0x04 )
        };

        public abstract IServiceDescriptor Descriptor { get; }

        public async Task < FileFormat? > DetectAsync ( Stream file, CancellationToken cancellationToken = default )
        {
            if ( file is null )
                throw new ArgumentNullException ( nameof ( file ) );

            return await file.MatchAsync ( signatures, cancellationToken ).ConfigureAwait ( false ) switch
            {
                0 => await DetectContainerAsync ( file, cancellationToken ).ConfigureAwait ( false ),
                _ => null
            };
        }

        private async Task < FileFormat? > DetectContainerAsync ( Stream file, CancellationToken cancellationToken )
        {
            file.Seek ( 0, SeekOrigin.Begin );

            using var archive = new ZipArchive ( file, ZipArchiveMode.Read, true );

            return await DetectAsync ( archive, cancellationToken ).ConfigureAwait ( false );
        }

        protected abstract Task < FileFormat? > DetectAsync ( ZipArchive archive, CancellationToken cancellationToken );
    }
}