using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Omnidoc.Core;

namespace Omnidoc.Image
{
    // TODO: Implement multi-page TIFF support
    public sealed class ImagePager < T > : IPager < T >
    {
        public ImagePager ( Stream document, Func < Stream, T > factory )
        {
            Document = document;
            Factory  = factory;
        }

        public ImagePager ( Stream document, IDisposable disposable, Func < Stream, T > factory )
        {
            Document   = document;
            Disposable = disposable;
            Factory    = factory;
        }

        public  Stream             Document   { get; }
        private IDisposable?       Disposable { get; }
        private Func < Stream, T > Factory    { get; }

        public Task < int > GetPageCountAsync ( CancellationToken cancellationToken )
        {
            return Task.FromResult ( 1 );
        }

        public Task < T > GetPageAsync ( int index, CancellationToken cancellationToken )
        {
            return Task.FromResult ( Factory ( Document ) );
        }

        private bool isDisposed;

        public void Dispose ( )
        {
            if ( ! isDisposed )
            {
                Document   .Dispose ( );
                Disposable?.Dispose ( );

                isDisposed = true;
            }
        }
    }
}