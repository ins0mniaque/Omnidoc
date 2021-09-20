using PdfSharpCore.Drawing;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Omnidoc.Html.Renderer.Adapters.Entities;

namespace Omnidoc.Html.Pdf.Renderer.Utilities
{
    /// <summary>
    /// Utilities for converting ImageSharp entities to HtmlRenderer core entities.
    /// </summary>
    internal static class Utils
    {
        /// <summary>
        /// Convert from ImageSharp point to core point.
        /// </summary>
        public static RPoint Convert(XPoint p)
        {
            return new RPoint(p.X, p.Y);
        }

        /// <summary>
        /// Convert from ImageSharp point to core point.
        /// </summary>
        public static XPoint[] Convert(RPoint[] points)
        {
            var myPoints = new XPoint[points.Length];
            for (var i = 0; i < points.Length; i++)
                myPoints[i] = Convert(points[i]);
            return myPoints;
        }

        /// <summary>
        /// Convert from core point to ImageSharp point.
        /// </summary>
        public static XPoint Convert(RPoint p)
        {
            return new XPoint(p.X, p.Y);
        }

        /// <summary>
        /// Convert from ImageSharp size to core size.
        /// </summary>
        public static RSize Convert(XSize s)
        {
            return new RSize(s.Width, s.Height);
        }

        /// <summary>
        /// Convert from core size to ImageSharp size.
        /// </summary>
        public static XSize Convert(RSize s)
        {
            return new XSize(s.Width, s.Height);
        }

        /// <summary>
        /// Convert from ImageSharp rectangle to core rectangle.
        /// </summary>
        public static RRect Convert(XRect r)
        {
            return new RRect(r.X, r.Y, r.Width, r.Height);
        }

        /// <summary>
        /// Convert from core rectangle to ImageSharp rectangle.
        /// </summary>
        public static XRect Convert(RRect r)
        {
            return new XRect(r.X, r.Y, r.Width, r.Height);
        }

        /// <summary>
        /// Convert from core color to ImageSharp color.
        /// </summary>
        public static XColor Convert(RColor c)
        {
            return XColor.FromArgb(c.A, c.R, c.G, c.B);
        }

        /// <summary>
        /// Convert from color to ImageSharp color.
        /// </summary>
        public static RColor Convert(Color c)
        {
            var pixel = c.ToPixel<Rgba32>();
            return RColor.FromArgb(pixel.A, pixel.R, pixel.G, pixel.B);
        }
    }
}