using Omnidoc.Html.Renderer.Adapters;

namespace Omnidoc.Html.Image.Renderer.Adapters
{
    using Image = SixLabors.ImageSharp.Image;

    /// <summary>
    /// Adapter for ImageSharp Image object for core.
    /// </summary>
    internal sealed class ImageAdapter : RImage
    {
        public ImageAdapter(Image image)
        {
            Image = image;
        }

        /// <summary>
        /// The underlying ImageSharp image.
        /// </summary>
        public Image Image { get; }

        public override double Width  => Image.Width;
        public override double Height => Image.Height;

        public override void Dispose()
        {
            Image.Dispose();
        }
    }
}