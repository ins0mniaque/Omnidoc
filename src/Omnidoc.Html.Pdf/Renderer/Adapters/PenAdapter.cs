using Omnidoc.Html.Renderer.Adapters;
using Omnidoc.Html.Renderer.Adapters.Entities;
using PdfSharpCore.Drawing;

namespace Omnidoc.Html.Pdf.Renderer.Adapters
{
    /// <summary>
    /// Adapter for ImageSharp pens objects for core.
    /// </summary>
    internal sealed class PenAdapter : RPen
    {
        /// <summary>
        /// Init.
        /// </summary>
        public PenAdapter(XPen pen)
        {
            Pen = pen;
        }

        /// <summary>
        /// The actual ImageSharp brush instance.
        /// </summary>
        public XPen Pen { get; }

        public override double Width
        {
            get { return Pen.Width; }
            set { Pen.Width = value; }
        }

        public override RDashStyle DashStyle
        {
            set
            {
                switch (value)
                {
                    case RDashStyle.Solid:
                        Pen.DashStyle = XDashStyle.Solid;
                        break;
                    case RDashStyle.Dash:
                        Pen.DashStyle = XDashStyle.Dash;
                        if (Width < 2)
                            Pen.DashPattern = new[] { 4, 4d }; // better looking
                        break;
                    case RDashStyle.Dot:
                        Pen.DashStyle = XDashStyle.Dot;
                        break;
                    case RDashStyle.DashDot:
                        Pen.DashStyle = XDashStyle.DashDot;
                        break;
                    case RDashStyle.DashDotDot:
                        Pen.DashStyle = XDashStyle.DashDotDot;
                        break;
                    case RDashStyle.Custom:
                        Pen.DashStyle = XDashStyle.Custom;
                        break;
                    default:
                        Pen.DashStyle = XDashStyle.Solid;
                        break;
                }
            }
        }
    }
}