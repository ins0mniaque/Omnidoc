using System.Threading;
using System.Threading.Tasks;

using Omnidoc.Model;

namespace Omnidoc.Services
{
    public interface IDocumentWriter : IDocumentService
    {
        bool Supports < TContent > ( ) where TContent : Content;

        Task < IWritableDocument > CreateAsync  (                     CancellationToken cancellationToken = default );
        Task < IWritableDocument > PrepareAsync ( IDocument document, CancellationToken cancellationToken = default );
    }
}