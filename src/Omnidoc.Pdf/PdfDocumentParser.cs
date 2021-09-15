using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Omnidoc.Services;

namespace Omnidoc.Pdf
{
    using static PDFiumCore.fpdfview;

    public class PdfDocumentParser : IDocumentParser
    {
        public IReadOnlyCollection < DocumentType > Types { get; } = new [ ] { DocumentTypes.Pdf };

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