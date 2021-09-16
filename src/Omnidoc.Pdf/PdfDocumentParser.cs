using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Omnidoc.Model;
using Omnidoc.Services;

namespace Omnidoc.Pdf
{
    using static PDFiumCore.fpdfview;

    public class PdfDocumentParser : IDocumentParser
    {
        private static readonly IDocumentServiceDescriptor descriptor = new DocumentServiceDescriptor
        (
            new [ ] { DocumentTypes.Pdf },
            new [ ] { typeof ( Content ) }
        );

        public IDocumentServiceDescriptor Descriptor => descriptor;

        public Task < IPager < IPageParser > > PrepareAsync ( Stream document, CancellationToken cancellationToken = default )
        {
            return Task.Run ( ( ) => Prepare ( document ), cancellationToken );
        }

        private IPager < IPageParser > Prepare ( Stream document )
        {
            var fileAccess = document.ToFileAccess ( );

            return new PdfPager < IPageParser > ( FPDF_LoadCustomDocument ( fileAccess, null ), fileAccess,
                                                  page => new PdfPageParser ( page ) );
        }
    }
}