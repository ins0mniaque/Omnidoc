using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

using Omnidoc.Collections;

namespace Omnidoc
{
    [ Serializable ]
    public struct Size : IEquatable < Size >, IFormattable
    {
        public Size ( double width, double height )
        {
            this.width  = width  >= 0 ? width  : throw new ArgumentException ( "Width must be non-negative",  nameof ( width  ) );
            this.height = height >= 0 ? height : throw new ArgumentException ( "Height must be non-negative", nameof ( height ) );
        }

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

        public static   bool Equals ( Size left, Size right ) => left.Equals ( right );
        public          bool Equals ( Size   other )          => width == other.width && height == other.height;
        public override bool Equals ( object obj   )          => obj is Size other ? Equals ( other ) : false;
        public override int  GetHashCode ( )                  => HashCode.Combine ( width, height );

        [ SuppressMessage ( "Usage", "CA2225:Operator overloads have named alternates", Justification = "System.Drawing.Size" ) ]
        public static explicit operator Point ( Size size ) => new Point ( size.width, size.height );

        public static bool operator == ( Size left, Size right ) =>   left.Equals ( right );
        public static bool operator != ( Size left, Size right ) => ! left.Equals ( right );

        public static Size Parse ( string source )
        {
            if ( source == null )
                throw new ArgumentNullException ( nameof ( source ) );

            var index = 0;
            if ( NumberListParser.TryParse < double > ( double.TryParse, source, ref index, NumberStyles.Float, CultureInfo.InvariantCulture, out var width, out var height ) )
                return new Size ( width, height );

            throw new FormatException ( $"Invalid { nameof ( Size ) } format: { source }" );
        }

        public override string ToString ( )                                                 => ToString ( null, null );
        public          string ToString (                 IFormatProvider? formatProvider ) => ToString ( null, formatProvider );
        public          string ToString ( string? format, IFormatProvider? formatProvider ) => NumberListParser.Format ( formatProvider, format, width, height );
    }
}