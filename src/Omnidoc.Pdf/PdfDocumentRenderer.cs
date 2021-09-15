using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Omnidoc.Services;

namespace Omnidoc.Pdf
{
    using static PDFiumCore.fpdfview;

    public class PdfDocumentRenderer : IDocumentRenderer
    {
        public IReadOnlyCollection < DocumentType > Types { get; } = new [ ] { DocumentTypes.Pdf };

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