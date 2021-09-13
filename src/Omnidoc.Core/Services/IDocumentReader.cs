using System.Collections.Generic;
using System.IO;
using System.Threading;

using Omnidoc.Content;

namespace Omnidoc.Services
{
    public interface IDocumentReader : IDocumentService
    {
        IAsyncEnumerable < DocumentContent > ReadAsync ( Stream document, CancellationToken cancellationToken = default );
    }
}