using System;
using System.Net;

namespace Omnidoc.Net
{
    public sealed class DataWebRequestFactory : IWebRequestCreate
    {
        public WebRequest Create ( Uri uri ) => new DataWebRequest ( uri );
    }
}