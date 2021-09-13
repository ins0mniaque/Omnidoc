using System;

namespace Omnidoc.Content
{
    public class DocumentImage : DocumentContent
    {
        public DocumentImage ( Uri uri )
        {
            Uri = uri;
        }

        public Uri Uri { get; }
    }
}