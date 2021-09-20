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
        /// The actual PdfSharp brush instance.<br/>
        /// Should be <see cref="XBrush"/> but there is some fucking issue inheriting from it =/
        /// </summary>
        private readonly object _brush;

        /// <summary>
        /// Init.
        /// </summary>
        public BrushAdapter(object brush)
        {
            _brush = brush;
        }

        /// <summary>
        /// The actual ImageSharp brush instance.
        /// </summary>
        public object Brush
        {
            get { return _brush; }
        }

        public override void Dispose()
        { }
    }
}