using System.Threading;
using System.Threading.Tasks;

namespace Omnidoc.Core
{
    public interface IArchiveBuilder : IArchiveLoader < IArchive >
    {
        Task < IArchive > CreateAsync ( ArchiveOptions options, CancellationToken cancellationToken = default );
    }
}