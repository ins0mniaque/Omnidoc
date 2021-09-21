using System.Collections.Generic;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;
using Omnidoc.Html.Renderer.Adapters.Entities;
using Omnidoc.Html.Renderer.Core.Utils;
using Omnidoc.Html.Renderer.Adapters;
using Omnidoc.Html.Image.Renderer.Utilities;

namespace Omnidoc.Html.Image.Renderer.Adapters
{
    /// <summary>
    /// Adapter for ImageSharp Graphics for core.
    /// </summary>
    internal sealed class GraphicsAdapter : RGraphics
    {
        /// <summary>
        /// The wrapped ImageSharp graphics object
        /// </summary>
        private readonly IImageProcessingContext _g;

        // TODO: Implement clipping
        private enum ClipMode { Replace, Exclude }

        private readonly Stack<ClipMode> _clipModeStack = new();

        /// <summary>
        /// Init.
        /// </summary>
        /// <param name="g">the ImageSharp graphics object to use</param>
        public GraphicsAdapter(IImageProcessingContext g)
            : base(ImageSharpAdapter.Instance, Utils.Convert(new RectangleF(PointF.Empty, g.GetCurrentSize())))
        {
            ArgChecker.AssertArgNotNull(g, "g");

            _g = g;
        }

        public override void PopClip()
        {
            ClipStack.Pop();
            _clipModeStack.Pop();
        }

        public override void PushClip(RRect rect)
        {
            ClipStack.Push(rect);
            _clipModeStack.Push(ClipMode.Replace);
        }

        public override void PushClipExclude(RRect rect)
        {
            ClipStack.Push(rect);
            _clipModeStack.Push(ClipMode.Exclude);
        }

        public override object SetAntiAliasSmoothingMode()
        {
            var options = _g.GetGraphicsOptions();
            var prevMode = options.Antialias;
            options.Antialias = true;
            _g.SetGraphicsOptions(options);
            return prevMode;
        }

        public override void ReturnPreviousSmoothingMode(object prevMode)
        {
            if (prevMode != null)
            {
                var options = _g.GetGraphicsOptions();
                options.Antialias = (bool)prevMode;
                _g.SetGraphicsOptions(options);
            }
        }

        public override RSize MeasureString(string str, RFont font)
        {
            var fontAdapter = (FontAdapter)font;
            var realFont = fontAdapter.Font;

            var options   = new RendererOptions(((FontAdapter)font).Font);
            var rectangle = TextMeasurer.Measure(str, options);
            var size      = new SizeF(rectangle.Width, rectangle.Height);

            return Utils.Convert(size);
        }

        public override void MeasureString(string str, RFont font, double maxWidth, out int charFit, out double charFitWidth)
        {
            charFit = 0;
            charFitWidth = 0;

            var size = MeasureString(str, font);

            for (var i = 1; i <= str.Length; i++)
            {
                charFit = i - 1;
                var pSize = MeasureString(str.Substring(0, i), font);
                if (pSize.Height <= size.Height && pSize.Width < maxWidth)
                    charFitWidth = pSize.Width;
                else
                    break;
            }
        }

        public override void DrawString(string str, RFont font, RColor color, RPoint point, RSize size, bool rtl)
        {
            // TODO: Add RTL font support support when SixLabors.Fonts supports it
            point = rtl ? point : new RPoint(point.X + size.Width, point.Y);
            _g.DrawText(str, ((FontAdapter)font).Font, Utils.Convert(color), Utils.Convert(point));
        }

        public override RBrush GetTextureBrush(RImage image, RRect dstRect, RPoint translateTransformLocation)
        {
            // TODO: Add dstRect and translateTransformLocation support
            var brush = new ImageBrush(((ImageAdapter)image).Image);
            return new BrushAdapter(brush);
        }

        public override RGraphicsPath GetGraphicsPath() => new GraphicsPathAdapter();

        public override void DrawLine(RPen pen, double x1, double y1, double x2, double y2) => _g.DrawLines(((PenAdapter)pen).Pen, new PointF((float)x1, (float)y1), new PointF((float)x2, (float)y2));

        public override void DrawRectangle(RPen pen, double x, double y, double width, double height) => _g.Draw(((PenAdapter)pen).Pen, new RectangleF((float)x, (float)y, (float)width, (float)height));

        public override void DrawRectangle(RBrush brush, double x, double y, double width, double height) => _g.Fill(((BrushAdapter)brush).Brush, new RectangleF((float)x, (float)y, (float)width, (float)height));

        public override void DrawImage(RImage image, RRect destRect, RRect srcRect)
        {
            var source = ((ImageAdapter)image).Image;
            var crop   = (int)srcRect.X != 0 || (int)srcRect.Y != 0 || (int)srcRect.Width != source.Width || (int)srcRect.Height != source.Height;
            var scale  = destRect.Width != srcRect.Width || destRect.Height != srcRect.Height;
            if ( crop || scale )
            {
                source = source.Clone ( x =>
                {
                    if ( crop  ) x.Crop   ( new Rectangle((int)srcRect.X, (int)srcRect.Y, (int)srcRect.Width, (int)srcRect.Height) );
                    if ( scale ) x.Resize ( new SixLabors.ImageSharp.Size((int)destRect.Width, (int)destRect.Height) );
                } );
            }

            _g.DrawImage(source, new SixLabors.ImageSharp.Point((int) destRect.X, (int) destRect.Y), 1f);
        }

        public override void DrawImage(RImage image, RRect destRect)
        {
            var source = ((ImageAdapter)image).Image;
            var scale  = (int)destRect.Width != source.Width || (int)destRect.Height != source.Height;
            if ( scale )
                source = source.Clone ( x => x.Resize ( new SixLabors.ImageSharp.Size((int)destRect.Width, (int)destRect.Height) ) );

            _g.DrawImage(source, new SixLabors.ImageSharp.Point((int) destRect.X, (int) destRect.Y), 1f);
        }

        public override void DrawPath(RPen pen, RGraphicsPath path) => _g.Draw(((PenAdapter)pen).Pen, ((GraphicsPathAdapter)path).PathBuilder.Build());

        public override void DrawPath(RBrush brush, RGraphicsPath path) => _g.Fill(((BrushAdapter)brush).Brush, ((GraphicsPathAdapter)path).PathBuilder.Build());

        public override void DrawPolygon(RBrush brush, RPoint[] points)
        {
            if (points != null && points.Length > 0)
                _g.FillPolygon(((BrushAdapter)brush).Brush, Utils.Convert(points));
        }
    }
}