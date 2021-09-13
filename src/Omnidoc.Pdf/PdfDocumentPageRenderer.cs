using System.IO;
using System.Threading;
using System.Threading.Tasks;

using PDFiumSharp;

using Omnidoc.Services;

namespace Omnidoc.Pdf
{
    public sealed class PdfDocumentPageRenderer : IDocumentPageRenderer
    {
        public PdfDocumentPageRenderer ( PdfPage page )
        {
            Page = page;
        }

        public PdfPage Page { get; }

        public double PageWidth  => Page.Width;
        public double PageHeight => Page.Height;

        public Task RenderAsync ( Stream output, RenderingOptions options, CancellationToken cancellationToken = default )
        {
            return Page.RenderAsync ( output, options, cancellationToken );
        }

        public void Dispose ( )
        {
            if ( ! Page.IsDisposed )
                Page.Dispose ( );
        }
    }
}