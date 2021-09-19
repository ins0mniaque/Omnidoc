using System.IO;
using System.Threading;
using System.Threading.Tasks;

using PDFiumCore;

using Omnidoc.Core;
using Omnidoc.Core.Disposables;

namespace Omnidoc.Pdf
{
    using static PDFiumCore.fpdfview;

    public sealed class PdfPageRenderer : AsyncDisposable, IPageRenderer
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

        protected override void Dispose ( bool disposing )
        {
            if ( disposing )
                FPDF_ClosePage ( Page );
        }
    }
}