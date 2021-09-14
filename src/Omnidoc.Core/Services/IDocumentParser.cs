using System.Collections.Generic;
using System.IO;
using System.Threading;

using Omnidoc.Content;

namespace Omnidoc.Services
{
    public interface IDocumentParser : IDocumentService
    {
        IAsyncEnumerable < DocumentContent > ParseAsync ( Stream document, CancellationToken cancellationToken = default );
    }
}