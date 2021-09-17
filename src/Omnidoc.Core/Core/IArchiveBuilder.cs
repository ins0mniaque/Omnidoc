using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Omnidoc.Core
{
    public interface IArchiveBuilder : IService
    {
        Task < IArchive > CreateAsync (                 ArchiveOptions options, CancellationToken cancellationToken = default );
        Task < IArchive > LoadAsync   ( Stream archive, ArchiveOptions options, CancellationToken cancellationToken = default );
    }
}