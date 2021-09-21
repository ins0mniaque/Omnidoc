using System;
using System.Globalization;
using System.IO;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using Omnidoc.Html.Renderer.Adapters.Entities;
using Omnidoc.Html.Renderer.Adapters;
using Omnidoc.Html.Image.Renderer.Utilities;

namespace Omnidoc.Html.Image.Renderer.Adapters
{
    using Image = SixLabors.ImageSharp.Image;

    /// <summary>
    /// Adapter for ImageSharp platform.
    /// </summary>
    internal sealed class ImageSharpAdapter : RAdapter
    {
        #region Fields and Consts

        /// <summary>
        /// Singleton instance of global adapter.
        /// </summary>
        private static readonly ImageSharpAdapter _instance = new ImageSharpAdapter();

        #endregion

        /// <summary>
        /// Init installed font families and set default font families mapping.
        /// </summary>
        private ImageSharpAdapter()
        {
            AddFontFamilyMapping("monospace", "Courier New");
            AddFontFamilyMapping("Helvetica", "Arial");

            foreach (var family in SystemFonts.Collection.Families)
            {
                AddFontFamily(new FontFamilyAdapter(family));
            }
        }

        /// <summary>
        /// Singleton instance of global adapter.
        /// </summary>
        public static ImageSharpAdapter Instance
        {
            get { return _instance; }
        }

        protected override RColor GetColorInt(string colorName)
        {
            return Utils.Convert(Color.Parse(colorName));
        }

        protected override RPen CreatePen(RColor color)
        {
            return new PenAdapter(new Pen(Utils.Convert(color), 1f));
        }

        protected override RBrush CreateSolidBrush(RColor color)
        {
            return new BrushAdapter(Brushes.Solid(Utils.Convert(color)));
        }

        protected override RBrush CreateLinearGradientBrush(RRect rect, RColor color1, RColor color2, double angle)
        {
            // TODO: Find line with angle intersection points on rect
            var start = new PointF(0, 0);
            var end   = new PointF(1, 0);
            var brush = new LinearGradientBrush(start, end,
                                                GradientRepetitionMode.None,
                                                new ColorStop(0, Utils.Convert(color1)),
                                                new ColorStop(1, Utils.Convert(color2)));

            return new BrushAdapter(brush);
        }

        protected override RImage ConvertImageInt(object image)
        {
            if(image is null)
                throw new ArgumentNullException(nameof(image));

            return new ImageAdapter((Image)image);
        }

        protected override RImage ImageFromStreamInt(Stream memoryStream)
        {
            return new ImageAdapter(Image.Load(memoryStream));
        }

        protected override RFont CreateFontInt(string family, double size, RFontStyle style)
        {
            return new FontAdapter(new Font(SystemFonts.Find(family, CultureInfo.InvariantCulture), (float)size, (FontStyle) (int) style));
        }

        protected override RFont CreateFontInt(RFontFamily family, double size, RFontStyle style)
        {
            return new FontAdapter(new Font(((FontFamilyAdapter)family).FontFamily, (float)size, (FontStyle) (int) style));
        }
    }
}