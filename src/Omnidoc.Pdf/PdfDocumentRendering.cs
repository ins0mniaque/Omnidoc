using System.Threading;
using System.Threading.Tasks;

using PDFiumCore;

using Omnidoc.Services;

namespace Omnidoc.Pdf
{
    using static PDFiumCore.fpdfview;

    public sealed class PdfDocumentRendering : IDocumentRendering
    {
        public PdfDocumentRendering ( FpdfDocumentT document )
        {
            Document = document;
        }

        public FpdfDocumentT Document { get; }

        public Task < int > GetPageCountAsync ( CancellationToken cancellationToken )
        {
            return Task.FromResult ( FPDF_GetPageCount ( Document ) );
        }

        public Task < IDocumentPageRenderer > GetPageRendererAsync ( int page, CancellationToken cancellationToken )
        {
            return Task.FromResult < IDocumentPageRenderer > ( new PdfDocumentPageRenderer ( FPDF_LoadPage ( Document, page ) ) );
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