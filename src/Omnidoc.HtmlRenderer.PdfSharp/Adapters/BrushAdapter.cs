using System;
using Omnidoc.HtmlRenderer.Adapters;
using PdfSharpCore.Drawing;

namespace Omnidoc.HtmlRenderer.PdfSharp.Adapters
{
    /// <summary>
    /// Adapter for WinForms brushes objects for core.
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
        /// The actual WinForms brush instance.
        /// </summary>
        public object Brush
        {
            get { return _brush; }
        }

        public override void Dispose()
        { }
    }
}