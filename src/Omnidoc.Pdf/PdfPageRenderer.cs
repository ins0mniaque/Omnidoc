using System.IO;
using System.Threading;
using System.Threading.Tasks;

using PDFiumCore;

using Omnidoc.Services;

namespace Omnidoc.Pdf
{
    using static PDFiumCore.fpdfview;

    public sealed class PdfPageRenderer : IPageRenderer
    {
        public PdfPageRenderer ( FpdfPageT page )
        {
            Page = page;
        }

        public FpdfPageT Page { get; }

        public double PageWidth  => FPDF_GetPageWidth  ( Page );
        public double PageHeight => FPDF_GetPageHeight ( Page );

        public Task RenderAsync ( Stream output, RenderingOptions options, CancellationToken cancellationToken = default )
        {
            return Page.RenderAsync ( output, options, cancellationToken );
        }

        private bool isDisposed;

        public void Dispose ( )
        {
            if ( isDisposed )
                return;

            FPDF_ClosePage ( Page );

            isDisposed = true;
        }
    }
}