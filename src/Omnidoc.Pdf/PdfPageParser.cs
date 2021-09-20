using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using PDFiumCore;

using Omnidoc.Core;
using Omnidoc.Core.Disposables;
using Omnidoc.Model;
using Omnidoc.Model.Elements;

namespace Omnidoc.Pdf
{
    using static PDFiumCore.fpdfview;
    using static PDFiumCore.fpdf_text;

    public class PdfPageParser : AsyncDisposable, IPageParser
    {
        public PdfPageParser ( FpdfPageT page )
        {
            Page = page;
        }

        public FpdfPageT Page { get; }

        public async IAsyncEnumerable < Element > ParseAsync ( ParserOptions options, [ EnumeratorCancellation ] CancellationToken cancellationToken )
        {
            using var elements = new BlockingCollection < Element > ( );

            var reading = Task.Run ( ( ) => Parse ( Page, elements, cancellationToken ), cancellationToken );

            foreach ( var element in elements )
                yield return element;

            await reading.ConfigureAwait ( false );
        }

        private static void Parse ( FpdfPageT page, BlockingCollection < Element > elements, CancellationToken cancellationToken )
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
                var element = new Glyphs ( text )
                {
                    Position = new Point ( left, top ),
                    Size     = new Size  ( right - left, bottom - top ),
                    Fill     = $"#{a:x2}{r:x2}{g:x2}{b:x2}",
                    Font     = new Font { Name   = font,
                                          Size   = FPDFTextGetFontSize   ( textPage, index ),
                                          Weight = FPDFTextGetFontWeight ( textPage, index ) }
                };

                elements.Add ( element, cancellationToken );
            }

            elements.CompleteAdding ( );
        }

        protected override void Dispose ( bool disposing )
        {
            if ( disposing )
                FPDF_ClosePage ( Page );

            base.Dispose ( disposing );
        }
    }
}