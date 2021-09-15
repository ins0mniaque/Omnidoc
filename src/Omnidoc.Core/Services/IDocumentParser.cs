using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Omnidoc.Services
{
    public interface IDocumentParser : IDocumentService
    {
        Task < IPager < IPageParser > > PrepareAsync ( Stream document, CancellationToken cancellationToken = default );
    }
}