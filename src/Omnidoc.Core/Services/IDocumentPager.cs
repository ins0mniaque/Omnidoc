using System;
using System.Threading;
using System.Threading.Tasks;

namespace Omnidoc.Services
{
    public interface IDocumentPager < T > : IDisposable
    {
        Task < int > GetPageCountAsync ( CancellationToken cancellationToken = default );
        Task < T >   GetPageAsync      ( int page, CancellationToken cancellationToken = default );
    }
}