using System.Threading;
using System.Threading.Tasks;

using Omnidoc.Model;

namespace Omnidoc.Core
{
    public interface IPageComposer : IPage
    {
        Task < bool > TryAddAsync    ( Element element, CancellationToken cancellationToken = default );
        Task < bool > TryRemoveAsync ( Element element, CancellationToken cancellationToken = default );

        Task AddAsync    ( Element element, CancellationToken cancellationToken = default );
        Task RemoveAsync ( Element element, CancellationToken cancellationToken = default );
    }
}