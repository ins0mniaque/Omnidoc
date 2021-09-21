using Omnidoc.Html.Renderer.Adapters;
using Omnidoc.Html.Renderer.Adapters.Entities;

namespace Omnidoc.Html.Renderer.Core.Dom
{
    /// <summary>
    /// Represents a word inside an inline box
    /// </summary>
    internal sealed class CssRectImage : CssRect
    {
        /// <summary>
        /// Creates a new BoxWord which represents an image
        /// </summary>
        /// <param name="owner">the CSS box owner of the word</param>
        public CssRectImage(CssBox owner)
            : base(owner)
        { }

        /// <summary>
        /// Gets the image this words represents (if one exists)
        /// </summary>
        public override RImage? Image { get; set; }

        /// <summary>
        /// Gets if the word represents an image.
        /// </summary>
        public override bool IsImage => true;

        /// <summary>
        /// the image rectange restriction as returned from image load event
        /// </summary>
        public RRect ImageRectangle { get; set; }

        /// <summary>
        /// Represents this word for debugging purposes
        /// </summary>
        /// <returns></returns>
        public override string ToString() => "Image";
    }
}