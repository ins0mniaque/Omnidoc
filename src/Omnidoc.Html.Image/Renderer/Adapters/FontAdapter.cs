using SixLabors.Fonts;
using Omnidoc.Html.Renderer.Adapters;

namespace Omnidoc.Html.Image.Renderer.Adapters
{
    /// <summary>
    /// Adapter for ImageSharp Font object for core.
    /// </summary>
    internal sealed class FontAdapter : RFont
    {
        public FontAdapter(Font font)
        {
            Font = font;
        }

        /// <summary>
        /// The underlying ImageSharp font.
        /// </summary>
        public Font Font { get; }

        // TODO: Add UnderlineOffset support when SixLabors.Fonts supports it
        public override double Size            => Font.Size;
        public override double UnderlineOffset => 0;
        public override double Height          => Font.LineHeight;
        public override double LeftPadding     => Font.LineHeight / 6f;

        private double WhitespaceWidth { get; set; } = -1;

        public override double GetWhitespaceWidth(RGraphics graphics)
        {
            if (WhitespaceWidth < 0)
                WhitespaceWidth = graphics.MeasureString(" ", this).Width;

            return WhitespaceWidth;
        }
    }
}