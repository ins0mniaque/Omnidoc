using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Omnidoc.IO;
using Omnidoc.Services;

namespace Omnidoc.Pdf
{
    public class PdfDocumentTypeDetector : IDocumentTypeDetector
    {
        private static readonly IDocumentServiceDescriptor descriptor = new DocumentServiceDescriptor
        (
            new [ ] { DocumentTypes.Pdf }
        );

        private static readonly FileSignature [ ] signatures = new [ ]
        {
            new FileSignature ( "%PDF" )
        };

        public IDocumentServiceDescriptor Descriptor => descriptor;

        public async Task < DocumentType? > DetectTypeAsync ( Stream stream, CancellationToken cancellationToken = default )
        {
            if ( stream is null )
                throw new ArgumentNullException ( nameof ( stream ) );

            return await stream.MatchAsync ( signatures ).ConfigureAwait ( false ) switch
            {
                0 => DocumentTypes.Pdf,
                _ => null
            };
        }
    }
}