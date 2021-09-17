using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Omnidoc.Core
{
    public interface IArchiveReader : IService
    {
        Task < IReadOnlyArchive > LoadAsync ( Stream archive, ArchiveOptions options, CancellationToken cancellationToken = default );
    }
}