using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Omnidoc.Core
{
    public interface IReadOnlyArchive : IAsyncDisposable, IDisposable
    {
        Task < IArchiveEntry? > TryGetEntryAsync ( int    index, CancellationToken cancellationToken = default );
        Task < IArchiveEntry? > TryGetEntryAsync ( string path,  CancellationToken cancellationToken = default );

        Task < IArchiveEntry > GetEntryAsync ( int    index, CancellationToken cancellationToken = default );
        Task < IArchiveEntry > GetEntryAsync ( string path,  CancellationToken cancellationToken = default );

        IAsyncEnumerable < IArchiveEntry > GetEntriesAsync ( CancellationToken cancellationToken = default );
    }
}