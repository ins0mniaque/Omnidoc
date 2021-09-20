using System.Threading;
using System.Threading.Tasks;

namespace Omnidoc.Core
{
    public interface IDocumentComposer : IDocumentLoader < IDocument < IPageComposer >, IPageComposer >
    {
        Task < IDocument < IPageComposer > > CreateAsync     ( CancellationToken cancellationToken = default );
        Task < IPageComposer >               CreatePageAsync ( CancellationToken cancellationToken = default );
    }
}