using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Omnidoc.Services
{
    public interface IDocumentTypeDetector : IDocumentService
    {
        Task < DocumentType? > DetectTypeAsync ( Stream stream, CancellationToken cancellationToken = default );
    }
}