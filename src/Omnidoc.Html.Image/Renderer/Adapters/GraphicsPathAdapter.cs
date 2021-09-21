using System;
using SixLabors.ImageSharp.Drawing;
using Omnidoc.Html.Renderer.Adapters;
using Omnidoc.Html.Renderer.Adapters.Entities;

namespace Omnidoc.Html.Image.Renderer.Adapters
{
    /// <summary>
    /// Adapter for ImageSharp graphics path object for core.
    /// </summary>
    internal sealed class GraphicsPathAdapter : RGraphicsPath
    {
        /// <summary>
        /// the last point added to the path to begin next segment from
        /// </summary>
        private RPoint _lastPoint;

        /// <summary>
        /// The actual ImageSharp graphics path object instance.
        /// </summary>
        public PathBuilder PathBuilder { get; } = new PathBuilder ( );

        public override void Start(double x, double y) => _lastPoint = new RPoint(x, y);

        public override void LineTo(double x, double y)
        {
            PathBuilder.AddLine((float)_lastPoint.X, (float)_lastPoint.Y, (float)x, (float)y);
            _lastPoint = new RPoint(x, y);
        }

        // TODO: Enable when SixLabors.ImageSharp.Drawing package supports arcs: https://github.com/SixLabors/ImageSharp.Drawing/pull/144
        public override void ArcTo(double x, double y, double size, Corner corner)
        {
            var left = (float)(Math.Min(x, _lastPoint.X) - (corner == Corner.TopRight || corner == Corner.BottomRight ? size : 0));
            var top = (float)(Math.Min(y, _lastPoint.Y) - (corner == Corner.BottomLeft || corner == Corner.BottomRight ? size : 0));
            // PathBuilder.AddEllipticalArc(left, top, (float)size * 2, (float)size * 2, 0f, GetStartAngle(corner), 90);
            _lastPoint = new RPoint(x, y);
        }

        /// <summary>
        /// Get arc start angle for the given corner.
        /// </summary>
        private static int GetStartAngle(Corner corner)
        {
            int startAngle;
            switch (corner)
            {
                case Corner.TopLeft:
                    startAngle = 180;
                    break;
                case Corner.TopRight:
                    startAngle = 270;
                    break;
                case Corner.BottomLeft:
                    startAngle = 90;
                    break;
                case Corner.BottomRight:
                    startAngle = 0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(corner));
            }
            return startAngle;
        }
    }
}