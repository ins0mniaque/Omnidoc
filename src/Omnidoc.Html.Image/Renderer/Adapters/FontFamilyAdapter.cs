using SixLabors.Fonts;
using Omnidoc.Html.Renderer.Adapters;

namespace Omnidoc.Html.Image.Renderer.Adapters
{
    /// <summary>
    /// Adapter for ImageSharp Font family object for core.
    /// </summary>
    internal sealed class FontFamilyAdapter : RFontFamily
    {

        /// <summary>
        /// Init.
        /// </summary>
        public FontFamilyAdapter(FontFamily fontFamily)
        {
            FontFamily = fontFamily;
        }

        /// <summary>
        /// the underline win-forms font family.
        /// </summary>
        public FontFamily FontFamily { get; }

        public override string Name => FontFamily.Name;
    }
}