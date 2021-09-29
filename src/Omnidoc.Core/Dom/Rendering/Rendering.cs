using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Omnidoc.Dom.Rendering
{
    public interface IRenderable
    {
        void Render ( ISurface surface, Rect bounds );
    }

    public interface IGeometryRenderer
    {
        void MoveTo           ( bool relative, float x, float y );
        void LineTo           ( bool relative, float x, float y );
        void HorizontalLineTo ( bool relative, float x          );
        void VerticalLineTo   ( bool relative,          float y );

        void CurveTo       ( bool relative, float x1, float y1, float x2, float y2, float x, float y );
        void SmoothCurveTo ( bool relative,                     float x2, float y2, float x, float y );
        void CurveTo       ( bool relative, float x1, float y1,                     float x, float y );
        void SmoothCurveTo ( bool relative,                                         float x, float y );

        void Arc ( bool relative, float rx, float ry, float angle, bool largeArc, bool sweep, float x, float y );

        void ClosePath ( );
    }

    public interface IBrush < T >
    {
        T Color            ( Color color );
        T LinearGradient   ( Point startPoint, Point endPoint, Gradient gradient );
        T RadialGradient   ( Point center, double radiusX, double radiusY, Gradient gradient );
        T GeometryGradient ( Geometry geometry, Color centerColor, Gradient gradient );
        T Renderable       ( IRenderable renderable );
    }

    public interface IShape
    {
        Pen?      GetStroke   ( Rect bounds );
        Brush?    GetFill     ( Rect bounds );
        Geometry? GetGeometry ( Rect bounds );
    }

    public interface IText
    {
        Font?   GetFont   ( );
        string? GetString ( );
        Glyphs? GetGlyphs ( );
    }

    public interface ITexture
    {
        Size Size { get; }
    }

    public interface ISurfaceFactory
    {
        ITextureSurface CreateTextureSurface ( );
        IMeasureSurface CreateMeasureSurface ( );
        ISurface        CreateSurface        ( );
    }

    // TODO: Add RefCountDisposable < T > : IDisposable < T > for caching implementation
    public interface ITextureSurface : IDisposable
    {
        bool              CanDraw   ( ITexture texture );
        Task < ITexture > LoadAsync ( Stream   stream, CancellationToken cancellationToken );
    }

    public interface IMeasureSurface : ITextureSurface
    {
        Rect Measure ( IShape   shape,   Rect bounds );
        Rect Measure ( IText    text,    Rect bounds );
        Rect Measure ( ITexture texture, Rect bounds );
    }

    public interface ISurface : IMeasureSurface
    {
        Size Size { get; }

        void Draw ( IShape   shape,   Rect bounds );
        void Draw ( IText    text,    Rect bounds );
        void Draw ( ITexture texture, Rect bounds );
    }
}