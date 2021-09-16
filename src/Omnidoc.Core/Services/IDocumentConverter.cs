using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Omnidoc.Services
{
    public interface IDocumentConverter : IDocumentService
    {
        Task ConvertAsync ( Stream document, Stream output, OutputOptions options, CancellationToken cancellationToken = default );
    }
}