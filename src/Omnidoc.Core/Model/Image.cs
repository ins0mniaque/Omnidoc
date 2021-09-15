using System;
using System.IO;

namespace Omnidoc.Model
{
    public class Image : Attachment
    {
        public Image ( Stream content )                      : base ( content )              { }
        public Image ( Stream content, string? contentType ) : base ( content, contentType ) { }
        public Image ( Uri uri )                             : base ( uri )                  { }
        public Image ( Uri uri, string? contentType )        : base ( uri, contentType )     { }

        public string? Text { get; set; }
    }
}