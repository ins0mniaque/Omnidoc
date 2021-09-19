using System;
using System.Threading;
using System.Threading.Tasks;

using PDFiumCore;

using Omnidoc.Core;
using Omnidoc.Core.Disposables;

namespace Omnidoc.Pdf
{
    using static PDFiumCore.fpdfview;

    public sealed class PdfPager < T > : AsyncDisposable, IPager < T >
    {
        public PdfPager ( FpdfDocumentT document, Func < FpdfPageT, T > factory )
        {
            Document = document;
            Factory  = factory;
        }

        public PdfPager ( FpdfDocumentT document, IDisposable disposable, Func < FpdfPageT, T > factory )
        {
            Document   = document;
            Disposable = disposable;
            Factory    = factory;
        }

        public  FpdfDocumentT         Document   { get; }
        private IDisposable?          Disposable { get; }
        private Func < FpdfPageT, T > Factory    { get; }

        public Task < int > GetPageCountAsync ( CancellationToken cancellationToken )
        {
            return Task.FromResult ( FPDF_GetPageCount ( Document ) );
        }

        public Task < T > GetPageAsync ( int index, CancellationToken cancellationToken )
        {
            return Task.FromResult ( Factory ( FPDF_LoadPage ( Document, index ) ) );
        }

        protected override void Dispose ( bool disposing )
        {
            if ( disposing )
            {
                FPDF_CloseDocument  ( Document );
                Disposable?.Dispose ( );
            }
        }
    }
}