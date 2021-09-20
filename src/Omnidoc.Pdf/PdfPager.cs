using System;
using System.Threading;
using System.Threading.Tasks;

using PDFiumCore;

using Omnidoc.Core;
using Omnidoc.Core.Disposables;

namespace Omnidoc.Pdf
{
    using static PDFiumCore.fpdfview;

    public class PdfPager < T > : Pager < FpdfDocumentT, Disposable < FpdfPageT >, T >
    {
        public PdfPager ( FpdfDocumentT                document, Func < Disposable < FpdfPageT >, T > factory ) : base ( document, factory ) { }
        public PdfPager ( Disposable < FpdfDocumentT > document, Func < Disposable < FpdfPageT >, T > factory ) : base ( document, factory ) { }

        public override Task < int > GetPageCountAsync ( CancellationToken cancellationToken )
        {
            return Task.FromResult ( FPDF_GetPageCount ( Document ) );
        }

        protected override Task < Disposable < FpdfPageT >? > LoadPageAsync ( int index, CancellationToken cancellationToken )
        {
            return Task.Run ( ( ) => FPDF_LoadPage ( Document, index )?.AsDisposable ( FPDF_ClosePage ), cancellationToken );
        }
    }
}