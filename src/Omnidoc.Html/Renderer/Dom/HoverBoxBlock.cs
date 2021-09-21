using Omnidoc.Html.Renderer.Core.Entities;

namespace Omnidoc.Html.Renderer.Core.Dom
{
    /// <summary>
    /// CSS boxes that have ":hover" selector on them.
    /// </summary>
    internal sealed class HoverBoxBlock
    {

        /// <summary>
        /// Init.
        /// </summary>
        public HoverBoxBlock(CssBox cssBox, CssBlock cssBlock)
        {
            CssBox = cssBox;
            CssBlock = cssBlock;
        }

        /// <summary>
        /// the box that has :hover css on
        /// </summary>
        public CssBox CssBox { get; }

        /// <summary>
        /// the :hover style block data
        /// </summary>
        public CssBlock CssBlock { get; }
    }
}