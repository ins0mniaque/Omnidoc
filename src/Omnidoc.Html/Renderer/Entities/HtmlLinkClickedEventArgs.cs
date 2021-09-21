using System;
using System.Collections.Generic;
using System.Globalization;

namespace Omnidoc.Html.Renderer.Core.Entities
{
    /// <summary>
    /// Raised when the user clicks on a link in the html.
    /// </summary>
    public sealed class HtmlLinkClickedEventArgs : EventArgs
    {
        /// <summary>
        /// Init.
        /// </summary>
        /// <param name="link">the link href that was clicked</param>
        public HtmlLinkClickedEventArgs(string link, IDictionary<string, string> attributes)
        {
            Link = link;
            Attributes = attributes;
        }

        /// <summary>
        /// the link href that was clicked
        /// </summary>
        public string Link { get; }

        /// <summary>
        /// collection of all the attributes that are defined on the link element
        /// </summary>
        public IDictionary<string, string> Attributes { get; }

        /// <summary>
        /// use to cancel the execution of the link
        /// </summary>
        public bool Handled { get; set; }

        public override string ToString() => string.Format(CultureInfo.InvariantCulture, "Link: {0}, Handled: {1}", Link, Handled);
    }
}