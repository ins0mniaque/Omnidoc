using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Omnidoc.Services
{
    public interface IDocumentConverter : IDocumentService
    {
        IReadOnlyCollection < DocumentType > OutputTypes { get; }

        Task ConvertAsync ( Stream document, Stream output, OutputOptions options, CancellationToken cancellationToken = default );
    }
}