using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;

using Omnidoc.Model;
using Omnidoc.Services;

namespace Omnidoc.Xps
{
    public class XpsDocumentParser : IDocumentParser
    {
        private static readonly IDocumentServiceDescriptor descriptor = new DocumentServiceDescriptor
        (
            new [ ] { DocumentTypes.Xps, DocumentTypes.Oxps },
            new [ ] { typeof ( Content ) }
        );

        public IDocumentServiceDescriptor Descriptor => descriptor;

        public Task < IPager < IPageParser > > PrepareAsync ( Stream document, CancellationToken cancellationToken = default )
        {
            return Task.Run ( ( ) => Prepare ( document ), cancellationToken );
        }

        private static IPager < IPageParser > Prepare ( Stream document )
        {
            return new XpsPager < IPageParser > ( new ZipArchive ( document ),
                                                  page => new XpsPageParser ( page ) );
        }
    }
}