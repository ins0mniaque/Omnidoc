using System;
using System.Diagnostics.CodeAnalysis;

namespace Omnidoc.Core.Disposables
{
    public static class Disposable
    {
        public static Disposable < T > AsDisposable < T > ( this T instance, Action < T > dispose )
        {
            return new Disposable < T > ( instance, dispose );
        }
    }

    public sealed class Disposable < T > : IDisposable
    {
        private readonly T            instance;
        private readonly Action < T > dispose;

        public Disposable ( T instance, Action < T > dispose )
        {
            this.instance = instance;
            this.dispose  = dispose;
        }

        public T Unwrap ( ) => instance;

        private bool disposed;

        public void Dispose ( )
        {
            if ( ! disposed )
            {
                dispose ( instance );

                disposed = true;
            }
        }

        [ SuppressMessage ( "Usage", "CA2225:Operator overloads have named alternates", Justification = nameof ( Unwrap ) ) ]
        [ return: NotNullIfNotNull ( "disposable" ) ]
        public static implicit operator T? ( Disposable < T > disposable ) => disposable is null ? default : disposable.Unwrap ( );
    }
}