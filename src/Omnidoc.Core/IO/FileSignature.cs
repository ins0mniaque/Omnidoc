using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Omnidoc.IO
{
    [ SuppressMessage ( "Performance", "CA1819:Properties should not return arrays", Justification = "ReadOnlySpan" ) ]
    public sealed class FileSignature
    {
        private static byte [ ] Cast ( string signature ) => signature.Select ( character => (byte) character ).ToArray ( );

        public FileSignature (               string          signature ) : this ( 0,      signature          ) { }
        public FileSignature (               params byte [ ] signature ) : this ( 0,      signature          ) { }
        public FileSignature ( Index offset, string          signature ) : this ( offset, Cast ( signature ) ) { }
        public FileSignature ( Index offset, params byte [ ] signature )
        {
            if ( signature is null     ) throw new ArgumentNullException ( nameof ( signature ) );
            if ( signature.Length == 0 ) throw new ArgumentException     ( Strings.Error_EmptyFileSignature, nameof ( signature ) );

            Signature = signature;
            Offset    = offset;
        }

        public byte [ ] Signature { get; }
        public Index    Offset    { get; }
        public int      Length    => Signature.Length;

        public bool Matches ( ReadOnlySpan < byte > signature )
        {
            return Length == signature.Length && Signature.AsSpan ( ).SequenceEqual ( signature );
        }
    }
}