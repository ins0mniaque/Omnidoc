using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Omnidoc.Core;
using Omnidoc.Core.Disposables;
using Omnidoc.IO;
using Omnidoc.Services;

namespace Omnidoc.Html
{
    public class HtmlFormatDetector : AsyncDisposable, IFileFormatDetector
    {
        private static readonly IServiceDescriptor descriptor = new ServiceDescriptor
        (
            new [ ] { FileFormats.Html }
        );

        private static readonly FileSignature [ ] signatures = new [ ]
        {
            new FileSignature ( "<" )
        };

        public IServiceDescriptor Descriptor => descriptor;

        public async Task < FileFormat? > DetectAsync ( Stream input, CancellationToken cancellationToken = default )
        {
            if ( input is null )
                throw new ArgumentNullException ( nameof ( input ) );

            // TODO: Improve HTML detection
            return await input.MatchAsync ( signatures, cancellationToken ).ConfigureAwait ( false ) switch
            {
                0 => FileFormats.Html,
                _ => null
            };
        }
    }
}