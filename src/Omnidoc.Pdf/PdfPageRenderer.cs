using System.IO;
using System.Threading;
using System.Threading.Tasks;

using PDFiumCore;

using Omnidoc.Core;

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

        public Size PageSize => new Size ( FPDF_GetPageWidth  ( Page ),
                                           FPDF_GetPageHeight ( Page ) );

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