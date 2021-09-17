using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Omnidoc.IO;
using Omnidoc.Services;

namespace Omnidoc.HtmlToPdf
{
    public class HtmlToPdfDocumentTypeDetector : IDocumentTypeDetector
    {
        private static readonly IDocumentServiceDescriptor descriptor = new DocumentServiceDescriptor
        (
            new [ ] { DocumentTypes.Html, DocumentTypes.Pdf }
        );

        private static readonly FileSignature [ ] signatures = new [ ]
        {
            new FileSignature ( "<" ),
            new FileSignature ( "%PDF" )
        };

        public IDocumentServiceDescriptor Descriptor => descriptor;

        public async Task < DocumentType? > DetectTypeAsync ( Stream stream, CancellationToken cancellationToken = default )
        {
            if ( stream is null )
                throw new ArgumentNullException ( nameof ( stream ) );

            // TODO: Improve HTML detection
            return await stream.MatchAsync ( signatures ).ConfigureAwait ( false ) switch
            {
                0 => DocumentTypes.Html,
                1 => DocumentTypes.Pdf,
                _ => null
            };
        }
    }
}