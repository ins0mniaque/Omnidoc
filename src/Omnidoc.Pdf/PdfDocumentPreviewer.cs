﻿using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Omnidoc.Core;
using Omnidoc.Interop;
using Omnidoc.Services;

namespace Omnidoc.Pdf
{
    using static PDFiumCore.fpdfview;

    public class PdfDocumentPreviewer : IDocumentPreviewer
    {
        private static readonly IServiceDescriptor descriptor = new ServiceDescriptor
        (
            new [ ] { FileFormats.Pdf },
            new [ ] { FileFormats.Bmp }
        );

        public IServiceDescriptor Descriptor => descriptor;

        public async Task < bool > TryPreviewAsync ( Stream document, Stream output, RenderingOptions options, CancellationToken cancellationToken = default )
        {
            using var fileAccess = document.ToFileAccess ( );

            using var pdf  = FPDF_LoadCustomDocument ( fileAccess, null ).AsDisposable ( FPDF_CloseDocument );
            using var page = FPDF_LoadPage           ( pdf, 0 )          .AsDisposable ( FPDF_ClosePage     );

            await page.RenderAsync    ( output, options, cancellationToken )
                      .ConfigureAwait ( false );

            return true;
        }
    }
}