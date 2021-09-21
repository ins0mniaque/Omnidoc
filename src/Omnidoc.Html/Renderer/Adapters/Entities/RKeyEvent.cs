using Omnidoc.Html.Renderer.Core;

namespace Omnidoc.Html.Renderer.Adapters.Entities
{
    /// <summary>
    /// Even class for handling keyboard events in <see cref="HtmlContainerInt"/>.
    /// </summary>
    public sealed class RKeyEvent
    {
        /// <summary>
        /// Init.
        /// </summary>
        public RKeyEvent(bool control, bool aKeyCode, bool cKeyCode)
        {
            Control = control;
            AKeyCode = aKeyCode;
            CKeyCode = cKeyCode;
        }

        /// <summary>
        /// is control is pressed
        /// </summary>
        public bool Control { get; }

        /// <summary>
        /// is 'A' key is pressed
        /// </summary>
        public bool AKeyCode { get; }

        /// <summary>
        /// is 'C' key is pressed
        /// </summary>
        public bool CKeyCode { get; }
    }
}