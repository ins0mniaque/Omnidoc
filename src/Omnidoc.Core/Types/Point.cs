using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

using Omnidoc.Collections;

namespace Omnidoc
{
    [ Serializable ]
    public struct Point : IEquatable < Point >, IFormattable
    {
        public Point ( double x, double y )
        {
            X = x;
            Y = y;
        }

        public double X { get; set; }
        public double Y { get; set; }

        public void Offset ( double offsetX, double offsetY )
        {
            X += offsetX;
            Y += offsetY;
        }

        public static   bool Equals ( Point left, Point right ) => left.Equals ( right );
        public          bool Equals ( Point  other )            => X == other.X && Y == other.Y;
        public override bool Equals ( object obj   )            => obj is Point other ? Equals ( other ) : false;
        public override int  GetHashCode ( )                    => HashCode.Combine ( X, Y );

        public static Point Add         ( Point left, Point right ) => new Point ( left.X + right.X, left.Y + right.Y );
        public static Point Subtract    ( Point left, Point right ) => new Point ( left.X - right.X, left.Y - right.Y );
        public static Point operator +  ( Point left, Point right ) => Add      ( left, right );
        public static Point operator -  ( Point left, Point right ) => Subtract ( left, right );
        public static bool  operator == ( Point left, Point right ) =>   left.Equals ( right );
        public static bool  operator != ( Point left, Point right ) => ! left.Equals ( right );

        [ SuppressMessage ( "Usage", "CA2225:Operator overloads have named alternates", Justification = "System.Drawing.Point" ) ]
        public static explicit operator Size ( Point point ) => new Size ( point.X, point.Y );

        public static Point Parse ( string source )
        {
            if ( source == null )
                throw new ArgumentNullException ( nameof ( source ) );

            var index = 0;
            if ( NumberListParser.TryParse < double > ( double.TryParse, source, ref index, NumberStyles.Float, CultureInfo.InvariantCulture, out var x, out var y ) )
                return new Point ( x, y );

            throw new FormatException ( string.Format ( CultureInfo.InvariantCulture, Strings.Error_InvalidFormat, nameof ( Point ), source ) );
        }

        public override string ToString ( )                                                 => ToString ( null, null );
        public          string ToString (                 IFormatProvider? formatProvider ) => ToString ( null, formatProvider );
        public          string ToString ( string? format, IFormatProvider? formatProvider ) => NumberListParser.Format ( formatProvider, format, X, Y );
    }
}