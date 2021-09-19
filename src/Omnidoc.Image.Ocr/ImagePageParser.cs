using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

using Tesseract;

using Omnidoc.Core;
using Omnidoc.Core.Disposables;
using Omnidoc.Model;
using Omnidoc.Model.Elements;

namespace Omnidoc.Image
{
    public class ImagePageParser : AsyncDisposable, IPageParser
    {
        public ImagePageParser ( TesseractEngine engine, Stream page )
        {
            Engine = engine;
            Page   = page;
        }

        public TesseractEngine Engine { get; }
        public Stream          Page   { get; }

        public async IAsyncEnumerable < Element > ParseAsync ( ParserOptions options, [ EnumeratorCancellation ] CancellationToken cancellationToken = default )
        {
            using var elements = new BlockingCollection < Element > ( );

            var reading = Task.Run ( ( ) => ParseWords ( Engine, Page, elements, cancellationToken ), cancellationToken );

            foreach ( var element in elements )
                yield return element;

            await reading.ConfigureAwait ( false );
        }

        private static void ParseWords ( TesseractEngine engine, Stream stream, BlockingCollection < Element > elements, CancellationToken cancellationToken )
        {
            using var buffer = new MemoryStream ( );

            stream.CopyTo ( buffer );

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

                            elements.Add ( ParseWord ( iterator ), cancellationToken );
                        }
                        while ( iterator.Next ( PageIteratorLevel.TextLine, PageIteratorLevel.Word ) );
                    }
                    while ( iterator.Next ( PageIteratorLevel.Para, PageIteratorLevel.TextLine ) );
                }
                while ( iterator.Next ( PageIteratorLevel.Block, PageIteratorLevel.Para ) );
            }
            while ( iterator.Next ( PageIteratorLevel.Block ) );
        }

        private static Glyphs ParseWord ( ResultIterator iterator )
        {
            var blockStart     = iterator.IsAtBeginningOf ( PageIteratorLevel.Block );
            var blockEnd       = iterator.IsAtFinalOf     ( PageIteratorLevel.Block,    PageIteratorLevel.Word );
            var paragraphStart = iterator.IsAtBeginningOf ( PageIteratorLevel.Para );
            var paragraphEnd   = iterator.IsAtFinalOf     ( PageIteratorLevel.Para,     PageIteratorLevel.Word );
            var lineStart      = iterator.IsAtBeginningOf ( PageIteratorLevel.TextLine );
            var lineEnd        = iterator.IsAtFinalOf     ( PageIteratorLevel.TextLine, PageIteratorLevel.Word );
            var levels         = ( blockStart     ? Levels.BlockStart     : Levels.None ) |
                                 ( blockEnd       ? Levels.BlockEnd       : Levels.None ) |
                                 ( paragraphStart ? Levels.ParagraphStart : Levels.None ) |
                                 ( paragraphEnd   ? Levels.ParagraphEnd   : Levels.None ) |
                                 ( lineStart      ? Levels.LineStart      : Levels.None ) |
                                 ( lineEnd        ? Levels.LineEnd        : Levels.None ) |
                                 Levels.WordStart | Levels.WordEnd;

            var font    = iterator.GetWordFontAttributes ( );
            var element = new Glyphs ( iterator.GetText ( PageIteratorLevel.Word ) )
            {
                Levels = levels,
                Font   = new Font { Name   = font.FontInfo.Name,
                                    Size   = font.PointSize,
                                    Weight = font.FontInfo.IsBold ? 700 : 400 }
            };

            if ( iterator.TryGetBoundingBox ( PageIteratorLevel.Word, out var bounds ) )
            {
                element.Position = new Point ( bounds.X1, bounds.Y1 );
                element.Size     = new Size  ( bounds.X2 - bounds.X1, bounds.Y2 - bounds.Y1 );
            }

            return element;
        }

        protected override void Dispose ( bool disposing )
        {
            if ( disposing )
                Page.Dispose ( );

            base.Dispose ( disposing );
        }
    }
}