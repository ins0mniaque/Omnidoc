using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Omnidoc.Services
{
    public interface IDocumentMetadataReader : IDocumentService
    {
        Task < DocumentMetadata > ReadAsync ( Stream stream, CancellationToken cancellationToken = default );
    }
}