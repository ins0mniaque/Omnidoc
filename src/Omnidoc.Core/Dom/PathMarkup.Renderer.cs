using System;

namespace Omnidoc.Dom
{
    public partial class PathMarkup
    {
        private const char Absolute  = '\0';
        private const char Relative  = ' ';
        private const char NoCommand = '\0';

        public static void Render ( ReadOnlySpan < char > markup, Rendering.IGeometryRenderer renderer )
        {
            if ( markup == null )
                throw new ArgumentNullException ( nameof ( markup ) );

            var length = TryRender ( markup, renderer );
            if ( length != markup.Length )
                throw new FormatException ( $"Invalid path command '{ markup [ length - 1 ] }'" );
        }

        public static int TryRender ( ReadOnlySpan < char > markup, Rendering.IGeometryRenderer renderer )
        {
            var position  = 0;
            var command   = NoCommand;
            var arguments = new float [ 7 ];

            while ( SkipWhitespace ( markup, ref position ) )
            {
                var read     = markup [ position++ ];
                var argCount = GetCommandArgumentCount ( read );
                if ( argCount < 0 )
                {
                    if ( ! IsNumeric ( read ) )
                        return position;

                    if ( command is 'M' or 'm' )
                        command--;
                }
                else
                    command = read;

                for ( var index = 0; index < argCount; index++ )
                {
                    SkipWhitespace ( markup, ref position );
                    if ( ! TryReadNumber ( markup, ref position, out var number ) )
                        return position;

                    arguments [ index ] = number;
                }

                RenderCommand ( renderer, command, arguments );
            }

            return position;
        }

        private static int GetCommandArgumentCount ( char command ) => command switch
        {
            'Z' or 'z'                             =>  0,
            'H' or 'h' or 'V' or 'v'               =>  1,
            'M' or 'm' or 'L' or 'l' or 'T' or 't' =>  2,
            'S' or 's' or 'Q' or 'q'               =>  4,
            'C' or 'c'                             =>  6,
            'A' or 'a'                             =>  7,
            _                                      => -1
        };

        private static void RenderCommand ( Rendering.IGeometryRenderer renderer, char command, params float [ ] args )
        {
            var relative = ( command & Relative ) != 0;

            switch ( command )
            {
                case 'M' or 'm' : renderer.MoveTo           ( relative, args [ 0 ], args [ 1 ] ); break;
                case 'L' or 'l' : renderer.LineTo           ( relative, args [ 0 ], args [ 1 ] ); break;
                case 'H' or 'h' : renderer.HorizontalLineTo ( relative, args [ 0 ]             ); break;
                case 'V' or 'v' : renderer.VerticalLineTo   ( relative, args [ 0 ]             ); break;
                case 'C' or 'c' : renderer.CurveTo          ( relative, args [ 0 ], args [ 1 ], args [ 2 ], args [ 3 ], args [ 4 ], args [ 5 ] ); break;
                case 'S' or 's' : renderer.SmoothCurveTo    ( relative, args [ 0 ], args [ 1 ], args [ 2 ], args [ 3 ]                         ); break;
                case 'Q' or 'q' : renderer.CurveTo          ( relative, args [ 0 ], args [ 1 ], args [ 2 ], args [ 3 ]                         ); break;
                case 'T' or 't' : renderer.SmoothCurveTo    ( relative, args [ 0 ], args [ 1 ]                                                 ); break;
                case 'A' or 'a' : renderer.Arc              ( relative, args [ 0 ], args [ 1 ], args [ 2 ], (int) args [ 3 ] == 1, (int) args [ 4 ] == 1, args [ 5 ], args [ 6 ] ); break;
                case 'Z' or 'z' : renderer.ClosePath        ( ); break;
                default  : throw new InvalidOperationException ( $"Invalid path command '{ command }'" );
            }
        }

        private static bool IsDigit   ( char c ) => c is '0' or '1' or '2' or '3' or '4' or '5' or '6' or '7' or '8' or '9';
        private static bool IsNumeric ( char c ) => c is '0' or '1' or '2' or '3' or '4' or '5' or '6' or '7' or '8' or '9' or '.' or '-' or '+' or 'e' or 'E';

        private static bool TryReadNumber ( ReadOnlySpan < char > markup, ref int position, out float number )
        {
            var start = position;
            var read  = NoCommand;
            var dot   = false;

            while ( position < markup.Length && IsNumeric ( read = markup [ position ] ) )
            {
                if ( read is '-' or '+' && position != start || read is '.' && dot )
                    if ( markup [ position - 1 ] is not 'e' and not 'E' )
                        break;

                dot |= read is '.';
                position++;
            }

            return float.TryParse ( markup.Slice ( start, position - start ), out number );
        }

        private static bool SkipWhitespace ( ReadOnlySpan < char > markup, ref int position )
        {
            while ( position < markup.Length && ( char.IsWhiteSpace ( markup [ position ] ) || markup [ position ] is ',' ) )
                position++;

            return position < markup.Length;
        }
    }
}