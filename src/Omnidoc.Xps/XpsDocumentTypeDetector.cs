using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Omnidoc.IO;
using Omnidoc.Services;

namespace Omnidoc.Xps
{
    public class XpsDocumentTypeDetector : IDocumentTypeDetector
    {
        private static readonly IDocumentServiceDescriptor descriptor = new DocumentServiceDescriptor
        (
            new [ ] { DocumentTypes.Xps, DocumentTypes.Oxps }
        );

        private static FileSignature [ ] signatures = new [ ]
        {
            new FileSignature ( (byte) 'P', (byte) 'K', 0x03, 0x04 )
        };

        public IDocumentServiceDescriptor Descriptor => descriptor;

        public async Task < DocumentType? > DetectTypeAsync ( Stream stream, CancellationToken cancellationToken = default )
        {
            if ( stream is null )
                throw new ArgumentNullException ( nameof ( stream ) );

            // TODO: Detect non-xps zip files and oxps files
            return await stream.MatchAsync ( signatures ).ConfigureAwait ( false ) switch
            {
                0 => DocumentTypes.Xps,
                _ => null
            };
        }
    }
}