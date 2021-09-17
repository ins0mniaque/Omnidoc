using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Omnidoc.IO
{
    public static class FileSignatureMatcher
    {
        public static int Match ( this Stream stream, IEnumerable < FileSignature > signatures )
        {
            if ( stream     is null ) throw new ArgumentNullException ( nameof ( stream     ) );
            if ( signatures is null ) throw new ArgumentNullException ( nameof ( signatures ) );

            return Match ( stream, signatures.ToArray ( ) );
        }

        public static int Match ( this Stream stream, params FileSignature [ ] signatures )
        {
            if ( stream     is null ) throw new ArgumentNullException ( nameof ( stream     ) );
            if ( signatures is null ) throw new ArgumentNullException ( nameof ( signatures ) );

            if ( signatures.Length == 0 )
                return -1;

            if ( ! stream.CanSeek )
                throw new NotSupportedException ( string.Format ( CultureInfo.InvariantCulture, Strings.Error_StreamMustBeSeekable, $"{ nameof ( Seekable ) }.{ nameof ( Seekable.AsSeekable ) }" ) );

            Split ( signatures, out var prefixes, out var suffixes );

            prefixes.Range ( out var offset, out var length );

            var buffer = new byte [ length ];

            stream.Seek ( offset, SeekOrigin.Begin );

            length = stream.Read ( buffer, offset, length );

            if ( buffer.Match ( offset, signatures ) is FileSignature prefixMatch )
                return Array.IndexOf ( signatures, prefixMatch );

            suffixes.Range ( out offset, out length );

            buffer = new byte [ length ];

            stream.Seek ( offset, SeekOrigin.End );

            length = stream.Read ( buffer, offset, length );

            if ( buffer.Match ( offset, signatures ) is FileSignature suffixMatch )
                return Array.IndexOf ( signatures, suffixMatch );

            return -1;
        }

        public static Task < int > MatchAsync ( this Stream stream, IEnumerable < FileSignature > signatures, CancellationToken cancellationToken = default )
        {
            if ( stream     is null ) throw new ArgumentNullException ( nameof ( stream     ) );
            if ( signatures is null ) throw new ArgumentNullException ( nameof ( signatures ) );

            return MatchAsync ( stream, signatures.ToArray ( ), cancellationToken );
        }

        public static async Task < int > MatchAsync ( this Stream stream, FileSignature [ ] signatures, CancellationToken cancellationToken = default )
        {
            if ( stream     is null ) throw new ArgumentNullException ( nameof ( stream     ) );
            if ( signatures is null ) throw new ArgumentNullException ( nameof ( signatures ) );

            if ( signatures.Length == 0 )
                return -1;

            if ( ! stream.CanSeek )
                throw new NotSupportedException ( string.Format ( CultureInfo.InvariantCulture, Strings.Error_StreamMustBeSeekable, $"{ nameof ( Seekable ) }.{ nameof ( Seekable.AsSeekable ) }" ) );

            Split ( signatures, out var prefixes, out var suffixes );

            prefixes.Range ( out var offset, out var length );

            var buffer = new byte [ length ];

            stream.Seek ( offset, SeekOrigin.Begin );

            length = await stream.ReadAsync      ( buffer, offset, length, cancellationToken )
                                 .ConfigureAwait ( false );

            if ( buffer.Match ( offset, signatures ) is FileSignature prefixMatch )
                return Array.IndexOf ( signatures, prefixMatch );

            suffixes.Range ( out offset, out length );

            buffer = new byte [ length ];

            stream.Seek ( offset, SeekOrigin.End );

            length = await stream.ReadAsync      ( buffer, offset, length, cancellationToken )
                                 .ConfigureAwait ( false );

            if ( buffer.Match ( offset, signatures ) is FileSignature suffixMatch )
                return Array.IndexOf ( signatures, suffixMatch );

            return -1;
        }

        private static void Split ( this FileSignature [ ] signatures, out FileSignature [ ] prefixes, out FileSignature [ ] suffixes )
        {
            prefixes = signatures.Where ( signature => ! signature.Offset.IsFromEnd ).OrderByDescending ( prefix => prefix.Length ).ToArray ( );
            suffixes = signatures.Where ( signature =>   signature.Offset.IsFromEnd ).OrderByDescending ( suffix => suffix.Length ).ToArray ( );
        }

        private static void Range ( this FileSignature [ ] signatures, out int offset, out int length )
        {
            offset = signatures.Min ( signature => signature.Offset.Value );
            length = signatures.Max ( signature => signature.Offset.Value + signature.Length );
        }

        private static FileSignature? Match ( this byte [ ] buffer, int offset, FileSignature [ ] signatures )
        {
            var span = buffer.AsSpan ( );

            foreach ( var signature in signatures )
                if ( signature.Matches ( span.Slice ( signature.Offset.GetOffset ( span.Length ) - offset, signature.Length ) ) )
                    return signature;

            return null;
        }
    }
}