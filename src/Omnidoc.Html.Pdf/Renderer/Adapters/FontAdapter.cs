using Omnidoc.Html.Renderer.Adapters;
using PdfSharpCore.Drawing;

namespace Omnidoc.Html.Pdf.Renderer.Adapters
{
    /// <summary>
    /// Adapter for ImageSharp Font object for core.
    /// </summary>
    internal sealed class FontAdapter : RFont
    {
        /// <summary>
        /// the underline win-forms font.
        /// </summary>
        /// <summary>
        /// the vertical offset of the font underline location from the top of the font.
        /// </summary>
        private double _underlineOffset = -1;

        /// <summary>
        /// Cached font height.
        /// </summary>
        private double _height = -1;

        /// <summary>
        /// Cached font whitespace width.
        /// </summary>
        private double _whitespaceWidth = -1;

        /// <summary>
        /// Init.
        /// </summary>
        public FontAdapter(XFont font)
        {
            Font = font;
        }

        /// <summary>
        /// the underline win-forms font.
        /// </summary>
        public XFont Font { get; }

        public override double Size
        {
            get { return Font.Size; }
        }

        public override double UnderlineOffset
        {
            get { return _underlineOffset; }
        }

        public override double Height
        {
            get { return _height; }
        }

        public override double LeftPadding
        {
            get { return _height / 6f; }
        }

        public override double GetWhitespaceWidth(RGraphics graphics)
        {
            if (_whitespaceWidth < 0)
            {
                _whitespaceWidth = graphics.MeasureString(" ", this).Width;
            }
            return _whitespaceWidth;
        }

        /// <summary>
        /// Set font metrics to be cached for the font for future use.
        /// </summary>
        /// <param name="height">the full height of the font</param>
        /// <param name="underlineOffset">the vertical offset of the font underline location from the top of the font.</param>
        internal void SetMetrics(int height, int underlineOffset)
        {
            _height = height;
            _underlineOffset = underlineOffset;
        }
    }
}