using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using PdfSharpCore;

using Omnidoc.Core;
using Omnidoc.HtmlRenderer.PdfSharp;
using Omnidoc.Services;

namespace Omnidoc.HtmlToPdf
{
    public class HtmlToPdfFormatConverter : IFileFormatConverter
    {
        private static readonly IServiceDescriptor descriptor = new ServiceDescriptor
        (
            new [ ] { FileFormats.Html },
            new [ ] { FileFormats.Pdf }
        );

        public IServiceDescriptor Descriptor => descriptor;

        public async Task ConvertAsync ( Stream file, Stream output, OutputOptions options, CancellationToken cancellationToken = default )
        {
            if ( file    is null ) throw new ArgumentNullException ( nameof ( file    ) );
            if ( output  is null ) throw new ArgumentNullException ( nameof ( output  ) );
            if ( options is null ) throw new ArgumentNullException ( nameof ( options ) );

            using var reader = new StreamReader ( file );

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