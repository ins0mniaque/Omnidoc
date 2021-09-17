using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Omnidoc.Core
{
    public interface IDocumentComposer : IService
    {
        Task < IDocument < IPageComposer > > CreateAsync (                  CancellationToken cancellationToken = default );
        Task < IDocument < IPageComposer > > LoadAsync   ( Stream document, CancellationToken cancellationToken = default );

        Task < IPageComposer > CreatePageAsync ( CancellationToken cancellationToken = default );
    }
}