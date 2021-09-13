using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Omnidoc.IO
{
    public static class MagicNumber
    {
        public static byte [ ] From ( params byte [ ] magicNumber ) => magicNumber;
        public static byte [ ] From ( string          magicNumber ) => magicNumber.Cast < byte > ( ).ToArray ( );

        public static int Match ( this Stream stream, IEnumerable < byte [ ] > magicNumbers )
        {
            return Match ( stream, magicNumbers.ToArray ( ) );
        }

        public static int Match ( this Stream stream, int offset, IEnumerable < byte [ ] > magicNumbers )
        {
            return Match ( stream, offset, magicNumbers.ToArray ( ) );
        }

        public static int Match ( this Stream stream, params byte [ ] [ ] magicNumbers )
        {
            return Match ( stream, 0, magicNumbers );
        }

        public static int Match ( this Stream stream, int offset, params byte [ ] [ ] magicNumbers )
        {
            if ( stream is null )
                throw new ArgumentNullException ( nameof ( stream ) );

            var length = magicNumbers.Max ( magicNumber => magicNumber.Length );
            var header = new byte [ length ];

            length = stream.Read ( header, offset, length );

            for ( var index = 0; index < magicNumbers.Length; index++ )
            {
                var magicNumber = magicNumbers [ index ];
                if ( magicNumber.Length <= length && magicNumber.SequenceEqual ( header.Take ( magicNumber.Length ) ) )
                    return index;
            }

            return -1;
        }
    }
}