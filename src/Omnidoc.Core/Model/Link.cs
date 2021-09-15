using System;

namespace Omnidoc.Model
{
    public class Link : Content
    {
        public Link ( Uri uri )
        {
            Uri = uri;
        }

        public Uri Uri { get; }
    }
}