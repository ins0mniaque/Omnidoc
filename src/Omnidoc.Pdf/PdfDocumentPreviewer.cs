using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using PDFiumSharp;
using PDFiumSharp.Types;

using Omnidoc.Services;

namespace Omnidoc.Pdf
{
    public class PdfDocumentPreviewer : IDocumentPreviewer
    {
        public IReadOnlyCollection < DocumentType > Types { get; } = new [ ] { DocumentTypes.Pdf };

        public async Task PreviewAsync ( Stream document, Stream output, RenderingOptions options, CancellationToken cancellationToken = default )
        {
            using var pdf = new PdfDocument ( document, FPDF_FILEREAD.FromStream ( document ) );

            await pdf.Pages [ 0 ].RenderAsync    ( output, options, cancellationToken )
                                 .ConfigureAwait ( false );
        }
    }
}