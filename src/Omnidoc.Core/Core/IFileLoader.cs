using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Omnidoc.Core
{
    public interface IFileLoader < T >
    {
        Task < T > LoadAsync ( Stream input, CancellationToken cancellationToken = default );
    }
}