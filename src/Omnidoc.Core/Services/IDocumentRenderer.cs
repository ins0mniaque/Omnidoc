using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Omnidoc.Services
{
    public interface IDocumentRenderer : IDocumentService
    {
        Task < IDocumentRendering > PrepareAsync ( Stream document, CancellationToken cancellationToken = default );
    }
}