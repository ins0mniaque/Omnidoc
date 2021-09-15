using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Net;

namespace Omnidoc
{
    public static class DataUri
    {
        public  const string Scheme             = "data";
        private const string DefaultContentType = "application/octet-stream";

        public static bool RegisterHandler ( )               => RegisterHandler ( Scheme );
        public static bool RegisterHandler ( string prefix ) => WebRequest.RegisterPrefix ( prefix, new RequestFactory ( ) );

        public static Uri Generate ( byte [ ] content )
        {
            return Generate ( content, null );
        }

        public static Uri Generate ( byte [ ] content, string? contentType )
        {
            return new Uri ( $"{ Scheme }:{ contentType ?? DefaultContentType };base64,{ Convert.ToBase64String ( content ) }" );
        }

        public static void Parse ( Uri uri, [ NotNull ] out string? contentType )
        {
            Parse ( uri, false, out _, out contentType );
        }

        public static void Parse ( Uri uri, [ NotNull ] out byte [ ]? content )
        {
            Parse ( uri, true, out content, out _ );
        }

        public static void Parse ( Uri uri, [ NotNull ] out byte [ ]? content, [ NotNull ] out string? contentType )
        {
            Parse ( uri, true, out content, out contentType );
        }

        private static void Parse ( Uri uri, bool decodeData, [ NotNull ] out byte [ ]? content, [ NotNull ] out string? contentType )
        {
            if ( ! TryParse ( uri, decodeData, out content, out contentType ) )
                throw new FormatException ( Strings.Error_InvalidDataUri );
        }

        public static bool TryParse ( Uri uri, [ NotNullWhen ( true ) ] out string? contentType )
        {
            return TryParse ( uri, false, out _, out contentType );
        }

        public static bool TryParse ( Uri uri, [ NotNullWhen ( true ) ] out byte [ ]? content )
        {
            return TryParse ( uri, true, out content, out _ );
        }

        public static bool TryParse ( Uri uri, [ NotNullWhen ( true ) ] out byte [ ]? content, [ NotNullWhen ( true ) ] out string? contentType )
        {
            return TryParse ( uri, true, out content, out contentType );
        }

        private static bool TryParse ( Uri uri, bool decodeData, [ NotNullWhen ( true ) ] out byte [ ]? content, [ NotNullWhen ( true ) ] out string? contentType )
        {
            if ( uri is null )
                throw new ArgumentNullException ( nameof ( uri ) );

            content     = null;
            contentType = null;

            if ( uri.Scheme != Scheme )
                return false;

            var absoluteUri = uri.AbsoluteUri;

            var comma = absoluteUri.IndexOf ( ',', StringComparison.InvariantCulture );
            if ( comma < 0 )
                return false;

            var parameters = absoluteUri.Substring ( 0, comma  ).Split ( ';' );
            var encoded    = absoluteUri.Substring ( comma + 1 );

            var encoding = Encoding.ASCII;
            var decode   = (Func < string, byte [ ] >) ( encoded => UrlDecode ( encoded, encoding ) );
            foreach ( var parameter in parameters.Skip ( 1 ) )
            {
                if ( parameter == "base64" )
                    decode = Convert.FromBase64String;
                else if ( parameter.StartsWith ( "charset=", StringComparison.Ordinal ) )
                    encoding = Encoding.GetEncoding ( parameter.Substring ( "charset=".Length ) );
            }

            content     = decodeData ? decode ( encoded ) : Array.Empty < byte > ( );
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

        private sealed class RequestFactory : IWebRequestCreate
        {
            public WebRequest Create ( Uri uri ) => new Request ( uri );
        }

        private sealed class Request : WebRequest
        {
            private readonly Uri uri;

            public Request ( Uri uri ) { this.uri = uri; }

            public override WebResponse GetResponse ( ) => new Response ( uri );
        }

        private sealed class Response : WebResponse
        {
            private readonly string   contentType;
            private readonly byte [ ] content;

            public Response ( Uri uri )
            {
                Parse ( uri, out content, out contentType );
            }

            public override string ContentType
            {
                get => contentType;
                set => throw new NotSupportedException ( );
            }

            public override long ContentLength
            {
                get => content.Length;
                set => throw new NotSupportedException ( );
            }

            public override Stream GetResponseStream ( ) => new MemoryStream ( content, false );
        }
    }
}