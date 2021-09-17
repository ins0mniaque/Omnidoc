using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

using Omnidoc.Model;

namespace Omnidoc.Core
{
    public interface IPageComposer : IPage
    {
        Task < bool > TryAddAsync    ( Element element, CancellationToken cancellationToken = default );
        Task < bool > TryRemoveAsync ( Element element, CancellationToken cancellationToken = default );

        async Task AddAsync ( Element element, CancellationToken cancellationToken = default )
        {
            if ( ! await TryAddAsync ( element, cancellationToken ).ConfigureAwait ( false ) )
                throw new ArgumentException ( string.Format ( CultureInfo.InvariantCulture, Strings.Error_FailedToAddElement, element?.GetType ( ).Name ), nameof ( element ) );
        }

        async Task RemoveAsync ( Element element, CancellationToken cancellationToken = default )
        {
            if ( ! await TryRemoveAsync ( element, cancellationToken ).ConfigureAwait ( false ) )
                throw new ArgumentException ( string.Format ( CultureInfo.InvariantCulture, Strings.Error_FailedToRemoveElement, element?.GetType ( ).Name ), nameof ( element ) );
        }
    }
}