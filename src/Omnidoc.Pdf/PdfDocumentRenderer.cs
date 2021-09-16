using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Omnidoc.Services;

namespace Omnidoc.Pdf
{
    using static PDFiumCore.fpdfview;

    public class PdfDocumentRenderer : IDocumentRenderer
    {
        private static readonly IDocumentServiceDescriptor descriptor = new DocumentServiceDescriptor
        (
            new [ ] { DocumentTypes.Pdf },
            new [ ] { DocumentTypes.Bmp }
        );

        public IDocumentServiceDescriptor Descriptor => descriptor;

        public Task < IPager < IPageRenderer > > PrepareAsync ( Stream document, CancellationToken cancellationToken = default )
        {
            return Task.Run ( ( ) => Prepare ( document ), cancellationToken );
        }

        private static IPager < IPageRenderer > Prepare ( Stream document )
        {
            var fileAccess = document.ToFileAccess ( );

            return new PdfPager < IPageRenderer > ( FPDF_LoadCustomDocument ( fileAccess, null ), fileAccess,
                                                    page => new PdfPageRenderer ( page ) );
        }
    }
}