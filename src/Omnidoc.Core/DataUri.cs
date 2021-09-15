using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Net;

using Omnidoc.Net;

namespace Omnidoc
{
    public static class DataUri
    {
        public  const string Scheme             = "data";
        private const string DefaultContentType = "application/octet-stream";

        public static void Enable ( )
        {
            WebRequest.RegisterPrefix ( Scheme, new DataWebRequestFactory ( ) );
        }

        public static Uri Generate ( byte [ ] data )
        {
            return Generate ( data, null );
        }

        public static Uri Generate ( byte [ ] data, string? contentType )
        {
            return new Uri ( $"{ Scheme }:{ contentType ?? DefaultContentType };base64,{ Convert.ToBase64String ( data ) }" );
        }

        public static void Parse ( Uri uri, [ NotNull ] out string? contentType )
        {
            Parse ( uri, false, out _, out contentType );
        }

        public static void Parse ( Uri uri, [ NotNull ] out byte [ ]? data )
        {
            Parse ( uri, true, out data, out _ );
        }

        public static void Parse ( Uri uri, [ NotNull ] out byte [ ]? data, [ NotNull ] out string? contentType )
        {
            Parse ( uri, true, out data, out contentType );
        }

        private static void Parse ( Uri uri, bool decodeData, [ NotNull ] out byte [ ]? data, [ NotNull ] out string? contentType )
        {
            if ( ! TryParse ( uri, decodeData, out data, out contentType ) )
                throw new FormatException ( Strings.Error_InvalidDataUri );
        }

        public static bool TryParse ( Uri uri, [ NotNullWhen ( true ) ] out string? contentType )
        {
            return TryParse ( uri, false, out _, out contentType );
        }

        public static bool TryParse ( Uri uri, [ NotNullWhen ( true ) ] out byte [ ]? data )
        {
            return TryParse ( uri, true, out data, out _ );
        }

        public static bool TryParse ( Uri uri, [ NotNullWhen ( true ) ] out byte [ ]? data, [ NotNullWhen ( true ) ] out string? contentType )
        {
            return TryParse ( uri, true, out data, out contentType );
        }

        private static bool TryParse ( Uri uri, bool decodeData, [ NotNullWhen ( true ) ] out byte [ ]? data, [ NotNullWhen ( true ) ] out string? contentType )
        {
            if ( uri is null )
                throw new ArgumentNullException ( nameof ( uri ) );

            data        = null;
            contentType = null;

            if ( uri.Scheme != Scheme )
                return false;

            var absoluteUri = uri.AbsoluteUri;

            var comma = absoluteUri.IndexOf ( ',', StringComparison.InvariantCulture );
            if ( comma < 0 )
                return false;

            var parameters  = absoluteUri.Substring ( 0, comma  ).Split ( ';' );
            var content     = absoluteUri.Substring ( comma + 1 );

            var encoding = Encoding.ASCII;
            var decode   = (Func < string, byte [ ] >) ( content => UrlDecode ( content, encoding ) );
            foreach ( var parameter in parameters.Skip ( 1 ) )
            {
                if ( parameter == "base64" )
                    decode = Convert.FromBase64String;
                else if ( parameter.StartsWith ( "charset=", StringComparison.InvariantCulture ) )
                    encoding = Encoding.GetEncoding ( parameter.Substring ( "charset=".Length ) );
            }

            data        = decodeData ? decode ( content ) : Array.Empty < byte > ( );
            contentType = parameters [ 0 ];
            if ( string.IsNullOrEmpty ( contentType ) )
                contentType = DefaultContentType;

            return true;
        }

        private static byte [ ] UrlDecode ( string encodedValue, Encoding encoding )
        {
            var bytes = Encoding.ASCII.GetBytes     ( encodedValue );
                bytes = WebUtility.UrlDecodeToBytes ( bytes, 0, bytes.Length );

            return Encoding.Convert ( encoding, Encoding.UTF8, bytes );
        }
    }
}