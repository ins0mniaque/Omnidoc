using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using PDFiumCore;

using Omnidoc.Content;
using Omnidoc.Services;

namespace Omnidoc.Pdf
{
    using static PDFiumCore.fpdfview;
    using static PDFiumCore.fpdf_text;

    public class PdfDocumentParser : IDocumentParser
    {
        public IReadOnlyCollection < DocumentType > Types { get; } = new [ ] { DocumentTypes.Pdf };

        public async IAsyncEnumerable < DocumentContent > ParseAsync ( Stream document, [ EnumeratorCancellation ] CancellationToken cancellationToken )
        {
            using var contents = new BlockingCollection < DocumentContent > ( );

            var reading = Task.Run ( ( ) => Parse ( document, contents, cancellationToken ), cancellationToken );

            foreach ( var content in contents )
                yield return content;

            await reading.ConfigureAwait ( false );
        }

        private static void Parse ( Stream document, BlockingCollection < DocumentContent > contents, CancellationToken cancellationToken )
        {
            using var fileAccess = document.ToFileAccess ( );

            using var pdf = FPDF_LoadCustomDocument ( fileAccess, null ).AsDisposable ( FPDF_CloseDocument );

            var pageCount = FPDF_GetPageCount ( pdf );

            for ( var index = 0; index < pageCount; index++ )
            {
                cancellationToken.ThrowIfCancellationRequested ( );

                using var page = FPDF_LoadPage ( pdf, index ).AsDisposable ( FPDF_ClosePage );
                foreach ( var content in ParsePage ( page, cancellationToken ) )
                    contents.Add ( content );
            }

            contents.CompleteAdding ( );
        }

        private static IEnumerable < DocumentContent > ParsePage ( FpdfPageT page, CancellationToken cancellationToken )
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

                var text = PdfString.Alloc ( (ref ushort buffer, int length) => FPDFTextGetBoundedText ( textPage, left, top, right, bottom, ref buffer, length ) );

                yield return new DocumentText ( text )
                {
                    Left       = left,
                    Top        = top,
                    Right      = right,
                    Bottom     = bottom,
                    Color      = $"#{a:x2}{r:x2}{g:x2}{b:x2}",
                    Font       = font,
                    FontSize   = FPDFTextGetFontSize   ( textPage, index ),
                    FontWeight = FPDFTextGetFontWeight ( textPage, index )
                };
            }
        }
    }
}