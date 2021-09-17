using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

using Omnidoc.Model;

namespace Omnidoc.Core
{
    public interface IPageComposer : IPage
    {
        Task < bool > TryAddAsync    ( Content content, CancellationToken cancellationToken = default );
        Task < bool > TryRemoveAsync ( Content content, CancellationToken cancellationToken = default );

        async Task AddAsync ( Content content, CancellationToken cancellationToken = default )
        {
            if ( ! await TryAddAsync ( content, cancellationToken ).ConfigureAwait ( false ) )
                throw new ArgumentException ( string.Format ( CultureInfo.InvariantCulture, Strings.Error_FailedToAddContent, content?.GetType ( ).Name ), nameof ( content ) );
        }

        async Task RemoveAsync ( Content content, CancellationToken cancellationToken = default )
        {
            if ( ! await TryRemoveAsync ( content, cancellationToken ).ConfigureAwait ( false ) )
                throw new ArgumentException ( string.Format ( CultureInfo.InvariantCulture, Strings.Error_FailedToRemoveContent, content?.GetType ( ).Name ), nameof ( content ) );
        }
    }
}