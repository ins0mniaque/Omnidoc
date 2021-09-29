using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

using Omnidoc.Dom.Abstractions;
using Omnidoc.Dom.Rendering;

namespace Omnidoc.Dom
{
    public abstract class Containable : IContainable
    {
        public IElement? Parent { get; private set; }

        void IContainable.SetParent ( IElement? parent ) => Parent = parent;
    }

    // TODO: Override Equals/GetHashCode
    public abstract class Implementable : Containable, IEquatable < IElement >
    {
        protected abstract object GetImplementation ( );

        public bool Equals ( IElement other ) => ReferenceEquals ( this, other ) || other is Implementable otherElement && ReferenceEquals ( GetImplementation ( ), otherElement.GetImplementation ( ) );
    }

    // TODO: Metadata (e.g. Title/Alt from HTML)
    public abstract class Element : Implementable, IElement
    {
        public abstract IElement<TOtherLayout, TOtherStyle> Cast<TOtherLayout, TOtherStyle> ( )
            where TOtherLayout : class, new()
            where TOtherStyle : class, new();
    }

    // TODO: Implement Equals/GetHashCode
    public abstract class Geometry : IEquatable < Geometry >
    {
        public static Geometry Empty { get; } = new Path { Markup = string.Empty };

        public static Geometry Parse ( string path )
        {
            var markup = new PathMarkup ( );
            PathMarkup.Render ( path, markup );
            return new Path { Markup = markup.ToString ( ) };
        }

        public static Geometry? TryParse ( string path )
        {
            var markup = new PathMarkup ( );
            var length = PathMarkup.TryRender ( path, markup );
            if ( length != path.Length )
                return null;

            return new Path { Markup = markup.ToString ( ) };
        }

        public static bool Equals      ( Geometry left, Geometry right ) => throw new NotImplementedException ( );
        public static int  GetHashCode ( Geometry geometry )             => throw new NotImplementedException ( );

        public static Geometry Union     ( Geometry left, Geometry right ) => throw new NotImplementedException ( );
        public static Geometry Intersect ( Geometry left, Geometry right ) => throw new NotImplementedException ( );
        public static Geometry Exclude   ( Geometry left, Geometry right ) => throw new NotImplementedException ( );
        public static Geometry Xor       ( Geometry left, Geometry right ) => throw new NotImplementedException ( );

        public static Geometry operator + (Geometry left, Geometry right) => Union     ( left, right );
        public static Geometry operator - (Geometry left, Geometry right) => Exclude   ( left, right );
        public static Geometry operator | (Geometry left, Geometry right) => Union     ( left, right );
        public static Geometry operator & (Geometry left, Geometry right) => Intersect ( left, right );
        public static Geometry operator ^ (Geometry left, Geometry right) => Xor       ( left, right );

        public abstract void Render ( IGeometryRenderer renderer );
        public virtual  bool Equals ( Geometry          other    ) => Equals ( this, other );
        public override bool Equals ( object obj ) => obj is Geometry other && Equals ( other );
        public override int  GetHashCode ( ) => GetHashCode ( this );

        public static bool operator ==(Geometry left, Geometry right) => left.Equals ( right );
        public static bool operator !=(Geometry left, Geometry right) => ! left.Equals ( right );

        private class Path : Geometry
        {
            public string Markup { get; set; } = string.Empty;

            public override void Render ( IGeometryRenderer renderer ) => PathMarkup.Render ( Markup, renderer );
            public override bool Equals ( Geometry          other    ) => other is Path otherPath ? Markup == otherPath.Markup : base.Equals ( other );
        }
    }

    // TODO: Color blending methods?
    public partial struct Color
    {
        // TODO: Parse/ParseHex
        public static Color AliceBlue { get; } = new Color ( );
    }

    [ StructLayout ( LayoutKind.Explicit ) ]
    public partial struct Color : IEquatable < Color >
    {
        [ FieldOffset ( 0 ) ] private readonly ushort red;
        [ FieldOffset ( 2 ) ] private readonly ushort green;
        [ FieldOffset ( 4 ) ] private readonly ushort blue;
        [ FieldOffset ( 6 ) ] private readonly ushort alpha;
        [ FieldOffset ( 0 ) ] private readonly ulong  value;

        private Color ( ulong value ) : this ( )
        {
            this.value = value;
        }

        private Color ( ushort red, ushort green, ushort blue, ushort alpha ) : this ( )
        {
            this.red   = red;
            this.green = green;
            this.blue  = blue;
            this.alpha = alpha;
        }

        public static Color FromRgb  ( byte r, byte g, byte b )         => new Color ( FromByte ( r ), FromByte ( g ), FromByte ( b ), ushort.MaxValue );
        public static Color FromRgba ( byte r, byte g, byte b, byte a ) => new Color ( FromByte ( r ), FromByte ( g ), FromByte ( b ), FromByte ( a )  );

        [ CLSCompliant ( false ) ] public static Color FromValue ( ulong value )                            => new Color ( value );
        [ CLSCompliant ( false ) ] public static Color FromRgb   ( ushort r, ushort g, ushort b )           => new Color ( r, g, b, ushort.MaxValue );
        [ CLSCompliant ( false ) ] public static Color FromRgba  ( ushort r, ushort g, ushort b, ushort a ) => new Color ( r, g, b, a );

        [ CLSCompliant ( false ) ] public ushort Red   => red;
        [ CLSCompliant ( false ) ] public ushort Green => green;
        [ CLSCompliant ( false ) ] public ushort Blue  => blue;
        [ CLSCompliant ( false ) ] public ushort Alpha => alpha;
        [ CLSCompliant ( false ) ] public ulong  Value => value;

        public byte R => ToByte ( Red   );
        public byte G => ToByte ( Green );
        public byte B => ToByte ( Blue  );
        public byte A => ToByte ( Alpha );

        // CLS compliance
        public static Color FromInt64 ( long value ) => new Color ( unchecked ( (ulong) value ) );
        public        long  ToInt64   ( )            => unchecked ( (long) value );

        private static byte ToByte ( ushort value )
        {
            const float Ratio = byte.MaxValue / (float) ushort.MaxValue;

            return (byte) Math.Round ( value * Ratio );
        }

        private static ushort FromByte ( byte value )
        {
            const float Ratio = ushort.MaxValue / (float) byte.MaxValue;

            return (ushort) Math.Round ( value * Ratio );
        }

        public bool Equals ( Color other ) => value == other.value;
        public override bool Equals ( object obj ) => obj is Color other && Equals ( other );
        public override int GetHashCode ( ) => value.GetHashCode ( );

        public static bool operator ==(Color left, Color right) => left.Equals ( right );
        public static bool operator !=(Color left, Color right) => ! left.Equals ( right );
    }

    public sealed class Font : IEquatable < Font >
    {
        public string? Name   { get; set; }
        public double  Size   { get; set; }
        public int     Weight { get; set; }

        public bool Equals ( Font other ) => Name == other.Name && Size == other.Size && Equals ( Weight, other.Weight );
        public override bool Equals ( object obj ) => obj is Font other && Equals ( other );
        public override int GetHashCode ( ) => HashCode.Combine ( Name, Size, Weight );

        public static bool operator ==(Font left, Font right) => left.Equals ( right );
        public static bool operator !=(Font left, Font right) => ! left.Equals ( right );
    }

    public sealed class Pen : IEquatable < Pen >
    {
        public Brush? Stroke { get; set; }
        public double Width  { get; set; }

        public bool Equals ( Pen other ) => Width == other.Width && Equals ( Stroke, other.Stroke );
        public override bool Equals ( object obj ) => obj is Pen other && Equals ( other );
        public override int GetHashCode ( ) => HashCode.Combine ( Stroke, Width );

        public static bool operator ==(Pen left, Pen right) => left.Equals ( right );
        public static bool operator !=(Pen left, Pen right) => ! left.Equals ( right );
    }

    public sealed class Glyphs : IEquatable<Glyphs>
    {
        // System.Windows.Documents.Glyphs.Indices
        // [GlyphIndex][,[Advance][,[uOffset][,[vOffset]]]]
        public bool Equals ( Glyphs other ) => true;
        public override bool Equals ( object obj ) => obj is Glyphs other && Equals ( other );
        public override int GetHashCode ( ) => 0;

        public static bool operator ==(Glyphs left, Glyphs right) => left.Equals ( right );
        public static bool operator !=(Glyphs left, Glyphs right) => ! left.Equals ( right );
    }

    public abstract class Brush : IEquatable < Brush >
    {
        public abstract T Adapt < T > ( IBrush < T > brush );
        public abstract bool Equals ( Brush other );
        public override bool Equals ( object obj ) => obj is Brush other && Equals ( other );
        public override int GetHashCode ( ) => 0;

        public static bool operator ==(Brush left, Brush right) => left.Equals ( right );
        public static bool operator !=(Brush left, Brush right) => ! left.Equals ( right );
    }

    public class ColorBrush : Brush
    {
        public Color Color { get; set; }

        public override T Adapt < T > ( IBrush < T > brush ) => brush.Color ( Color );

        public override bool Equals ( Brush other ) => other is ColorBrush otherBrush &&
                                                       Color == otherBrush.Color;

        public override int GetHashCode ( ) => Color.GetHashCode ( );
    }

    public abstract class GradientBrush : Brush
    {
        public Gradient Gradient { get; } = new Gradient ( );
    }

    public sealed class Gradient : List < GradientStop >, IEquatable < Gradient >
    {
        public bool Equals ( Gradient other ) => other is not null && this.SequenceEqual ( other );
        public override bool Equals ( object obj ) => obj is Gradient other && Equals ( other );
        public override int GetHashCode ( ) => this.Aggregate(0, HashCode.Combine);

        public static bool operator ==(Gradient left, Gradient right) => left.Equals ( right );
        public static bool operator !=(Gradient left, Gradient right) => ! left.Equals ( right );
    }

    public sealed class GradientStop : IEquatable < GradientStop >
    {
        public Color  Color  { get; }
        public double Offset { get; }

        public bool Equals ( GradientStop other ) => other is not null && Color == other.Color && Offset == other.Offset;
        public override bool Equals ( object obj ) => obj is GradientStop other && Equals ( other );
        public override int GetHashCode ( ) => HashCode.Combine ( Color, Offset );

        public static bool operator ==(GradientStop left, GradientStop right) => left.Equals ( right );
        public static bool operator !=(GradientStop left, GradientStop right) => ! left.Equals ( right );
    }

    public class LinearGradientBrush : GradientBrush
    {
        public Point StartPoint { get; set; }
        public Point EndPoint   { get; set; } = new Point ( 1, 1 );

        public override T Adapt < T > ( IBrush < T > brush ) => brush.LinearGradient ( StartPoint, EndPoint, Gradient );

        public override bool Equals ( Brush other ) => other is LinearGradientBrush otherBrush &&
                                                       StartPoint == otherBrush.StartPoint &&
                                                       EndPoint  == otherBrush.EndPoint &&
                                                       Gradient == otherBrush.Gradient;

        public override int GetHashCode ( ) => HashCode.Combine ( StartPoint, EndPoint, Gradient );
    }

    public class RadialGradientBrush : GradientBrush
    {
        public Point  Center  { get; set; }
        public double RadiusX { get; set; }
        public double RadiusY { get; set; }

        public override T Adapt < T > ( IBrush < T > brush ) => brush.RadialGradient ( Center, RadiusX, RadiusY, Gradient );

        public override bool Equals ( Brush other ) => other is RadialGradientBrush otherBrush &&
                                                       Center   == otherBrush.Center &&
                                                       RadiusX  == otherBrush.RadiusX &&
                                                       RadiusY  == otherBrush.RadiusY &&
                                                       Gradient == otherBrush.Gradient;

        public override int GetHashCode ( ) => HashCode.Combine ( Center, RadiusX, RadiusY, Gradient );
    }
    public class GeometryGradientBrush : GradientBrush
    {
        public Geometry Geometry    { get; set; } = Geometry.Empty;
        public Color    CenterColor { get; set; }

        public override T Adapt < T > ( IBrush < T > brush ) => brush.GeometryGradient ( Geometry ?? Geometry.Empty, CenterColor, Gradient );

        public override bool Equals ( Brush other ) => other is GeometryGradientBrush otherBrush &&
                                                       Geometry    == otherBrush.Geometry &&
                                                       CenterColor == otherBrush.CenterColor &&
                                                       Gradient    == otherBrush.Gradient;

        public override int GetHashCode ( ) => HashCode.Combine ( Geometry, CenterColor, Gradient );
    }

    public sealed class TileBrush : Brush, IAsyncRenderable
    {
        public IAsyncRenderable Tile { get; set; }

        public bool IsLoadedFor ( ISurface surface, Rect bounds ) => Tile.IsLoadedFor ( surface, bounds );

        public Task LoadAsync ( ISurface surface, Rect bounds, CancellationToken cancellationToken = default )
        {
            return Tile.LoadAsync ( surface, bounds, cancellationToken );
        }

        public void Render ( ISurface surface, Rect bounds ) => Tile.Render ( surface, bounds );

        public override T Adapt<T> ( IBrush<T> brush ) => brush.Renderable ( this );

        public override bool Equals ( Brush other ) => other is TileBrush otherBrush &&
                                                       Tile == otherBrush.Tile;

        public override int GetHashCode ( ) => Tile.GetHashCode ( );
    }
}