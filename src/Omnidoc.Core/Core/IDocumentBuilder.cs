using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Omnidoc.Core
{
    public interface IDocumentBuilder : IService
    {
        Task < IDocument < IPage > > LoadAsync ( Stream document, CancellationToken cancellationToken = default );
    }
}