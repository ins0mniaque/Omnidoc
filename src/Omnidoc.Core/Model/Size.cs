using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

using Omnidoc.Collections;

namespace Omnidoc.Model
{
    [ Serializable ]
    public struct Size : IEquatable < Size >, IFormattable
    {
        public Size ( double width, double height )
        {
            this.width  = width  >= 0 ? width  : throw new ArgumentException ( string.Format ( CultureInfo.InvariantCulture, Strings.Error_MustBeNonNegative, nameof ( Width  ) ), nameof ( width  ) );
            this.height = height >= 0 ? height : throw new ArgumentException ( string.Format ( CultureInfo.InvariantCulture, Strings.Error_MustBeNonNegative, nameof ( Height ) ), nameof ( height ) );
        }

        private double width;
        public  double Width
        {
            get => width;
            set => width = value >= 0 ? value : throw new ArgumentException ( string.Format ( CultureInfo.InvariantCulture, Strings.Error_MustBeNonNegative, nameof ( Width ) ), nameof ( value ) );
        }

        private double height;
        public  double Height
        {
            get => height;
            set => height = value >= 0 ? value : throw new ArgumentException ( string.Format ( CultureInfo.InvariantCulture, Strings.Error_MustBeNonNegative, nameof ( Height ) ), nameof ( value ) );
        }

        public static   bool Equals ( Size left, Size right ) => left.Equals ( right );
        public          bool Equals ( Size   other )          => width == other.width && height == other.height;
        public override bool Equals ( object other )          => other is Size ? Equals ( (Size) other ) : false;
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

            throw new FormatException ( string.Format ( CultureInfo.InvariantCulture, Strings.Error_InvalidFormat, nameof ( Size ), source ) );
        }

        public override string ToString ( )                                           => ToString ( null, null );
        public          string ToString (                 IFormatProvider? provider ) => ToString ( null, provider );
        public          string ToString ( string? format, IFormatProvider? provider ) => NumberListParser.Format ( provider, format, width, height );
    }
}