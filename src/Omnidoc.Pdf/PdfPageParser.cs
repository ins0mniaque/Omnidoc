using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using PDFiumCore;

using Omnidoc.Core;
using Omnidoc.Interop;
using Omnidoc.Model;

namespace Omnidoc.Pdf
{
    using static PDFiumCore.fpdfview;
    using static PDFiumCore.fpdf_text;

    public sealed class PdfPageParser : IPageParser
    {
        public PdfPageParser ( FpdfPageT page )
        {
            Page = page;
        }

        public FpdfPageT Page { get; }

        public async IAsyncEnumerable < Content > ParseAsync ( ParserOptions options, [ EnumeratorCancellation ] CancellationToken cancellationToken )
        {
            using var contents = new BlockingCollection < Content > ( );

            var reading = Task.Run ( ( ) => Parse ( Page, contents, cancellationToken ), cancellationToken );

            foreach ( var content in contents )
                yield return content;

            await reading.ConfigureAwait ( false );
        }

        private static void Parse ( FpdfPageT page, BlockingCollection < Content > contents, CancellationToken cancellationToken )
        {
            using var textPage = FPDFTextLoadPage ( page ).AsDisposable ( FPDFTextClosePage );

            var count = FPDFTextCountRects ( textPage, 0, 1000000 );

            for ( var index = 0; index < count; index++ )
            {
                cancellationToken.ThrowIfCancellationRequested ( );

                var flags = 0;
                var font  = PdfString.Alloc ( (buffer, length) => (int) FPDFTextGetFontInfo ( textPage, index, buffer, (uint) length, ref flags ), Encoding.UTF8 );

                double left = 0, top = 0, right = 0, bottom = 0;
                FPDFTextGetRect ( textPage, index, ref left, ref top, ref right, ref bottom );

                uint r = 0, g = 0, b = 0, a = 255;
                FPDFTextGetFillColor ( textPage, index, ref r, ref g, ref b, ref a );

                var text    = PdfString.Alloc ( (ref ushort buffer, int length) => FPDFTextGetBoundedText ( textPage, left, top, right, bottom, ref buffer, length ) );
                var content = new Glyphs ( text )
                {
                    Position = new Point ( left, top ),
                    Size     = new Size  ( right - left, bottom - top ),
                    Fill     = $"#{a:x2}{r:x2}{g:x2}{b:x2}",
                    Font     = new Font { Name   = font,
                                          Size   = FPDFTextGetFontSize   ( textPage, index ),
                                          Weight = FPDFTextGetFontWeight ( textPage, index ) }
                };

                contents.Add ( content );
            }

            contents.CompleteAdding ( );
        }

        private bool isDisposed;

        public void Dispose ( )
        {
            if ( isDisposed )
                return;

            FPDF_ClosePage ( Page );

            isDisposed = true;
        }
    }
}