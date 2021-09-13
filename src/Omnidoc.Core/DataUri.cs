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
        public const string Scheme = "data";

        public static void Enable ( )
        {
            WebRequest.RegisterPrefix ( Scheme, new DataWebRequestFactory ( ) );
        }

        public static void Parse ( Uri uri, [ NotNull ] out byte [ ]? data )
        {
            Parse ( uri, out data, out _ );
        }

        public static void Parse ( Uri uri, [ NotNull ] out byte [ ]? data, [ NotNull ] out string? contentType )
        {
            if ( ! TryParse ( uri, out data, out contentType ) )
                throw new FormatException ( Strings.Error_InvalidDataUri );
        }

        public static bool TryParse ( Uri uri, [ NotNullWhen ( true ) ] out byte [ ]? data )
        {
            return TryParse ( uri, out data, out _ );
        }

        public static bool TryParse ( Uri uri, [ NotNullWhen ( true ) ] out byte [ ]? data, [ NotNullWhen ( true ) ] out string? contentType )
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

            data        = decode ( content );
            contentType = parameters [ 0 ];
            if ( string.IsNullOrEmpty ( contentType ) )
                contentType = "text/plain";

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