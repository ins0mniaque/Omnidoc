using System;
using System.IO;
using System.Net;

namespace Omnidoc.Net
{
    public sealed class DataWebResponse : WebResponse
    {
        private readonly string   contentType;
        private readonly byte [ ] data;

        public DataWebResponse ( Uri uri )
        {
            DataUri.Parse ( uri, out data, out contentType );
        }

        public override string ContentType
        {
            get => contentType;
            set => throw new NotSupportedException ( );
        }

        public override long ContentLength
        {
            get => data.Length;
            set => throw new NotSupportedException ( );
        }

        public override Stream GetResponseStream ( ) => new MemoryStream ( data );
    }
}