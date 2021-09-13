using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using PdfSharpCore;

using Omnidoc.HtmlRenderer.PdfSharp;
using Omnidoc.Services;

namespace Omnidoc.HtmlToPdf
{
    public class HtmlToPdfDocumentConverter : IDocumentConverter
    {
        public IReadOnlyCollection < DocumentType > Types       { get; } = new [ ] { DocumentTypes.Html };
        public IReadOnlyCollection < DocumentType > OutputTypes { get; } = new [ ] { DocumentTypes.Pdf  };

        public async Task ConvertAsync ( Stream document, Stream output, OutputOptions options, CancellationToken cancellationToken = default )
        {
            if ( document is null ) throw new ArgumentNullException ( nameof ( document ) );
            if ( output   is null ) throw new ArgumentNullException ( nameof ( output   ) );
            if ( options  is null ) throw new ArgumentNullException ( nameof ( options  ) );

            using var reader = new StreamReader ( document );

            var html = await reader.ReadToEndAsync ( ).ConfigureAwait ( false );

            await Task.Run ( ( ) => Convert ( html, output, cancellationToken ), cancellationToken ).ConfigureAwait ( false );
        }

        private static void Convert ( string html, Stream output, CancellationToken cancellationToken = default )
        {
            // TODO: Extract page size and margins from CSS (e.g. @page { size = A4 landscape, margin: 1cm })
            var config = new PdfGenerateConfig ( )
            {
                PageSize        = PageSize.A4,
                PageOrientation = PageOrientation.Portrait,
                MarginLeft      = 24,
                MarginTop       = 72,
                MarginRight     = 24,
                MarginBottom    = 72
            };

            using var pdf = PdfGenerator.GeneratePdf ( html, config );

            cancellationToken.ThrowIfCancellationRequested ( );

            pdf.Save ( output );
        }
    }
}