using System.Threading;
using System.Threading.Tasks;

using PDFiumSharp;

using Omnidoc.Services;

namespace Omnidoc.Pdf
{
    public sealed class PdfDocumentRendering : IDocumentRendering
    {
        public PdfDocumentRendering ( PdfDocument document )
        {
            Document = document;
        }

        public PdfDocument Document { get; }

        public Task < int > GetPageCountAsync ( CancellationToken cancellationToken )
        {
            return Task.FromResult ( Document.Pages.Count );
        }

        public Task < IDocumentPageRenderer > GetPageRendererAsync ( int page, CancellationToken cancellationToken )
        {
            return Task.FromResult < IDocumentPageRenderer > ( new PdfDocumentPageRenderer ( Document.Pages [ page ] ) );
        }

        public void Dispose ( )
        {
            if ( ! Document.IsDisposed )
                Document.Close ( );
        }
    }
}