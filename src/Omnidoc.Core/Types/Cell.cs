using System;
using System.Globalization;

using Omnidoc.Collections;

namespace Omnidoc
{
    [ Serializable ]
    public struct Cell : IEquatable < Cell >, IFormattable
    {
        public Cell ( int column, int row ) : this ( column, row, 1, 1 ) { }
        public Cell ( int column, int row, int columnSpan, int rowSpan )
        {
            Column     = column;
            Row        = row;
            ColumnSpan = columnSpan;
            RowSpan    = rowSpan;
        }

        public int Column     { get; set; }
        public int Row        { get; set; }
        public int ColumnSpan { get; set; }
        public int RowSpan    { get; set; }

        public static   bool Equals ( Cell left, Cell right ) => left.Equals ( right );
        public          bool Equals ( Cell   other )          => Column == other.Column && Row == other.Row && ColumnSpan == other.ColumnSpan && RowSpan == other.RowSpan;
        public override bool Equals ( object obj   )          => obj is Cell other ? Equals ( other ) : false;
        public override int  GetHashCode ( )                  => HashCode.Combine ( Column, Row, ColumnSpan, RowSpan );

        public static bool operator == ( Cell left, Cell right ) =>   left.Equals ( right );
        public static bool operator != ( Cell left, Cell right ) => ! left.Equals ( right );

        public static Cell Parse ( string source )
        {
            if ( source == null )
                throw new ArgumentNullException ( nameof ( source ) );

            var index = 0;
            if ( NumberListParser.TryParse < int > ( int.TryParse, source, ref index, NumberStyles.Integer, CultureInfo.InvariantCulture, out var column, out var row ) )
            {
                if ( index++ >= 0 && NumberListParser.TryParse < int > ( int.TryParse, source, ref index, NumberStyles.Integer, CultureInfo.InvariantCulture, out var columnSpan, out var rowSpan ) )
                    return new Cell ( column, row, columnSpan, rowSpan );

                return new Cell ( column, row );
            }

            throw new FormatException ( string.Format ( CultureInfo.InvariantCulture, Strings.Error_InvalidFormat, nameof ( Cell ), source ) );
        }

        public override string ToString ( )                                                 => ToString ( null, null );
        public          string ToString (                 IFormatProvider? formatProvider ) => ToString ( null, formatProvider );
        public          string ToString ( string? format, IFormatProvider? formatProvider )
        {
            return ColumnSpan == 1 && RowSpan == 1 ? NumberListParser.Format ( formatProvider, format, Column, Row ) :
                                                     NumberListParser.Format ( formatProvider, format, Column, Row, ColumnSpan, RowSpan );
        }
    }
}