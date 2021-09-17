using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Omnidoc.Core
{
    public interface IReadOnlyArchive
    {
        Task < IArchiveEntry? > TryGetEntryAsync ( int    index, CancellationToken cancellationToken = default );
        Task < IArchiveEntry? > TryGetEntryAsync ( string path,  CancellationToken cancellationToken = default );

        async Task < IArchiveEntry > GetEntryAsync ( int index, CancellationToken cancellationToken = default )
        {
            return await TryGetEntryAsync ( index, cancellationToken ).ConfigureAwait ( false ) ??
                   throw new ArgumentException ( string.Format ( CultureInfo.InvariantCulture, Strings.Error_ArchiveEntryNotFound, index ), nameof ( index ) );
        }

        async Task < IArchiveEntry > GetEntryAsync ( string path, CancellationToken cancellationToken = default )
        {
            return await TryGetEntryAsync ( path, cancellationToken ).ConfigureAwait ( false ) ??
                   throw new ArgumentException ( string.Format ( CultureInfo.InvariantCulture, Strings.Error_ArchiveEntryNotFound, path ), nameof ( path ) );
        }

        async IAsyncEnumerable < IArchiveEntry > GetEntriesAsync ( [ EnumeratorCancellation ] CancellationToken cancellationToken = default )
        {
            var index = 0;
            while ( await TryGetEntryAsync ( index++ ).ConfigureAwait ( false ) is IArchiveEntry entry )
                yield return entry;
        }
    }
}