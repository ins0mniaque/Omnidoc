using System;
using System.Globalization;

using Omnidoc.Collections;

namespace Omnidoc
{
    [ Serializable ]
    public struct Rect : IEquatable < Rect >, IFormattable
    {
        public Rect ( double width, double height ) : this ( 0, 0, width, height ) { }
        public Rect ( double x, double y, double width, double height )
        {
            X = x;
            Y = y;
            this.width  = width  >= 0 ? width  : throw new ArgumentException ( "Width must be non-negative",  nameof ( width  ) );
            this.height = height >= 0 ? height : throw new ArgumentException ( "Height must be non-negative", nameof ( height ) );
        }

        public double X { get; set; }
        public double Y { get; set; }

        private double width;
        public  double Width
        {
            get => width;
            set => width = value >= 0 ? value : throw new ArgumentException ( "Width must be non-negative", nameof ( value ) );
        }

        private double height;
        public  double Height
        {
            get => height;
            set => height = value >= 0 ? value : throw new ArgumentException ( "Height must be non-negative", nameof ( value ) );
        }

        // TODO: Union/Intersect/Points/Size
        public double Left   => X;
        public double Top    => Y;
        public double Right  => X + Width;
        public double Bottom => Y + Height;

        public static   bool Equals ( Rect left, Rect right ) => left.Equals ( right );
        public          bool Equals ( Rect   other )          => X == other.X && Y == other.Y && width == other.width && height == other.height;
        public override bool Equals ( object obj   )          => obj is Rect other && Equals ( other );
        public override int  GetHashCode ( )                  => HashCode.Combine ( X, Y, width, height );

        public static bool operator == ( Rect left, Rect right ) =>   left.Equals ( right );
        public static bool operator != ( Rect left, Rect right ) => ! left.Equals ( right );

        public static Rect Parse ( string source )
        {
            if ( source == null )
                throw new ArgumentNullException ( nameof ( source ) );

            var index = 0;
            if ( NumberListParser.TryParse < double > ( double.TryParse, source, ref index, NumberStyles.Float, CultureInfo.InvariantCulture, out var x, out var y ) )
            {
                if ( index++ >= 0 && NumberListParser.TryParse < double > ( double.TryParse, source, ref index, NumberStyles.Float, CultureInfo.InvariantCulture, out var width, out var height ) )
                    return new Rect ( x, y, width, height );

                return new Rect ( width = x, height = y );
            }

            throw new FormatException ( $"Invalid { nameof ( Rect ) } format: { source }" );
        }

        public override string ToString ( )                                                 => ToString ( null, null );
        public          string ToString (                 IFormatProvider? formatProvider ) => ToString ( null, formatProvider );
        public          string ToString ( string? format, IFormatProvider? formatProvider )
        {
            return X == 0 && Y == 0 ? NumberListParser.Format ( formatProvider, format,       Width, Height ) :
                                      NumberListParser.Format ( formatProvider, format, X, Y, Width, Height );
        }
    }
}