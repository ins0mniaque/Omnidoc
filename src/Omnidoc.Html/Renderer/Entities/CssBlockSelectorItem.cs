using Omnidoc.Html.Renderer.Core.Utils;

namespace Omnidoc.Html.Renderer.Core.Entities
{
    /// <summary>
    /// Holds single class selector in css block hierarchical selection (p class1 > div.class2)
    /// </summary>
    public class CssBlockSelectorItem
    {
        /// <summary>
        /// the name of the css class of the block
        /// </summary>
        /// <summary>
        /// Creates a new block from the block's source
        /// </summary>
        /// <param name="class">the name of the css class of the block</param>
        /// <param name="directParent"> </param>
        public CssBlockSelectorItem(string @class, bool directParent)
        {
            ArgChecker.AssertArgNotNullOrEmpty(@class, "@class");

            Class = @class;
            DirectParent = directParent;
        }

        /// <summary>
        /// the name of the css class of the block
        /// </summary>
        public string Class { get; }

        /// <summary>
        /// is the selector item has to be direct parent
        /// </summary>
        public bool DirectParent { get; }

        /// <summary>
        /// Returns a <see cref="string"/> that represents the current <see cref="object"/>.
        /// </summary>
        public override string ToString() => Class + (DirectParent ? " > " : string.Empty);
    }
}