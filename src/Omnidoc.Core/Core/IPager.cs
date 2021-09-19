using System;
using System.Threading;
using System.Threading.Tasks;

namespace Omnidoc.Core
{
    public interface IPager < T > : IAsyncDisposable, IDisposable
    {
        Task < int > GetPageCountAsync ( CancellationToken cancellationToken = default );
        Task < T >   GetPageAsync      ( int index, CancellationToken cancellationToken = default );
    }
}