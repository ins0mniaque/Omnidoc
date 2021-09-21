using System;
using System.Collections.Generic;

namespace Omnidoc.Html.Renderer.Core.Entities
{
    /// <summary>
    /// Invoked when a stylesheet is about to be loaded by file path or URL in 'link' element.<br/>
    /// Allows to overwrite the loaded stylesheet by providing the stylesheet data manually, or different source (file or URL) to load from.<br/>
    /// Example: The stylesheet 'href' can be non-valid URI string that is interpreted in the overwrite delegate by custom logic to pre-loaded stylesheet object<br/>
    /// If no alternative data is provided the original source will be used.<br/>
    /// </summary>
    public sealed class HtmlStylesheetLoadEventArgs : EventArgs
    {
        #region Fields and Consts

        /// <summary>
        /// the source of the stylesheet as found in the HTML (file path or URL)
        /// </summary>

        /// <summary>
        /// provide the stylesheet data to load
        /// </summary>
        private CssData? _setStyleSheetData;

        #endregion


        /// <summary>
        /// Init.
        /// </summary>
        /// <param name="src">the source of the image (file path or URL)</param>
        /// <param name="attributes">collection of all the attributes that are defined on the image element</param>
        internal HtmlStylesheetLoadEventArgs(string src, IDictionary<string, string>? attributes)
        {
            Src = src;
            Attributes = attributes;
        }

        /// <summary>
        /// the source of the stylesheet as found in the HTML (file path or URL)
        /// </summary>
        public string Src { get; }

        /// <summary>
        /// collection of all the attributes that are defined on the link element
        /// </summary>
        public IDictionary<string, string>? Attributes { get; }

        /// <summary>
        /// provide the new source (file path or URL) to load stylesheet from
        /// </summary>
        public string? SetSrc { get; set; }

        /// <summary>
        /// provide the stylesheet to load
        /// </summary>
        public string? SetStyleSheet { get; set; }

        /// <summary>
        /// provide the stylesheet data to load
        /// </summary>
        public CssData? SetStyleSheetData
        {
            get => _setStyleSheetData;
            set => _setStyleSheetData = value;
        }
    }
}