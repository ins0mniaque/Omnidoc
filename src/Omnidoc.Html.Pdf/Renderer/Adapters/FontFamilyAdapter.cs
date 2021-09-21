using Omnidoc.Html.Renderer.Adapters;
using PdfSharpCore.Drawing;

namespace Omnidoc.Html.Pdf.Renderer.Adapters
{
    /// <summary>
    /// Adapter for ImageSharp Font object for core.
    /// </summary>
    internal sealed class FontFamilyAdapter : RFontFamily
    {
        /// <summary>
        /// Init.
        /// </summary>
        public FontFamilyAdapter(XFontFamily fontFamily)
        {
            FontFamily = fontFamily;
        }

        /// <summary>
        /// the underline win-forms font family.
        /// </summary>
        public XFontFamily FontFamily { get; }

        public override string Name
        {
            get { return FontFamily.Name; }
        }
    }
}