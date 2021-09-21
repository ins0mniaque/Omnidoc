using SixLabors.ImageSharp.Drawing.Processing;
using Omnidoc.Html.Renderer.Adapters;

namespace Omnidoc.Html.Image.Renderer.Adapters
{
    /// <summary>
    /// Adapter for ImageSharp brushes objects for core.
    /// </summary>
    internal sealed class BrushAdapter : RBrush
    {
        public BrushAdapter(IBrush brush)
        {
            Brush = brush;
        }

        /// <summary>
        /// The actual ImageSharp brush instance.
        /// </summary>
        public IBrush Brush { get; }
    }
}