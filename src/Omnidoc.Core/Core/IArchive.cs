using System.Threading;
using System.Threading.Tasks;

using Omnidoc.IO;

namespace Omnidoc.Core
{
    public interface IArchive : IReadOnlyArchive
    {
        Task < IArchiveEntry > CreateEntryAsync ( string path, FileMetadata? metadata, CancellationToken cancellationToken = default );

        Task < bool > TryAddEntryAsync    ( IArchiveEntry entry, CancellationToken cancellationToken = default );
        Task < bool > TryRemoveEntryAsync ( IArchiveEntry entry, CancellationToken cancellationToken = default );
        Task < bool > TryRemoveEntryAsync ( int           index, CancellationToken cancellationToken = default );
        Task < bool > TryRemoveEntryAsync ( string        path,  CancellationToken cancellationToken = default );
    }
}