using PdfSharpCore.Drawing;

namespace Omnidoc.Html.Pdf.Renderer.Adapters
{
    /// <summary>
    /// Because PdfSharp doesn't support texture brush we need to implement it ourselves.
    /// </summary>
    internal sealed class XTextureBrush
    {
        /// <summary>
        /// The image to draw in the brush
        /// </summary>
        private readonly XImage _image;

        /// <summary>
        /// the
        /// </summary>
        private readonly XRect _dstRect;

        /// <summary>
        /// the transform the location of the image to handle center alignment
        /// </summary>
        private readonly XPoint _translateTransformLocation;

        /// <summary>
        /// Init.
        /// </summary>
        public XTextureBrush(XImage image, XRect dstRect, XPoint translateTransformLocation)
        {
            _image = image;
            _dstRect = dstRect;
            _translateTransformLocation = translateTransformLocation;
        }

        /// <summary>
        /// Draw the texture image in the given graphics at the given location.
        /// </summary>
        public void DrawRectangle(XGraphics g, double x, double y, double width, double height)
        {
            var prevState = g.Save();
            g.IntersectClip(new XRect(x, y, width, height));

            var rx = _translateTransformLocation.X;
            double w = _image.PixelWidth, h = _image.PixelHeight;
            while (rx < x + width)
            {
                var ry = _translateTransformLocation.Y;
                while (ry < y + height)
                {
                    g.DrawImage(_image, rx, ry, w, h);
                    ry += h;
                }
                rx += w;
            }

            g.Restore(prevState);
        }
    }
}