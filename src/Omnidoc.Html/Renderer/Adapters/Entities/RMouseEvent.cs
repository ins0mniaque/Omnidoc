using Omnidoc.Html.Renderer.Core;

namespace Omnidoc.Html.Renderer.Adapters.Entities
{
    /// <summary>
    /// Even class for handling keyboard events in <see cref="HtmlContainerInt"/>.
    /// </summary>
    public sealed class RMouseEvent
    {

        /// <summary>
        /// Init.
        /// </summary>
        public RMouseEvent(bool leftButton)
        {
            LeftButton = leftButton;
        }

        /// <summary>
        /// Is the left mouse button participated in the event
        /// </summary>
        public bool LeftButton { get; }
    }
}