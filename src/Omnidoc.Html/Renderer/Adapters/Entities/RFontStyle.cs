using System;
using System.Diagnostics.CodeAnalysis;

namespace Omnidoc.Html.Renderer.Adapters.Entities
{
    /// <summary>
    /// Specifies style information applied to text.
    /// </summary>
    [Flags]
    [SuppressMessage("Design", "CA1008:Enums should have zero value", Justification = "FontStyle")]
    public enum RFontStyle
    {
        Regular = 0,
        Bold = 1,
        Italic = 2,
        Underline = 4,
        Strikeout = 8,
    }
}