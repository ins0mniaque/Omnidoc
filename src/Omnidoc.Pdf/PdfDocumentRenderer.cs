using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using PDFiumSharp;
using PDFiumSharp.Types;

using Omnidoc.Services;

namespace Omnidoc.Pdf
{
    public class PdfDocumentRenderer : IDocumentRenderer
    {
        public IReadOnlyCollection < DocumentType > Types { get; } = new [ ] { DocumentTypes.Pdf };

        public Task < IDocumentRendering > PrepareAsync ( Stream document, CancellationToken cancellationToken = default )
        {
            return Task.Run ( ( ) => Prepare ( document ), cancellationToken );
        }

        private static IDocumentRendering Prepare ( Stream document )
        {
            var pdf = new PdfDocument ( document, FPDF_FILEREAD.FromStream ( document ) );

            return new PdfDocumentRendering ( pdf );
        }
    }
}