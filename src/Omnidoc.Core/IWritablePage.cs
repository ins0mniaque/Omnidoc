using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

using Omnidoc.Content;

namespace Omnidoc
{
    public interface IWritablePage : IPage
    {
        Task < bool > TryAddAsync    ( DocumentContent content, CancellationToken cancellationToken = default );
        Task < bool > TryRemoveAsync ( DocumentContent content, CancellationToken cancellationToken = default );

        async Task AddAsync ( DocumentContent content, CancellationToken cancellationToken = default )
        {
            if ( ! await TryAddAsync ( content, cancellationToken ).ConfigureAwait ( false ) )
                throw new ArgumentException ( string.Format ( CultureInfo.InvariantCulture, Strings.Error_FailedToAddContent, content?.GetType ( ).Name ), nameof ( content ) );
        }

        async Task RemoveAsync ( DocumentContent content, CancellationToken cancellationToken = default )
        {
            if ( ! await TryRemoveAsync ( content, cancellationToken ).ConfigureAwait ( false ) )
                throw new ArgumentException ( string.Format ( CultureInfo.InvariantCulture, Strings.Error_FailedToRemoveContent, content?.GetType ( ).Name ), nameof ( content ) );
        }
    }
}