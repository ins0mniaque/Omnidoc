using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Omnidoc.Services
{
    public interface IDocumentPreviewer : IDocumentService
    {
        Task PreviewAsync ( Stream document, Stream output, RenderingOptions options, CancellationToken cancellationToken = default );
    }
}