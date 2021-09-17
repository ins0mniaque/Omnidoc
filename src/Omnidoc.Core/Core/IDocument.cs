using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Omnidoc.Core
{
    public interface IDocument < TPage > : IPager < TPage > where TPage : IPage
    {
        Task InsertPageAsync ( IPage  page,   int index, CancellationToken cancellationToken = default );
        Task RemovePageAsync (                int index, CancellationToken cancellationToken = default );
        Task WriteAsync      ( Stream output,            CancellationToken cancellationToken = default );
    }
}