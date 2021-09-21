using System;
using System.Globalization;

namespace Omnidoc.Html.Renderer.Core.Entities
{
    /// <summary>
    /// Raised when html renderer requires refresh of the control hosting (invalidation and re-layout).<br/>
    /// It can happen if some async event has occurred that requires re-paint and re-layout of the html.<br/>
    /// Example: async download of image is complete.
    /// </summary>
    public sealed class HtmlRefreshEventArgs : EventArgs
    {
        /// <summary>
        /// Init.
        /// </summary>
        /// <param name="layout">is re-layout is required for the refresh</param>
        public HtmlRefreshEventArgs(bool layout)
        {
            Layout = layout;
        }

        /// <summary>
        /// is re-layout is required for the refresh
        /// </summary>
        public bool Layout { get; }

        public override string ToString() => string.Format(CultureInfo.InvariantCulture, "Layout: {0}", Layout);
    }
}