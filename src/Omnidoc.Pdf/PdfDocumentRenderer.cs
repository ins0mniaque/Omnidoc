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

        public Task < IDocumentRendering > PrepareAsync ( Stream document, CancellationToken cancellationToken = default )
        {
            return Task.Run ( ( ) => Prepare ( document ), cancellationToken );
        }

        private static IDocumentRendering Prepare ( Stream document )
        {
            using var fileAccess = document.ToFileAccess ( );

            return new PdfDocumentRendering ( FPDF_LoadCustomDocument ( fileAccess, null ) );
        }
    }
}