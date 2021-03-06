using System;
using System.Collections.Generic;
using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Omnidoc.Html.Renderer.Adapters.Entities;
using Omnidoc.Html.Renderer.Core;
using Omnidoc.Html.Renderer.Core.Entities;
using Omnidoc.Html.Renderer.Core.Parse;
using Omnidoc.Html.Renderer.Core.Utils;
using Omnidoc.Html.Image.Renderer.Adapters;
using Omnidoc.Html.Image.Renderer.Utilities;

namespace Omnidoc.Html.Image.Renderer
{
    /// <summary>
    /// Low level handling of Html Renderer logic, this class is used by <see cref="HtmlParser"/>,
    /// <see cref="HtmlMeasurer"/> and <see cref="DrawHtmlExtensions"/>.
    /// </summary>
    /// <seealso cref="HtmlContainerInt"/>
    public sealed class HtmlContainer : IDisposable
    {
        /// <summary>
        /// The internal core html container
        /// </summary>
        public HtmlContainer()
        {
            HtmlContainerInt = new HtmlContainerInt(ImageSharpAdapter.Instance);
            HtmlContainerInt.SetMargins(0);
            HtmlContainerInt.PageSize = new RSize(99999, 99999);
        }

        /// <summary>
        /// Raised when the set html document has been fully loaded.<br/>
        /// Allows manipulation of the html dom, scroll position, etc.
        /// </summary>
        public event EventHandler LoadComplete
        {
            add { HtmlContainerInt.LoadComplete += value; }
            remove { HtmlContainerInt.LoadComplete -= value; }
        }

        /// <summary>
        /// Raised when the user clicks on a link in the html.<br/>
        /// Allows canceling the execution of the link.
        /// </summary>
        public event EventHandler<HtmlLinkClickedEventArgs> LinkClicked
        {
            add { HtmlContainerInt.LinkClicked += value; }
            remove { HtmlContainerInt.LinkClicked -= value; }
        }

        /// <summary>
        /// Raised when html renderer requires refresh of the control hosting (invalidation and re-layout).
        /// </summary>
        /// <remarks>
        /// There is no guarantee that the event will be raised on the main thread, it can be raised on thread-pool thread.
        /// </remarks>
        public event EventHandler<HtmlRefreshEventArgs> Refresh
        {
            add { HtmlContainerInt.Refresh += value; }
            remove { HtmlContainerInt.Refresh -= value; }
        }

        /// <summary>
        /// Raised when Html Renderer request scroll to specific location.<br/>
        /// This can occur on document anchor click.
        /// </summary>
        public event EventHandler<HtmlScrollEventArgs> ScrollChange
        {
            add { HtmlContainerInt.ScrollChange += value; }
            remove { HtmlContainerInt.ScrollChange -= value; }
        }

        /// <summary>
        /// Raised when an error occurred during html rendering.<br/>
        /// </summary>
        /// <remarks>
        /// There is no guarantee that the event will be raised on the main thread, it can be raised on thread-pool thread.
        /// </remarks>
        public event EventHandler<HtmlRenderErrorEventArgs> RenderError
        {
            add { HtmlContainerInt.RenderError += value; }
            remove { HtmlContainerInt.RenderError -= value; }
        }

        /// <summary>
        /// Raised when a stylesheet is about to be loaded by file path or URI by link element.<br/>
        /// This event allows to provide the stylesheet manually or provide new source (file or Uri) to load from.<br/>
        /// If no alternative data is provided the original source will be used.<br/>
        /// </summary>
        public event EventHandler<HtmlStylesheetLoadEventArgs> StylesheetLoad
        {
            add { HtmlContainerInt.StylesheetLoad += value; }
            remove { HtmlContainerInt.StylesheetLoad -= value; }
        }

        /// <summary>
        /// Raised when an image is about to be loaded by file path or URI.<br/>
        /// This event allows to provide the image manually, if not handled the image will be loaded from file or download from URI.
        /// </summary>
        public event EventHandler<HtmlImageLoadEventArgs> ImageLoad
        {
            add { HtmlContainerInt.ImageLoad += value; }
            remove { HtmlContainerInt.ImageLoad -= value; }
        }

        /// <summary>
        /// The internal core html container
        /// </summary>
        internal HtmlContainerInt HtmlContainerInt { get; }

        /// <summary>
        /// the parsed stylesheet data used for handling the html
        /// </summary>
        public CssData? CssData => HtmlContainerInt.CssData;

        /// <summary>
        /// Gets or sets a value indicating if anti-aliasing should be avoided for geometry like backgrounds and borders (default - false).
        /// </summary>
        public bool AvoidGeometryAntialias
        {
            get => HtmlContainerInt.AvoidGeometryAntialias;
            set => HtmlContainerInt.AvoidGeometryAntialias = value;
        }

        /// <summary>
        /// Gets or sets a value indicating if image asynchronous loading should be avoided (default - false).<br/>
        /// True - images are loaded synchronously during html parsing.<br/>
        /// False - images are loaded asynchronously to html parsing when downloaded from URL or loaded from disk.<br/>
        /// </summary>
        /// <remarks>
        /// Asynchronously image loading allows to unblock html rendering while image is downloaded or loaded from disk using IO
        /// ports to achieve better performance.<br/>
        /// Asynchronously image loading should be avoided when the full html content must be available during render, like render to image.
        /// </remarks>
        public bool AvoidAsyncImagesLoading
        {
            get => HtmlContainerInt.AvoidAsyncImagesLoading;
            set => HtmlContainerInt.AvoidAsyncImagesLoading = value;
        }

        /// <summary>
        /// Gets or sets a value indicating if image loading only when visible should be avoided (default - false).<br/>
        /// True - images are loaded as soon as the html is parsed.<br/>
        /// False - images that are not visible because of scroll location are not loaded until they are scrolled to.
        /// </summary>
        /// <remarks>
        /// Images late loading improve performance if the page contains image outside the visible scroll area, especially if there is large
        /// amount of images, as all image loading is delayed (downloading and loading into memory).<br/>
        /// Late image loading may effect the layout and actual size as image without set size will not have actual size until they are loaded
        /// resulting in layout change during user scroll.<br/>
        /// Early image loading may also effect the layout if image without known size above the current scroll location are loaded as they
        /// will push the html elements down.
        /// </remarks>
        public bool AvoidImagesLateLoading
        {
            get => HtmlContainerInt.AvoidImagesLateLoading;
            set => HtmlContainerInt.AvoidImagesLateLoading = value;
        }

        /// <summary>
        /// Is content selection is enabled for the rendered html (default - true).<br/>
        /// If set to 'false' the rendered html will be static only with ability to click on links.
        /// </summary>
        public bool IsSelectionEnabled
        {
            get => HtmlContainerInt.IsSelectionEnabled;
            set => HtmlContainerInt.IsSelectionEnabled = value;
        }

        /// <summary>
        /// Is the build-in context menu enabled and will be shown on mouse right click (default - true)
        /// </summary>
        public bool IsContextMenuEnabled
        {
            get => HtmlContainerInt.IsContextMenuEnabled;
            set => HtmlContainerInt.IsContextMenuEnabled = value;
        }

        /// <summary>
        /// The scroll offset of the html.<br/>
        /// This will adjust the rendered html by the given offset so the content will be "scrolled".<br/>
        /// </summary>
        /// <example>
        /// Element that is rendered at location (50,100) with offset of (0,200) will not be rendered as it
        /// will be at -100 therefore outside the client rectangle.
        /// </example>
        public PointF ScrollOffset
        {
            get => Utils.ConvertRound(HtmlContainerInt.ScrollOffset);
            set => HtmlContainerInt.ScrollOffset = Utils.Convert(value);
        }

        /// <summary>
        /// The top-left most location of the rendered html.<br/>
        /// This will offset the top-left corner of the rendered html.
        /// </summary>
        public PointF Location
        {
            get => Utils.Convert(HtmlContainerInt.Location);
            set => HtmlContainerInt.Location = Utils.Convert(value);
        }

        /// <summary>
        /// The max width and height of the rendered html.<br/>
        /// The max width will effect the html layout wrapping lines, resize images and tables where possible.<br/>
        /// The max height does NOT effect layout, but will not render outside it (clip).<br/>
        /// <see cref="ActualSize"/> can be exceed the max size by layout restrictions (unwrappable line, set image size, etc.).<br/>
        /// Set zero for unlimited (width\height separately).<br/>
        /// </summary>
        public SizeF MaxSize
        {
            get => Utils.Convert(HtmlContainerInt.MaxSize);
            set => HtmlContainerInt.MaxSize = Utils.Convert(value);
        }

        /// <summary>
        /// The actual size of the rendered html (after layout)
        /// </summary>
        public SizeF ActualSize
        {
            get => Utils.Convert(HtmlContainerInt.ActualSize);
            internal set => HtmlContainerInt.ActualSize = Utils.Convert(value);
        }

        /// <summary>
        /// Get the currently selected text segment in the html.
        /// </summary>
        public string? SelectedText => HtmlContainerInt.SelectedText;

        /// <summary>
        /// Copy the currently selected html segment with style.
        /// </summary>
        public string? SelectedHtml => HtmlContainerInt.SelectedHtml;

        /// <summary>
        /// Clear the current selection.
        /// </summary>
        public void ClearSelection() => HtmlContainerInt.ClearSelection();

        /// <summary>
        /// Init with optional document and stylesheet.
        /// </summary>
        /// <param name="htmlSource">the html to init with, init empty if not given</param>
        /// <param name="baseCssData">optional: the stylesheet to init with, init default if not given</param>
        public void SetHtml(string htmlSource, CssData? baseCssData = null) => HtmlContainerInt.SetHtml(htmlSource, baseCssData);

        /// <summary>
        /// Get html from the current DOM tree with style if requested.
        /// </summary>
        /// <param name="styleGen">Optional: controls the way styles are generated when html is generated (default: <see cref="HtmlGenerationStyle.Inline"/>)</param>
        /// <returns>generated html</returns>
        public string GetHtml(HtmlGenerationStyle styleGen = HtmlGenerationStyle.Inline) => HtmlContainerInt.GetHtml(styleGen);

        /// <summary>
        /// Get attribute value of element at the given x,y location by given key.<br/>
        /// If more than one element exist with the attribute at the location the inner most is returned.
        /// </summary>
        /// <param name="location">the location to find the attribute at</param>
        /// <param name="attribute">the attribute key to get value by</param>
        /// <returns>found attribute value or null if not found</returns>
        public string? GetAttributeAt(PointF location, string attribute) => HtmlContainerInt.GetAttributeAt(Utils.Convert(location), attribute);

        /// <summary>
        /// Get all the links in the HTML with the element rectangle and href data.
        /// </summary>
        /// <returns>collection of all the links in the HTML</returns>
        public IEnumerable<LinkElementData<RectangleF>> GetLinks() => HtmlContainerInt.GetLinks().Select(link => link.Convert(Utils.Convert));

        /// <summary>
        /// Get css link href at the given x,y location.
        /// </summary>
        /// <param name="location">the location to find the link at</param>
        /// <returns>css link href if exists or null</returns>
        public string? GetLinkAt(PointF location) => HtmlContainerInt.GetLinkAt(Utils.Convert(location));

        /// <summary>
        /// Get the rectangle of html element as calculated by html layout.<br/>
        /// Element if found by id (id attribute on the html element).<br/>
        /// Note: to get the screen rectangle you need to adjust by the hosting control.<br/>
        /// </summary>
        /// <param name="elementId">the id of the element to get its rectangle</param>
        /// <returns>the rectangle of the element or null if not found</returns>
        public RectangleF? GetElementRectangle(string elementId)
        {
            var r = HtmlContainerInt.GetElementRectangle(elementId);
            return r.HasValue ? Utils.Convert(r.Value) : (RectangleF?)null;
        }

        /// <summary>
        /// Measures the bounds of box and children, recursively.
        /// </summary>
        /// <param name="g">Device context to draw</param>
        public void PerformLayout(IImageProcessingContext g)
        {
            ArgChecker.AssertArgNotNull(g, "g");

            using var ig = new GraphicsAdapter(g);
            HtmlContainerInt.PerformLayout(ig);
        }

        /// <summary>
        /// Render the html using the given device.
        /// </summary>
        /// <param name="g">the device to use to render</param>
        public void PerformPaint(IImageProcessingContext g)
        {
            ArgChecker.AssertArgNotNull(g, "g");

            using var ig = new GraphicsAdapter(g);
            HtmlContainerInt.PerformPaint(ig);
        }

        public void Dispose() => HtmlContainerInt.Dispose();
    }
}