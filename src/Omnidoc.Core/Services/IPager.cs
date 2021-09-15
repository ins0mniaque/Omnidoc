using System;
using System.Threading;
using System.Threading.Tasks;

namespace Omnidoc.Services
{
    public interface IPager < T > : IDisposable
    {
        Task < int > GetPageCountAsync ( CancellationToken cancellationToken = default );
        Task < T >   GetPageAsync      ( int index, CancellationToken cancellationToken = default );
    }
}