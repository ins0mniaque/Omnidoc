using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Omnidoc.Core
{
    public interface IDocumentParser : IService
    {
        Task < IPager < IPageParser > > LoadAsync ( Stream document, CancellationToken cancellationToken = default );
    }
}