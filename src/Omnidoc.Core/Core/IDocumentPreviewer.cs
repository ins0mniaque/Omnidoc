using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Omnidoc.Core
{
    public interface IDocumentPreviewer : IService
    {
        Task PreviewAsync ( Stream document, Stream output, RenderingOptions options, CancellationToken cancellationToken = default );
    }
}