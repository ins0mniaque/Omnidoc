using System;
using System.Collections.Generic;
using System.Globalization;
using Omnidoc.Html.Renderer.Core.Utils;

namespace Omnidoc.Html.Renderer.Core.Entities
{
    /// <summary>
    /// Represents a block of CSS property values.<br/>
    /// Contains collection of key-value pairs that are CSS properties for specific css class.<br/>
    /// Css class can be either custom or html tag name.
    /// </summary>
    /// <remarks>
    /// To learn more about CSS blocks visit CSS spec: http://www.w3.org/TR/CSS21/syndata.html#block
    /// </remarks>
    public sealed class CssBlock
    {
        /// <summary>
        /// the name of the css class of the block
        /// </summary>
        /// <summary>
        /// the CSS block properties and values
        /// </summary>
        private readonly Dictionary<string, string> _properties;

        /// <summary>
        /// Creates a new block from the block's source
        /// </summary>
        /// <param name="class">the name of the css class of the block</param>
        /// <param name="properties">the CSS block properties and values</param>
        /// <param name="selectors">optional: additional selectors to used in hierarchy</param>
        /// <param name="hover">optional: is the css block has :hover pseudo-class</param>
        public CssBlock(string @class, Dictionary<string, string> properties, IList<CssBlockSelectorItem>? selectors = null, bool hover = false)
        {
            ArgChecker.AssertArgNotNullOrEmpty(@class, "@class");
            ArgChecker.AssertArgNotNull(properties, "properties");

            Class = @class;
            Selectors = selectors;
            _properties = properties;
            Hover = hover;
        }

        /// <summary>
        /// the name of the css class of the block
        /// </summary>
        public string Class { get; }

        /// <summary>
        /// additional selectors to used in hierarchy (p className1 > className2)
        /// </summary>
        public IList<CssBlockSelectorItem>? Selectors { get; }

        /// <summary>
        /// Gets the CSS block properties and its values
        /// </summary>
        public IDictionary<string, string> Properties => _properties;

        /// <summary>
        /// is the css block has :hover pseudo-class
        /// </summary>
        public bool Hover { get; }

        /// <summary>
        /// Merge the other block properties into this css block.<br/>
        /// Other block properties can overwrite this block properties.
        /// </summary>
        /// <param name="other">the css block to merge with</param>
        public void Merge(CssBlock other)
        {
            ArgChecker.AssertArgNotNull(other, "other");

            foreach (var prop in other._properties.Keys)
            {
                _properties[prop] = other._properties[prop];
            }
        }

        /// <summary>
        /// Create deep copy of the CssBlock.
        /// </summary>
        /// <returns>new CssBlock with same data</returns>
        public CssBlock Clone() => new(Class, new Dictionary<string, string>(_properties), Selectors != null ? new List<CssBlockSelectorItem>(Selectors) : null);

        /// <summary>
        /// Check if the two css blocks are the same (same class, selectors and properties).
        /// </summary>
        /// <param name="other">the other block to compare to</param>
        /// <returns>true - the two blocks are the same, false - otherwise</returns>
        public bool Equals(CssBlock other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            if (!Equals(other.Class, Class))
                return false;

            if (!Equals(other._properties.Count, _properties.Count))
                return false;

            foreach (var property in _properties)
            {
                if (!other._properties.ContainsKey(property.Key))
                    return false;
                if (!Equals(other._properties[property.Key], property.Value))
                    return false;
            }

            return EqualsSelector ( other );
        }

        /// <summary>
        /// Check if the selectors of the css blocks is the same.
        /// </summary>
        /// <param name="other">the other block to compare to</param>
        /// <returns>true - the selectors on blocks are the same, false - otherwise</returns>
        public bool EqualsSelector(CssBlock other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;

            if (other.Hover != Hover)
                return false;
            if (other.Selectors == null && Selectors != null)
                return false;
            if (other.Selectors != null && Selectors == null)
                return false;

            if (other.Selectors != null && Selectors != null)
            {
                if (!Equals(other.Selectors.Count, Selectors.Count))
                    return false;

                for (var i = 0; i < Selectors.Count; i++)
                {
                    if (!Equals(other.Selectors[i].Class, Selectors[i].Class))
                        return false;
                    if (!Equals(other.Selectors[i].DirectParent, Selectors[i].DirectParent))
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Check if the two css blocks are the same (same class, selectors and properties).
        /// </summary>
        /// <param name="obj">the other block to compare to</param>
        /// <returns>true - the two blocks are the same, false - otherwise</returns>
        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != typeof(CssBlock))
                return false;
            return Equals((CssBlock)obj);
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current <see cref="object"/>.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (( ( Class?.GetHashCode(StringComparison.Ordinal) ) ?? 0 ) * 397) ^ ( ( _properties?.GetHashCode() ) ?? 0 );
            }
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents the current <see cref="object"/>.
        /// </summary>
        public override string ToString()
        {
            var str = Class + " { ";
            foreach (var property in _properties)
            {
                str += string.Format(CultureInfo.InvariantCulture, "{0}={1}; ", property.Key, property.Value);
            }
            return str + " }";
        }
    }
}