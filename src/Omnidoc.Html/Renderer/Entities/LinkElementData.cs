using System;
using System.Globalization;

namespace Omnidoc.Html.Renderer.Core.Entities
{
    /// <summary>
    /// Holds data on link element in HTML.<br/>
    /// Used to expose data outside of HTML Renderer internal structure.
    /// </summary>
    public sealed class LinkElementData<T>
    {

        /// <summary>
        /// the rectangle of element as calculated by html layout
        /// </summary>
        private readonly T _rectangle;

        /// <summary>
        /// Init.
        /// </summary>
        public LinkElementData(string id, string href, T rectangle)
        {
            Id = id;
            Href = href;
            _rectangle = rectangle;
        }

        /// <summary>
        /// the id of the link element if present
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// the href data of the link
        /// </summary>
        public string Href { get; }

        /// <summary>
        /// the rectangle of element as calculated by html layout
        /// </summary>
        public T Rectangle => _rectangle;

        /// <summary>
        /// Is the link is directed to another element in the html
        /// </summary>
        public bool IsAnchor => Href.Length > 0 && Href[0] == '#';

        /// <summary>
        /// Return the id of the element this anchor link is referencing.
        /// </summary>
        public string AnchorId => IsAnchor && Href.Length > 1 ? Href[1..] : string.Empty;

        public LinkElementData<T2> Convert<T2>(Func<T, T2> convert)
        {
            if(convert == null)
                throw new ArgumentNullException(nameof(convert));

            return new LinkElementData<T2>(Id, Href, convert(_rectangle));
        }

        public override string ToString() => string.Format(CultureInfo.InvariantCulture, "Id: {0}, Href: {1}, Rectangle: {2}", Id, Href, _rectangle);
    }
}