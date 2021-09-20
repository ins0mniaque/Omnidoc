using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Omnidoc.Core.Disposables;

namespace Omnidoc.Core
{
    public abstract class FileLoader < TFile, T > : AsyncDisposable, IFileLoader < T >
    {
        protected FileLoader ( Func < TFile, T > factory )
        {
            Factory = factory;
        }

        protected Func < TFile, T > Factory { get; }

        public async Task < T > LoadAsync ( Stream input, CancellationToken cancellationToken )
        {
            var file = await LoadFileAsync ( input, cancellationToken ).ConfigureAwait ( false );
            if ( file is null )
                throw new InvalidOperationException ( "Error loading file" );

            return Factory ( file );
        }

        protected abstract Task < TFile? > LoadFileAsync ( Stream input, CancellationToken cancellationToken );
    }
}