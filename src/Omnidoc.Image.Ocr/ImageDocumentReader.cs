using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using Tesseract;

using Omnidoc.Content;
using Omnidoc.Services;

namespace Omnidoc.Image
{
    public class ImageDocumentReader : IDocumentReader
    {
        public static Func < TesseractEngine > CreateDefaultEngine { get; set; } = ( ) => new TesseractEngine ( @"tessdata", "eng" );

        public ImageDocumentReader ( ) : this ( CreateDefaultEngine ) { }
        public ImageDocumentReader ( Func < TesseractEngine > createEngine )
        {
            CreateEngine = createEngine;
        }

        private Func < TesseractEngine > CreateEngine { get; }

        public IReadOnlyCollection < DocumentType > Types { get; } = new [ ] { DocumentTypes.Bmp, DocumentTypes.Gif, DocumentTypes.Jpeg, DocumentTypes.Png, DocumentTypes.Tga, DocumentTypes.Tiff };

        public async IAsyncEnumerable < DocumentContent > ReadAsync ( Stream document, [ EnumeratorCancellation ] CancellationToken cancellationToken = default )
        {
            using var engine   = CreateEngine ( );
            using var contents = new BlockingCollection < DocumentContent > ( );

            var reading = Task.Run ( ( ) => ReadWords ( engine, document, contents, cancellationToken ), cancellationToken );

            foreach ( var content in contents )
                yield return content;

            await reading.ConfigureAwait ( false );
        }

        private static void ReadWords ( TesseractEngine engine, Stream document, BlockingCollection < DocumentContent > contents, CancellationToken cancellationToken )
        {
            using var buffer = new MemoryStream ( );

            document.CopyTo ( buffer );

            cancellationToken.ThrowIfCancellationRequested ( );

            using var image = Pix.LoadFromMemory ( buffer.ToArray ( ) );

            cancellationToken.ThrowIfCancellationRequested ( );

            using var page     = engine.Process ( image );
            using var iterator = page.GetIterator ( );

            iterator.Begin ( );

            do
            {
                do
                {
                    do
                    {
                        do
                        {
                            cancellationToken.ThrowIfCancellationRequested ( );

                            contents.Add ( ExtractWord ( iterator ) );
                        }
                        while ( iterator.Next ( PageIteratorLevel.TextLine, PageIteratorLevel.Word ) );
                    }
                    while ( iterator.Next ( PageIteratorLevel.Para, PageIteratorLevel.TextLine ) );
                }
                while ( iterator.Next ( PageIteratorLevel.Block, PageIteratorLevel.Para ) );
            }
            while ( iterator.Next ( PageIteratorLevel.Block ) );
        }

        private static DocumentText ExtractWord ( ResultIterator iterator )
        {
            var startBlock     = iterator.IsAtBeginningOf ( PageIteratorLevel.Block    );
            var startParagraph = iterator.IsAtBeginningOf ( PageIteratorLevel.Para     );
            var startLine      = iterator.IsAtBeginningOf ( PageIteratorLevel.TextLine );
            var endBlock       = iterator.IsAtFinalOf     ( PageIteratorLevel.Block,    PageIteratorLevel.Word );
            var endParagraph   = iterator.IsAtFinalOf     ( PageIteratorLevel.Para,     PageIteratorLevel.Word );
            var endLine        = iterator.IsAtFinalOf     ( PageIteratorLevel.TextLine, PageIteratorLevel.Word );
            var markers        = ( startBlock     ? DocumentMarkers.StartBlock     : DocumentMarkers.None ) |
                                 ( startParagraph ? DocumentMarkers.StartParagraph : DocumentMarkers.None ) |
                                 ( startLine      ? DocumentMarkers.StartLine      : DocumentMarkers.None ) |
                                 ( endBlock       ? DocumentMarkers.EndBlock       : DocumentMarkers.None ) |
                                 ( endParagraph   ? DocumentMarkers.EndParagraph   : DocumentMarkers.None ) |
                                 ( endLine        ? DocumentMarkers.EndLine        : DocumentMarkers.None );

            var font    = iterator.GetWordFontAttributes ( );
            var content = new DocumentText ( iterator.GetText ( PageIteratorLevel.Word ) )
            {
                Font       = font.FontInfo.Name,
                FontSize   = font.PointSize,
                FontWeight = font.FontInfo.IsBold ? 700 : 400,
                Markers    = markers
            };

            if ( iterator.TryGetBoundingBox ( PageIteratorLevel.Word, out var bounds ) )
            {
                content.Left   = bounds.X1;
                content.Top    = bounds.Y1;
                content.Right  = bounds.X2;
                content.Bottom = bounds.Y2;
            }

            return content;
        }
    }
}