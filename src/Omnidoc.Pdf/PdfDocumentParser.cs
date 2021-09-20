using PDFiumCore;

using Omnidoc.Core;
using Omnidoc.Core.Disposables;
using Omnidoc.Model;
using Omnidoc.Services;

namespace Omnidoc.Pdf
{
    public class PdfDocumentParser : PdfLoader < IPager < IPageParser > >, IDocumentParser
    {
        private static readonly IServiceDescriptor descriptor = new ServiceDescriptor
        (
            new [ ] { FileFormats.Pdf },
            new [ ] { typeof ( Element ) }
        );

        public PdfDocumentParser ( ) : base ( CreatePager ) { }

        public IServiceDescriptor Descriptor => descriptor;

        private static IPager < IPageParser > CreatePager ( Disposable < FpdfDocumentT > document )
        {
            return new PdfPager < IPageParser > ( document, page => new PdfPageParser ( page ) );
        }
    }
}