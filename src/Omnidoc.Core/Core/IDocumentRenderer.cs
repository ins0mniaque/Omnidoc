using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Omnidoc.Core
{
    public interface IDocumentRenderer : IService
    {
        Task < IPager < IPageRenderer > > LoadAsync ( Stream document, CancellationToken cancellationToken = default );
    }
}