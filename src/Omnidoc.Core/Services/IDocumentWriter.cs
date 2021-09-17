using System.Threading;
using System.Threading.Tasks;

namespace Omnidoc.Services
{
    public interface IDocumentWriter : IDocumentService
    {
        Task < IWritableDocument > CreateAsync  (                     CancellationToken cancellationToken = default );
        Task < IWritableDocument > PrepareAsync ( IDocument document, CancellationToken cancellationToken = default );
    }
}