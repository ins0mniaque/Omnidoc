using System;
using System.Threading;
using System.Threading.Tasks;

namespace Omnidoc.Services
{
    public interface IDocumentRendering : IDisposable
    {
        Task < int >                   GetPageCountAsync    ( CancellationToken cancellationToken = default );
        Task < IDocumentPageRenderer > GetPageRendererAsync ( int page, CancellationToken cancellationToken = default );
    }
}