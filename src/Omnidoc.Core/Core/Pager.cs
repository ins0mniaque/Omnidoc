using System;
using System.Threading;
using System.Threading.Tasks;

using Omnidoc.Core.Disposables;

namespace Omnidoc.Core
{
    public abstract class Pager < TDocument, TPage, T > : AsyncDisposable, IPager < T >
    {
        protected Pager ( TDocument document, Func < TPage, T > factory )
        {
            Document = document;
            Factory  = factory;
        }

        protected Pager ( Disposable < TDocument > document, Func < TPage, T > factory )
        {
            Document   = document;
            Factory    = factory;
            Disposable = document;
        }

        public    TDocument                 Document   { get; }
        protected Func < TPage, T >         Factory    { get; }
        private   Disposable < TDocument >? Disposable { get; }

        public abstract Task < int > GetPageCountAsync ( CancellationToken cancellationToken );

        public async Task < T > GetPageAsync ( int index, CancellationToken cancellationToken )
        {
            var page = await LoadPageAsync ( index, cancellationToken ).ConfigureAwait ( false );
            if ( page is null )
                throw new InvalidOperationException ( "Error loading page" );

            return Factory ( page );
        }

        protected abstract Task < TPage? > LoadPageAsync ( int index, CancellationToken cancellationToken );

        protected override void Dispose ( bool disposing )
        {
            if ( disposing )
                Disposable?.Dispose ( );

            base.Dispose ( disposing );
        }

        protected override ValueTask DisposeAsyncCore ( )
        {
            return Disposable is { } disposable ? DisposeAsyncCore ( disposable ) :
                                                  base.DisposeAsyncCore ( );
        }

        private async ValueTask DisposeAsyncCore ( IAsyncDisposable disposable )
        {
            await disposable.DisposeAsync ( ).ConfigureAwait ( false );
            await base.DisposeAsyncCore   ( ).ConfigureAwait ( false );
        }
    }
}