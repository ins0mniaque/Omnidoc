using System;
using System.Threading;
using System.Threading.Tasks;

using PDFiumCore;

using Omnidoc.Services;

namespace Omnidoc.Pdf
{
    using static PDFiumCore.fpdfview;

    public sealed class PdfDocumentPager < T > : IDocumentPager < T >
    {
        public PdfDocumentPager ( FpdfDocumentT document, Func < FpdfPageT, T > factory )
        {
            Document = document;
            Factory  = factory;
        }

        public  FpdfDocumentT         Document { get; }
        private Func < FpdfPageT, T > Factory  { get; }

        public Task < int > GetPageCountAsync ( CancellationToken cancellationToken )
        {
            return Task.FromResult ( FPDF_GetPageCount ( Document ) );
        }

        public Task < T > GetPageAsync ( int page, CancellationToken cancellationToken )
        {
            return Task.FromResult ( Factory ( FPDF_LoadPage ( Document, page ) ) );
        }

        private bool closed;

        public void Dispose ( )
        {
            if ( ! closed )
            {
                FPDF_CloseDocument ( Document );

                closed = true;
            }
        }
    }
}