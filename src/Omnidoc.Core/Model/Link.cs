using System;

namespace Omnidoc.Model
{
    public class Link : Element
    {
        public Link ( Uri uri )
        {
            Uri = uri;
        }

        public Uri Uri { get; }
    }
}