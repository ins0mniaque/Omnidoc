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
        private static readonly IDocumentServiceDescriptor descriptor = new DocumentServiceDescriptor
        (
            new [ ] { DocumentTypes.Pdf },
            new [ ] { DocumentTypes.Bmp }
        );

        public IDocumentServiceDescriptor Descriptor => descriptor;

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