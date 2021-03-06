using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using PdfSharpCore;

using Omnidoc.Core;
using Omnidoc.Core.Disposables;
using Omnidoc.Html.Pdf.Renderer;
using Omnidoc.Services;

namespace Omnidoc.Html.Pdf
{
    public class HtmlToPdfFormatConverter : AsyncDisposable, IFileFormatConverter
    {
        private static readonly IServiceDescriptor descriptor = new ServiceDescriptor
        (
            new [ ] { Html.FileFormats.Html },
            new [ ] { FileFormats.Pdf }
        );

        public IServiceDescriptor Descriptor => descriptor;

        public async Task ConvertAsync ( Stream input, Stream output, OutputOptions options, CancellationToken cancellationToken = default )
        {
            if ( input   is null ) throw new ArgumentNullException ( nameof ( input   ) );
            if ( output  is null ) throw new ArgumentNullException ( nameof ( output  ) );
            if ( options is null ) throw new ArgumentNullException ( nameof ( options ) );

            using var reader = new StreamReader ( input );

            var html = await reader.ReadToEndAsync ( ).ConfigureAwait ( false );

            await Task.Run ( ( ) => Convert ( html, output, cancellationToken ), cancellationToken ).ConfigureAwait ( false );
        }

        private static void Convert ( string html, Stream output, CancellationToken cancellationToken = default )
        {
            // TODO: Extract page size and margins from CSS (e.g. @page { size = A4 landscape, margin: 1cm })
            var options = new PdfPageOptions ( )
            {
                PageSize        = PageSize.A4,
                PageOrientation = PageOrientation.Portrait,
                MarginLeft      = 24,
                MarginTop       = 72,
                MarginRight     = 24,
                MarginBottom    = 72
            };

            using var pdf = PdfGenerator.FromHtml ( html, options );

            cancellationToken.ThrowIfCancellationRequested ( );

            pdf.Save ( output );
        }
    }
}