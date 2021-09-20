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
        public static RPoint Convert(PointF p)
        {
            return new RPoint(p.X, p.Y);
        }

        /// <summary>
        /// Convert from ImageSharp point to core point.
        /// </summary>
        public static PointF[] Convert(RPoint[] points)
        {
            PointF[] myPoints = new PointF[points.Length];
            for (int i = 0; i < points.Length; i++)
                myPoints[i] = Convert(points[i]);
            return myPoints;
        }

        /// <summary>
        /// Convert from core point to ImageSharp point.
        /// </summary>
        public static PointF Convert(RPoint p)
        {
            return new PointF((float)p.X, (float)p.Y);
        }

        /// <summary>
        /// Convert from core point to ImageSharp point.
        /// </summary>
        public static PointF ConvertRound(RPoint p)
        {
            return new PointF((int)Math.Round(p.X), (int)Math.Round(p.Y));
        }

        /// <summary>
        /// Convert from ImageSharp size to core size.
        /// </summary>
        public static RSize Convert(SizeF s)
        {
            return new RSize(s.Width, s.Height);
        }

        /// <summary>
        /// Convert from core size to ImageSharp size.
        /// </summary>
        public static SizeF Convert(RSize s)
        {
            return new SizeF((float)s.Width, (float)s.Height);
        }

        /// <summary>
        /// Convert from core size to ImageSharp size.
        /// </summary>
        public static SizeF ConvertRound(RSize s)
        {
            return new SizeF((int)Math.Round(s.Width), (int)Math.Round(s.Height));
        }

        /// <summary>
        /// Convert from ImageSharp rectangle to core rectangle.
        /// </summary>
        public static RRect Convert(RectangleF r)
        {
            return new RRect(r.X, r.Y, r.Width, r.Height);
        }

        /// <summary>
        /// Convert from core rectangle to ImageSharp rectangle.
        /// </summary>
        public static RectangleF Convert(RRect r)
        {
            return new RectangleF((float)r.X, (float)r.Y, (float)r.Width, (float)r.Height);
        }

        /// <summary>
        /// Convert from core rectangle to ImageSharp rectangle.
        /// </summary>
        public static RectangleF ConvertRound(RRect r)
        {
            return new RectangleF((int)Math.Round(r.X), (int)Math.Round(r.Y), (int)Math.Round(r.Width), (int)Math.Round(r.Height));
        }

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
        public static Color Convert(RColor c)
        {
            return Color.FromRgba(c.R, c.G, c.B, c.A);
        }
    }
}