using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;

using Omnidoc.Services;

namespace Omnidoc.Xps
{
    public class XpsDocumentParser : IDocumentParser
    {
        public IReadOnlyCollection < DocumentType > Types { get; } = new [ ] { DocumentTypes.Xps, DocumentTypes.Oxps };

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