using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using PDFiumSharp;

namespace Omnidoc.Pdf
{
    public static class PdfRenderer
    {
        public static void Render ( this PdfPage page, Stream output, RenderingOptions options )
        {
            if ( page    is null ) throw new ArgumentNullException ( nameof ( page    ) );
            if ( output  is null ) throw new ArgumentNullException ( nameof ( output  ) );
            if ( options is null ) throw new ArgumentNullException ( nameof ( options ) );

            var dpi    = options.Dpi ?? 96;
            var width  = ( options.Width  ?? page.Width  ) / 72.0 * dpi;
            var height = ( options.Height ?? page.Height ) / 72.0 * dpi;

            using var image = new PDFiumBitmap ( (int) width, (int) height, true );

            page .Render ( image );
            image.Save   ( output, dpi, dpi );
        }

        public static Task RenderAsync ( this PdfPage page, Stream output, RenderingOptions options, CancellationToken cancellationToken = default )
        {
            return Task.Run ( ( ) => page.Render ( output, options ), cancellationToken );
        }
    }
}