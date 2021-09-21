using System.Collections.Generic;
using System.Globalization;
using Omnidoc.Html.Renderer.Core.Utils;

namespace Omnidoc.Html.Renderer.Core.Dom
{
    internal sealed class HtmlTag
    {
        /// <summary>
        /// the name of the html tag
        /// </summary>
        /// <summary>
        /// collection of attributes and their value the html tag has
        /// </summary>
        private readonly Dictionary<string, string>? _attributes;

        /// <summary>
        /// Init.
        /// </summary>
        /// <param name="name">the name of the html tag</param>
        /// <param name="isSingle">if the tag is single placed; in other words it doesn't have a separate closing tag;</param>
        /// <param name="attributes">collection of attributes and their value the html tag has</param>
        public HtmlTag(string name, bool isSingle, Dictionary<string, string>? attributes = null)
        {
            ArgChecker.AssertArgNotNullOrEmpty(name, "name");

            Name = name;
            IsSingle = isSingle;
            _attributes = attributes;
        }

        /// <summary>
        /// Gets the name of this tag
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets collection of attributes and their value the html tag has
        /// </summary>
        public IDictionary<string, string>? Attributes => _attributes;

        /// <summary>
        /// Gets if the tag is single placed; in other words it doesn't have a separate closing tag; <br/>
        /// e.g. &lt;br&gt;
        /// </summary>
        public bool IsSingle { get; }

        /// <summary>
        /// is the html tag has attributes.
        /// </summary>
        /// <returns>true - has attributes, false - otherwise</returns>
        public bool HasAttributes() => _attributes?.Count > 0;

        /// <summary>
        /// Gets a boolean indicating if the attribute list has the specified attribute
        /// </summary>
        /// <param name="attribute">attribute name to check if exists</param>
        /// <returns>true - attribute exists, false - otherwise</returns>
        public bool HasAttribute(string attribute) => _attributes?.ContainsKey ( attribute ) == true;

        /// <summary>
        /// Get attribute value for given attribute name or null if not exists.
        /// </summary>
        /// <param name="attribute">attribute name to get by</param>
        /// <param name="defaultValue">optional: value to return if attribute is not specified</param>
        /// <returns>attribute value or null if not found</returns>
        public string? TryGetAttribute(string attribute, string? defaultValue = null) => _attributes?.ContainsKey ( attribute ) == true ? _attributes[attribute] : defaultValue;

        public override string ToString() => string.Format(CultureInfo.InvariantCulture, "<{0}>", Name);
    }
}