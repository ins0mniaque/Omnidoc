using SixLabors.ImageSharp.Drawing.Processing;
using Omnidoc.Html.Renderer.Adapters.Entities;
using Omnidoc.Html.Renderer.Adapters;

namespace Omnidoc.Html.Image.Renderer.Adapters
{
    /// <summary>
    /// Adapter for ImageSharp pens objects for core.
    /// </summary>
    internal sealed class PenAdapter : RPen
    {
        public PenAdapter ( Pen pen )
        {
            Pen = pen;
        }

        /// <summary>
        /// The actual ImageSharp brush instance.
        /// </summary>
        public IPen Pen { get; private set; }

        public override double Width
        {
            get => Pen.StrokeWidth;
            set => Pen = new Pen ( Pen.StrokeFill, (float) value, Pen.StrokePattern.ToArray ( ) );
        }

        public override RDashStyle DashStyle
        {
            set => Pen = value switch
            {
                RDashStyle.Solid      => Pens.Solid      ( Pen.StrokeFill, Pen.StrokeWidth ),
                RDashStyle.Dash       => Pens.Dash       ( Pen.StrokeFill, Pen.StrokeWidth ),
                RDashStyle.Dot        => Pens.Dot        ( Pen.StrokeFill, Pen.StrokeWidth ),
                RDashStyle.DashDot    => Pens.DashDot    ( Pen.StrokeFill, Pen.StrokeWidth ),
                RDashStyle.DashDotDot => Pens.DashDotDot ( Pen.StrokeFill, Pen.StrokeWidth ),
                RDashStyle.Custom     => new Pen         ( Pen.StrokeFill, Pen.StrokeWidth, CustomDashPattern ),
                _                     => Pens.Solid      ( Pen.StrokeFill, Pen.StrokeWidth )
            };
        }

        private static readonly float [ ] DefaultCustomDashPattern = new [ ] { 1f };

        public float [ ] CustomDashPattern { get; set; } = DefaultCustomDashPattern;
    }
}