using System;
using PdfSharpCore;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using Omnidoc.Html.Pdf.Renderer.Adapters;
using Omnidoc.Html.Renderer.Core;
using Omnidoc.Html.Renderer.Core.Entities;
using Omnidoc.Html.Renderer.Core.Utils;

namespace Omnidoc.Html.Pdf.Renderer
{
    public static class PdfGenerator
    {
        /// <summary>
        /// Adds a font mapping from <paramref name="fromFamily"/> to <paramref name="toFamily"/> iff the <paramref name="fromFamily"/> is not found.<br/>
        /// When the <paramref name="fromFamily"/> font is used in rendered html and is not found in existing
        /// fonts (installed or added) it will be replaced by <paramref name="toFamily"/>.<br/>
        /// </summary>
        /// <remarks>
        /// This fonts mapping can be used as a fallback in case the requested font is not installed in the client system.
        /// </remarks>
        /// <param name="fromFamily">the font family to replace</param>
        /// <param name="toFamily">the font family to replace with</param>
        public static void AddFontFamilyMapping(string fromFamily, string toFamily)
        {
            ArgChecker.AssertArgNotNullOrEmpty(fromFamily, "fromFamily");
            ArgChecker.AssertArgNotNullOrEmpty(toFamily, "toFamily");

            PdfSharpAdapter.Instance.AddFontFamilyMapping(fromFamily, toFamily);
        }

        /// <summary>
        /// Parse the given stylesheet to <see cref="CssData"/> object.<br/>
        /// If <paramref name="combineWithDefault"/> is true the parsed css blocks are added to the
        /// default css data (as defined by W3), merged if class name already exists. If false only the data in the given stylesheet is returned.
        /// </summary>
        /// <seealso cref="http://www.w3.org/TR/CSS21/sample.html"/>
        /// <param name="stylesheet">the stylesheet source to parse</param>
        /// <param name="combineWithDefault">true - combine the parsed css data with default css data, false - return only the parsed css data</param>
        /// <returns>the parsed css data</returns>
        public static CssData ParseStyleSheet(string stylesheet, bool combineWithDefault = true) => CssData.Parse(PdfSharpAdapter.Instance, stylesheet, combineWithDefault);

        /// <summary>
        /// Create PDF document from given HTML.<br/>
        /// </summary>
        /// <param name="html">HTML source to create PDF from</param>
        /// <param name="pageSize">the page size to use for each page in the generated pdf </param>
        /// <param name="margin">the margin to use between the HTML and the edges of each page</param>
        /// <param name="cssData">optional: the style to use for html rendering (default - use W3 default style)</param>
        /// <param name="stylesheetLoad">optional: can be used to overwrite stylesheet resolution logic</param>
        /// <param name="imageLoad">optional: can be used to overwrite image resolution logic</param>
        /// <returns>the generated image of the html</returns>
        public static PdfDocument FromHtml(string html, PageSize pageSize, int margin = 20, CssData? cssData = null,
            EventHandler<HtmlStylesheetLoadEventArgs>? stylesheetLoad = null, EventHandler<HtmlImageLoadEventArgs>? imageLoad = null)
        {
            var config = new PdfPageOptions
            {
                PageSize = pageSize
            };
            config.SetMargins(margin);
            return FromHtml(html, config, cssData, stylesheetLoad, imageLoad);
        }

        /// <summary>
        /// Create PDF document from given HTML.<br/>
        /// </summary>
        /// <param name="html">HTML source to create PDF from</param>
        /// <param name="options">the options to use for the PDF generation (page size/page orientation/margins/etc.)</param>
        /// <param name="cssData">optional: the style to use for html rendering (default - use W3 default style)</param>
        /// <param name="stylesheetLoad">optional: can be used to overwrite stylesheet resolution logic</param>
        /// <param name="imageLoad">optional: can be used to overwrite image resolution logic</param>
        /// <returns>the generated image of the html</returns>
        public static PdfDocument FromHtml(string html, PdfPageOptions options, CssData? cssData = null,
            EventHandler<HtmlStylesheetLoadEventArgs>? stylesheetLoad = null, EventHandler<HtmlImageLoadEventArgs>? imageLoad = null)
        {
            // create PDF document to render the HTML into
            var document = new PdfDocument();

            // add rendered PDF pages to document
            AddPagesFromHtml(document, html, options, cssData, stylesheetLoad, imageLoad);

            return document;
        }

        /// <summary>
        /// Create PDF pages from given HTML and appends them to the provided PDF document.<br/>
        /// </summary>
        /// <param name="document">PDF document to append pages to</param>
        /// <param name="html">HTML source to create PDF from</param>
        /// <param name="pageSize">the page size to use for each page in the generated pdf </param>
        /// <param name="margin">the margin to use between the HTML and the edges of each page</param>
        /// <param name="cssData">optional: the style to use for html rendering (default - use W3 default style)</param>
        /// <param name="stylesheetLoad">optional: can be used to overwrite stylesheet resolution logic</param>
        /// <param name="imageLoad">optional: can be used to overwrite image resolution logic</param>
        /// <returns>the generated image of the html</returns>
        public static void AddPagesFromHtml(this PdfDocument document, string html, PageSize pageSize, int margin = 20, CssData? cssData = null,
            EventHandler<HtmlStylesheetLoadEventArgs>? stylesheetLoad = null, EventHandler<HtmlImageLoadEventArgs>? imageLoad = null)
        {
            var config = new PdfPageOptions
            {
                PageSize = pageSize
            };
            config.SetMargins(margin);
            document.AddPagesFromHtml(html, config, cssData, stylesheetLoad, imageLoad);
        }

        /// <summary>
        /// Create PDF pages from given HTML and appends them to the provided PDF document.<br/>
        /// </summary>
        /// <param name="document">PDF document to append pages to</param>
        /// <param name="html">HTML source to create PDF from</param>
        /// <param name="options">the options to use for the PDF generation (page size/page orientation/margins/etc.)</param>
        /// <param name="cssData">optional: the style to use for html rendering (default - use W3 default style)</param>
        /// <param name="stylesheetLoad">optional: can be used to overwrite stylesheet resolution logic</param>
        /// <param name="imageLoad">optional: can be used to overwrite image resolution logic</param>
        /// <returns>the generated image of the html</returns>
        public static void AddPagesFromHtml(this PdfDocument document, string html, PdfPageOptions options, CssData? cssData = null,
            EventHandler<HtmlStylesheetLoadEventArgs>? stylesheetLoad = null, EventHandler<HtmlImageLoadEventArgs>? imageLoad = null)
        {
            if ( document is null ) throw new ArgumentNullException ( nameof ( document ) );
            if ( options  is null ) throw new ArgumentNullException ( nameof ( options  ) );

            XSize orgPageSize;
            // get the size of each page to layout the HTML in
            if (options.PageSize != PageSize.Undefined)
                orgPageSize = PageSizeConverter.ToSize(options.PageSize);
            else
                orgPageSize = options.ManualPageSize;

            if (options.PageOrientation == PageOrientation.Landscape)
            {
                // invert pagesize for landscape
                orgPageSize = new XSize(orgPageSize.Height, orgPageSize.Width);
            }

            var pageSize = new XSize(orgPageSize.Width - options.MarginLeft - options.MarginRight, orgPageSize.Height - options.MarginTop - options.MarginBottom);

            if (!string.IsNullOrEmpty(html))
            {
                using var container = new HtmlContainer();
                if (stylesheetLoad != null)
                    container.StylesheetLoad += stylesheetLoad;
                if (imageLoad != null)
                    container.ImageLoad += imageLoad;

                container.Location = new XPoint(options.MarginLeft, options.MarginTop);
                container.MaxSize = new XSize(pageSize.Width, 0);
                container.SetHtml(html, cssData);
                container.PageSize = pageSize;
                container.MarginBottom = options.MarginBottom;
                container.MarginLeft = options.MarginLeft;
                container.MarginRight = options.MarginRight;
                container.MarginTop = options.MarginTop;

                // layout the HTML with the page width restriction to know how many pages are required
                using (var measure = XGraphics.CreateMeasureContext(pageSize, XGraphicsUnit.Point, XPageDirection.Downwards))
                {
                    container.PerformLayout(measure);
                }

                // while there is un-rendered HTML, create another PDF page and render with proper offset for the next page
                double scrollOffset = 0;
                while (scrollOffset > -container.ActualSize.Height)
                {
                    var page = document.AddPage();
                    page.Height = orgPageSize.Height;
                    page.Width = orgPageSize.Width;

                    using (var g = XGraphics.FromPdfPage(page))
                    {
                        //g.IntersectClip(new XRect(config.MarginLeft, config.MarginTop, pageSize.Width, pageSize.Height));
                        g.IntersectClip(new XRect(0, 0, page.Width, page.Height));

                        container.ScrollOffset = new XPoint(0, scrollOffset);
                        container.PerformPaint(g);
                    }
                    scrollOffset -= pageSize.Height;
                }

                // add web links and anchors
                HandleLinks(document, container, orgPageSize, pageSize);
            }
        }

        /// <summary>
        /// Handle HTML links by create PDF Documents link either to external URL or to another page in the document.
        /// </summary>
        private static void HandleLinks(PdfDocument document, HtmlContainer container, XSize orgPageSize, XSize pageSize)
        {
            foreach (var link in container.GetLinks())
            {
                var i = (int)(link.Rectangle.Top / pageSize.Height);
                for (; i < document.Pages.Count && pageSize.Height * i < link.Rectangle.Bottom; i++)
                {
                    var offset = pageSize.Height * i;

                    // fucking position is from the bottom of the page
                    var xRect = new XRect(link.Rectangle.Left, orgPageSize.Height - (link.Rectangle.Height + link.Rectangle.Top - offset), link.Rectangle.Width, link.Rectangle.Height);

                    if (link.IsAnchor)
                    {
                        // create link to another page in the document
                        var anchorRect = container.GetElementRectangle(link.AnchorId);
                        if (anchorRect.HasValue)
                        {
                            // document links to the same page as the link is not allowed
                            var anchorPageIdx = (int)(anchorRect.Value.Top / pageSize.Height);
                            if (i != anchorPageIdx)
                                document.Pages[i].AddDocumentLink(new PdfRectangle(xRect), anchorPageIdx);
                        }
                    }
                    else
                    {
                        // create link to URL
                        document.Pages[i].AddWebLink(new PdfRectangle(xRect), link.Href);
                    }
                }
            }
        }
    }
}