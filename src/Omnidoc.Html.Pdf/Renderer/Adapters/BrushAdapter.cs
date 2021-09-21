using System;
using Omnidoc.Html.Renderer.Adapters;
using PdfSharpCore.Drawing;

namespace Omnidoc.Html.Pdf.Renderer.Adapters
{
    /// <summary>
    /// Adapter for ImageSharp brushes objects for core.
    /// </summary>
    internal sealed class BrushAdapter : RBrush
    {
        /// <summary>
        /// Init.
        /// </summary>
        public BrushAdapter(object brush)
        {
            Brush = brush;
        }

        /// <summary>
        /// The actual ImageSharp brush instance.
        /// </summary>
        public object Brush { get; }
    }
}