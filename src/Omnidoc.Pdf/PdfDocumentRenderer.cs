using PDFiumCore;

using Omnidoc.Core;
using Omnidoc.Core.Disposables;
using Omnidoc.Services;

namespace Omnidoc.Pdf
{
    public class PdfDocumentRenderer : PdfLoader < IPager < IPageRenderer > >, IDocumentRenderer
    {
        private static readonly IServiceDescriptor descriptor = new ServiceDescriptor
        (
            new [ ] { FileFormats.Pdf },
            new [ ] { FileFormats.Bmp }
        );

        public PdfDocumentRenderer ( ) : base ( CreatePager ) { }

        public IServiceDescriptor Descriptor => descriptor;

        private static IPager < IPageRenderer > CreatePager ( Disposable < FpdfDocumentT > document )
        {
            return new PdfPager < IPageRenderer > ( document, page => new PdfPageRenderer ( page ) );
        }
    }
}