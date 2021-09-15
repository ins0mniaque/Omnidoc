using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Omnidoc.Services
{
    public interface IDocumentReader : IDocumentMetadataReader
    {
        Task < IDocument > ReadAsync ( Stream document, CancellationToken cancellationToken = default );
    }
}