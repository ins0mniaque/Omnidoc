using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Omnidoc.Core.Disposables
{
    public abstract class AsyncDisposable : IAsyncDisposable, IDisposable
    {
        private bool disposed;

        protected void ThrowIfDisposed ( )
        {
            if ( disposed )
                throw new ObjectDisposedException ( GetType ( ).Name );
        }

        private bool BeginDispose ( )
        {
            if ( disposed )
                return false;

            return disposed = true;
        }

        [ SuppressMessage ( "Design", "CA1063:Implement IDisposable Correctly", Justification = "Ignore subsequent Dispose calls" ) ]
        public void Dispose ( )
        {
            if ( ! BeginDispose ( ) )
                return;

            Dispose ( disposing: true );
            GC.SuppressFinalize ( this );
        }

        public async ValueTask DisposeAsync ( )
        {
            if ( ! BeginDispose ( ) )
                return;

            await DisposeAsyncCore ( ).ConfigureAwait ( false );

            Dispose ( disposing: false );
            GC.SuppressFinalize ( this );
        }

        protected virtual void Dispose ( bool disposing ) { }

        [ SuppressMessage ( "Design", "CA1031:Do not catch general exception types", Justification = "IAsyncDisposable" ) ]
        protected virtual ValueTask DisposeAsyncCore ( )
        {
            try
            {
                Dispose ( disposing: true );

                return default;
            }
            catch ( Exception exception )
            {
                return new ValueTask ( Task.FromException ( exception ) );
            }
        }
    }
}