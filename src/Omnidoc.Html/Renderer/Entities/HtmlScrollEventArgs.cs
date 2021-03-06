using System;
using System.Globalization;
using Omnidoc.Html.Renderer.Adapters.Entities;

namespace Omnidoc.Html.Renderer.Core.Entities
{
    /// <summary>
    /// Raised when Html Renderer request scroll to specific location.<br/>
    /// This can occur on document anchor click.
    /// </summary>
    public sealed class HtmlScrollEventArgs : EventArgs
    {
        /// <summary>
        /// the location to scroll to
        /// </summary>
        private readonly RPoint _location;

        /// <summary>
        /// Init.
        /// </summary>
        /// <param name="location">the location to scroll to</param>
        public HtmlScrollEventArgs(RPoint location)
        {
            _location = location;
        }

        /// <summary>
        /// the x location to scroll to
        /// </summary>
        public double X => _location.X;

        /// <summary>
        /// the x location to scroll to
        /// </summary>
        public double Y => _location.Y;

        public override string ToString() => string.Format(CultureInfo.InvariantCulture, "Location: {0}", _location);
    }
}