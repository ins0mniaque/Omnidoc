using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using PDFiumSharp;
using PDFiumSharp.Types;

using Omnidoc.Content;
using Omnidoc.Services;

namespace Omnidoc.Pdf
{
    public class PdfDocumentReader : IDocumentReader
    {
        public IReadOnlyCollection < DocumentType > Types { get; } = new [ ] { DocumentTypes.Pdf };

        public async IAsyncEnumerable < DocumentContent > ReadAsync ( Stream document, [ EnumeratorCancellation ] CancellationToken cancellationToken )
        {
            using var contents = new BlockingCollection < DocumentContent > ( );

            var reading = Task.Run ( ( ) => Read ( document, contents, cancellationToken ), cancellationToken );

            foreach ( var content in contents )
                yield return content;

            await reading.ConfigureAwait ( false );
        }

        private static void Read ( Stream document, BlockingCollection < DocumentContent > contents, CancellationToken cancellationToken )
        {
            using var pdf = new PdfDocument ( document, FPDF_FILEREAD.FromStream ( document ) );

            foreach ( var page in pdf.Pages )
            {
                cancellationToken.ThrowIfCancellationRequested ( );

                foreach ( var content in ReadPage ( page, cancellationToken ) )
                    contents.Add ( content );
            }

            contents.CompleteAdding ( );
        }

        private static IEnumerable < DocumentContent > ReadPage ( PdfPage page, CancellationToken cancellationToken )
        {
            var textPage = PDFium.FPDFText_LoadPage ( page.Handle );

            try
            {
                var count = PDFium.FPDFText_CountRects ( textPage, 0, 1000000 );

                for ( var index = 0; index < count; index++ )
                {
                    cancellationToken.ThrowIfCancellationRequested ( );

                    var zero   = (byte) 0;
                    var length = PDFium.FPDFText_GetFontInfo ( textPage, index, ref zero, 0, out var flags );
                    var font   = new byte [ length ];

                    PDFium.FPDFText_GetFontInfo  ( textPage, index, ref font [ 0 ], (uint) font.Length, out flags );
                    PDFium.FPDFText_GetRect      ( textPage, index, out var left, out var top, out var right, out var bottom );
                    PDFium.FPDFText_GetFillColor ( textPage, index, out var r,    out var g,   out var b,     out var a      );

                    yield return new DocumentText ( PDFium.FPDFText_GetBoundedText ( textPage, left, top, right, bottom ) )
                    {
                        Left       = left,
                        Top        = top,
                        Right      = right,
                        Bottom     = bottom,
                        Color      = $"#{a:x2}{r:x2}{g:x2}{b:x2}",
                        Font       = Encoding.Unicode.GetString    ( font ),
                        FontSize   = PDFium.FPDFText_GetFontSize   ( textPage, index ),
                        FontWeight = PDFium.FPDFText_GetFontWeight ( textPage, index )
                    };
                }
            }
            finally
            {
                PDFium.FPDFText_ClosePage ( textPage );
            }
        }
    }
}