using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Omnidoc.Interop;
using Omnidoc.Services;

namespace Omnidoc.Pdf
{
    using static PDFiumCore.fpdfview;

    public class PdfDocumentPreviewer : IDocumentPreviewer
    {
        public IReadOnlyCollection < DocumentType > Types { get; } = new [ ] { DocumentTypes.Pdf };

        public async Task PreviewAsync ( Stream document, Stream output, RenderingOptions options, CancellationToken cancellationToken = default )
        {
            using var fileAccess = document.ToFileAccess ( );

            using var pdf  = FPDF_LoadCustomDocument ( fileAccess, null ).AsDisposable ( FPDF_CloseDocument );
            using var page = FPDF_LoadPage           ( pdf, 0 )          .AsDisposable ( FPDF_ClosePage     );

            await page.RenderAsync    ( output, options, cancellationToken )
                      .ConfigureAwait ( false );
        }
    }
}