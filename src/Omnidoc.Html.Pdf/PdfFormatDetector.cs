using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Omnidoc.Core;
using Omnidoc.Core.Disposables;
using Omnidoc.IO;
using Omnidoc.Services;

namespace Omnidoc.Html.Pdf
{
    public class PdfFormatDetector : AsyncDisposable, IFileFormatDetector
    {
        private static readonly IServiceDescriptor descriptor = new ServiceDescriptor
        (
            new [ ] { FileFormats.Pdf }
        );

        private static readonly FileSignature [ ] signatures = new [ ]
        {
            new FileSignature ( "%PDF" )
        };

        public IServiceDescriptor Descriptor => descriptor;

        public async Task < FileFormat? > DetectAsync ( Stream input, CancellationToken cancellationToken = default )
        {
            if ( input is null )
                throw new ArgumentNullException ( nameof ( input ) );

            return await input.MatchAsync ( signatures, cancellationToken ).ConfigureAwait ( false ) switch
            {
                0 => FileFormats.Pdf,
                _ => null
            };
        }
    }
}