using System;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Omnidoc.Services;

namespace Omnidoc.Xps
{
    public sealed class XpsPager < T > : IPager < T >
    {
        public XpsPager ( ZipArchive document, Func < ZipArchiveEntry, T > factory )
        {
            Document = document;
            Factory  = factory;
        }

        public XpsPager ( ZipArchive document, IDisposable disposable, Func < ZipArchiveEntry, T > factory )
        {
            Document   = document;
            Disposable = disposable;
            Factory    = factory;
        }

        public  ZipArchive                  Document   { get; }
        private IDisposable?                Disposable { get; }
        private Func < ZipArchiveEntry, T > Factory    { get; }

        public Task < int > GetPageCountAsync ( CancellationToken cancellationToken )
        {
            return Task.FromResult ( Document.Entries.Count ( IsPage ) );
        }

        public Task < T > GetPageAsync ( int page, CancellationToken cancellationToken )
        {
            return Task.FromResult ( Factory ( FindPage ( Document, page ) ) );
        }

        // TODO: Improve page counting
        private static bool IsPage ( ZipArchiveEntry entry )
        {
            return entry.FullName.EndsWith ( ".fpage", StringComparison.OrdinalIgnoreCase );
        }

        // TODO: Improve page finding
        private static ZipArchiveEntry FindPage ( ZipArchive document, int page )
        {
            return document.GetEntry ( $"/Documents/1/Pages/{ page + 1 }.fpage" );
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