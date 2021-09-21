using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Omnidoc.Html.Renderer.Adapters.Entities;

namespace Omnidoc.Html.Image.Renderer.Utilities
{
    /// <summary>
    /// Utilities for converting ImageSharp entities to HtmlRenderer core entities.
    /// </summary>
    internal static class Utils
    {
        /// <summary>
        /// Convert from ImageSharp point to core point.
        /// </summary>
        public static RPoint Convert(PointF p) => new(p.X, p.Y);

        /// <summary>
        /// Convert from ImageSharp point to core point.
        /// </summary>
        public static PointF[] Convert(RPoint[] points)
        {
            var myPoints = new PointF[points.Length];
            for (var i = 0; i < points.Length; i++)
                myPoints[i] = Convert(points[i]);
            return myPoints;
        }

        /// <summary>
        /// Convert from core point to ImageSharp point.
        /// </summary>
        public static PointF Convert(RPoint p) => new((float)p.X, (float)p.Y);

        /// <summary>
        /// Convert from core point to ImageSharp point.
        /// </summary>
        public static PointF ConvertRound(RPoint p) => new((int)Math.Round(p.X), (int)Math.Round(p.Y));

        /// <summary>
        /// Convert from ImageSharp size to core size.
        /// </summary>
        public static RSize Convert(SizeF s) => new(s.Width, s.Height);

        /// <summary>
        /// Convert from core size to ImageSharp size.
        /// </summary>
        public static SizeF Convert(RSize s) => new((float)s.Width, (float)s.Height);

        /// <summary>
        /// Convert from core size to ImageSharp size.
        /// </summary>
        public static SizeF ConvertRound(RSize s) => new((int)Math.Round(s.Width), (int)Math.Round(s.Height));

        /// <summary>
        /// Convert from ImageSharp rectangle to core rectangle.
        /// </summary>
        public static RRect Convert(RectangleF r) => new(r.X, r.Y, r.Width, r.Height);

        /// <summary>
        /// Convert from core rectangle to ImageSharp rectangle.
        /// </summary>
        public static RectangleF Convert(RRect r) => new((float)r.X, (float)r.Y, (float)r.Width, (float)r.Height);

        /// <summary>
        /// Convert from core rectangle to ImageSharp rectangle.
        /// </summary>
        public static RectangleF ConvertRound(RRect r) => new((int)Math.Round(r.X), (int)Math.Round(r.Y), (int)Math.Round(r.Width), (int)Math.Round(r.Height));

        /// <summary>
        /// Convert from ImageSharp color to core color.
        /// </summary>
        public static RColor Convert(Color c)
        {
            var pixel = c.ToPixel<Rgba32>();
            return RColor.FromArgb(pixel.A, pixel.R, pixel.G, pixel.B);
        }

        /// <summary>
        /// Convert from core color to ImageSharp color.
        /// </summary>
        public static Color Convert(RColor c) => Color.FromRgba(c.R, c.G, c.B, c.A);
    }
}