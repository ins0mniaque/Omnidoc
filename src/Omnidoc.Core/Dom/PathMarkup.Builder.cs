using System.Text;

namespace Omnidoc.Dom
{
    public partial class PathMarkup : Rendering.IGeometryRenderer
    {
        private readonly StringBuilder builder = new StringBuilder ( );

        public void MoveTo           ( bool relative, float x, float y ) => Append ( 'M', relative, x, y );
        public void LineTo           ( bool relative, float x, float y ) => Append ( 'L', relative, x, y );
        public void HorizontalLineTo ( bool relative, float x          ) => Append ( 'H', relative, x );
        public void VerticalLineTo   ( bool relative,          float y ) => Append ( 'V', relative, y );

        public void CurveTo       ( bool relative, float x1, float y1, float x2, float y2, float x, float y ) => Append ( 'C', relative, x1, y1, x2, y2, x, y );
        public void SmoothCurveTo ( bool relative,                     float x2, float y2, float x, float y ) => Append ( 'S', relative, x2, y2, x, y );
        public void CurveTo       ( bool relative, float x1, float y1,                     float x, float y ) => Append ( 'Q', relative, x1, y1, x, y );
        public void SmoothCurveTo ( bool relative,                                         float x, float y ) => Append ( 'T', relative, x, y );

        public void Arc ( bool relative, float rx, float ry, float angle, bool largeArc, bool sweep, float x, float y )
        {
            Append ( 'A', relative, rx, ry, angle, largeArc ? 1 : 0, sweep ? 1 : 0, x, y );
        }

        public void ClosePath ( ) => Append ( 'Z', lastRelative );

        public          void   Clear    ( ) => builder.Clear    ( );
        public override string ToString ( ) => builder.ToString ( );

        private char lastCommand;
        private bool lastRelative;

        private void Append ( char command, bool relative, params float [ ] arguments )
        {
            command |= ( lastRelative = relative ) ? Relative : Absolute;
            if ( command == lastCommand ) command     = NoCommand;
            else                          lastCommand = command;

            if ( command is not NoCommand )
                builder.Append ( command );

            foreach ( var argument in arguments )
            {
                if ( builder.Length == 0 || argument is >= 1 && IsDigit ( builder [ ^1 ] ) )
                    builder.Append ( ' ' );

                var start = builder.Length;
                builder.Append ( argument );
                if ( builder [ start ] is '0' )
                    builder.Remove ( start, 1 );
            }
        }
    }
}