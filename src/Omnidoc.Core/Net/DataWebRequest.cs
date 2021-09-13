using System;
using System.Net;

namespace Omnidoc.Net
{
    public sealed class DataWebRequest : WebRequest
    {
        private readonly Uri uri;

        public DataWebRequest ( Uri uri ) { this.uri = uri; }

        public override WebResponse GetResponse ( ) => new DataWebResponse ( uri );
    }
}