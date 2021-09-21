﻿using System;
using System.Collections.Generic;
using System.Globalization;

using Omnidoc.Html.Renderer.Core.Utils;

namespace Omnidoc.Html.Renderer.Core.Dom
{
    /// <summary>
    /// Used to make space on vertical cell combination
    /// </summary>
    internal sealed class CssSpacingBox : CssBox
    {
        public CssSpacingBox(CssBox tableBox, ref CssBox extendedBox, int startRow)
            : base(tableBox, new HtmlTag("none", false, new Dictionary<string, string> { { "colspan", "1" } }))
        {
            ExtendedBox = extendedBox;
            Display = CssConstants.None;

            StartRow = startRow;
            EndRow = startRow + int.Parse(extendedBox.GetAttribute("rowspan", "1"), CultureInfo.InvariantCulture) - 1;
        }

        public CssBox ExtendedBox { get; }

        /// <summary>
        /// Gets the index of the row where box starts
        /// </summary>
        public int StartRow { get; }

        /// <summary>
        /// Gets the index of the row where box ends
        /// </summary>
        public int EndRow { get; }
    }
}