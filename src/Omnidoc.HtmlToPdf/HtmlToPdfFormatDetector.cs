using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Omnidoc.Core;
using Omnidoc.Core.Disposables;
using Omnidoc.IO;
using Omnidoc.Services;

namespace Omnidoc.HtmlToPdf
{
    public sealed class HtmlToPdfFormatDetector : AsyncDisposable, IFileFormatDetector
    {
        private static readonly IServiceDescriptor descriptor = new ServiceDescriptor
        (
            new [ ] { FileFormats.Html, FileFormats.Pdf }
        );

        private static readonly FileSignature [ ] signatures = new [ ]
        {
            new FileSignature ( "<" ),
            new FileSignature ( "%PDF" )
        };

        public IServiceDescriptor Descriptor => descriptor;

        public async Task < FileFormat? > DetectAsync ( Stream file, CancellationToken cancellationToken = default )
        {
            if ( file is null )
                throw new ArgumentNullException ( nameof ( file ) );

            // TODO: Improve HTML detection
            return await file.MatchAsync ( signatures ).ConfigureAwait ( false ) switch
            {
                0 => FileFormats.Html,
                1 => FileFormats.Pdf,
                _ => null
            };
        }
    }
}