using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Omnidoc.Core.Disposables
{
    // TODO: Implement as non-async for performance
    //       See Microsoft.Extensions.DependencyInjection.ServiceLookup.ServiceProviderEngineScope for reference
    public abstract class AsyncDisposableContainer < T > : AsyncDisposable where T : IAsyncDisposable, IDisposable
    {
        protected abstract IEnumerable < T >? BeginDispose ( );

        protected override void Dispose ( bool disposing )
        {
            if ( BeginDispose ( ) is { } disposables && disposing )
                foreach ( var disposable in disposables )
                    disposable.Dispose ( );
        }

        protected override async ValueTask DisposeAsyncCore ( )
        {
            if ( BeginDispose ( ) is { } disposables )
                foreach ( var disposable in disposables )
                    await disposable.DisposeAsync ( ).ConfigureAwait ( false );
        }
    }
}