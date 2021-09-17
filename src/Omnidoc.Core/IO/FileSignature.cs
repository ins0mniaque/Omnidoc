using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Omnidoc.IO
{
    [ SuppressMessage ( "Performance", "CA1819:Properties should not return arrays", Justification = "ReadOnlySpan" ) ]
    public sealed class FileSignature
    {
        private static byte [ ] Cast ( string signature ) => signature.Cast < byte > ( ).ToArray ( );

        public FileSignature (              string          signature ) : this ( 0,     signature          ) { }
        public FileSignature (              params byte [ ] signature ) : this ( 0,     signature          ) { }
        public FileSignature ( Index index, string          signature ) : this ( index, Cast ( signature ) ) { }
        public FileSignature ( Index index, params byte [ ] signature )
        {
            if ( signature is null     ) throw new ArgumentNullException ( nameof ( signature ) );
            if ( signature.Length == 0 ) throw new ArgumentException     ( Strings.Error_EmptyFileSignature, nameof ( signature ) );

            Signature = signature;
            Index     = index;
        }

        public byte [ ] Signature { get; }
        public Index    Index     { get; }
        public int      Length    => Signature.Length;

        public bool Matches ( ReadOnlySpan < byte > signature )
        {
            return Length == signature.Length && Signature.AsSpan ( ).SequenceEqual ( signature );
        }
    }
}