using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Omnidoc.Core.Disposables
{
    public static class Disposable
    {
        public static Disposable < T > AsDisposable < T > ( this T instance, Action < T > dispose )
        {
            return new Disposable < T > ( instance, dispose );
        }

        public static Disposable < T > AsDisposable < T > ( this T instance, Action < T > dispose, Func < T, ValueTask > disposeAsync )
        {
            return new Disposable < T > ( instance, dispose, disposeAsync );
        }
    }

    public sealed class Disposable < T > : IAsyncDisposable, IDisposable
    {
        private readonly T                     instance;
        private readonly Action < T >          dispose;
        private readonly Func < T, ValueTask > disposeAsync;

        public Disposable ( T instance, Action < T > dispose )
        {
            this.instance     = instance;
            this.dispose      = dispose;
            this.disposeAsync = DefaultDisposeAsync;
        }

        public Disposable ( T instance, Action < T > dispose, Func < T, ValueTask > disposeAsync )
        {
            this.instance     = instance;
            this.dispose      = dispose;
            this.disposeAsync = disposeAsync;
        }

        public T Unwrap ( ) => instance;

        private bool disposed;

        public void Dispose ( )
        {
            if ( ! disposed )
            {
                disposed = true;

                dispose ( instance );
            }
        }

        public async ValueTask DisposeAsync ( )
        {
            if ( ! disposed )
            {
                disposed = true;

                await disposeAsync ( instance ).ConfigureAwait ( false );
            }
        }

        [ SuppressMessage ( "Design", "CA1031:Do not catch general exception types", Justification = "IAsyncDisposable" ) ]
        private ValueTask DefaultDisposeAsync ( T instance )
        {
            try
            {
                Dispose ( );

                return default;
            }
            catch ( Exception exception )
            {
                return new ValueTask ( Task.FromException ( exception ) );
            }
        }

        [ SuppressMessage ( "Usage", "CA2225:Operator overloads have named alternates", Justification = nameof ( Unwrap ) ) ]
        [ return: NotNullIfNotNull ( "disposable" ) ]
        public static implicit operator T? ( Disposable < T > disposable ) => disposable is null ? default : disposable.Unwrap ( );
    }
}