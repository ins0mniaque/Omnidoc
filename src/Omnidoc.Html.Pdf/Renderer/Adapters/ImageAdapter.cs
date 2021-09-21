using Omnidoc.Html.Renderer.Adapters;
using PdfSharpCore.Drawing;

namespace Omnidoc.Html.Pdf.Renderer.Adapters
{
    /// <summary>
    /// Adapter for ImageSharp Image object for core.
    /// </summary>
    internal sealed class ImageAdapter : RImage
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="object"/> class.
        /// </summary>
        public ImageAdapter(XImage image)
        {
            Image = image;
        }

        /// <summary>
        /// the underline win-forms image.
        /// </summary>
        public XImage Image { get; }

        public override double Width
        {
            get { return Image.PixelWidth; }
        }

        public override double Height
        {
            get { return Image.PixelHeight; }
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
                Image.Dispose();

            base.Dispose(disposing);
        }
    }
}