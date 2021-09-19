using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Omnidoc.Model.Elements
{
    public class Attachment
    {
        protected const string DefaultContentType = "application/octet-stream";

        public Attachment ( Stream content ) : this ( content, null ) { }
        public Attachment ( Stream content, string? contentType )
        {
            ContentType = contentType ?? DefaultContentType;
            Content     = content;
        }

        public Attachment ( Uri uri ) : this ( uri, null ) { }
        public Attachment ( Uri uri, string? contentType )
        {
            ContentType = contentType ?? ( DataUri.TryParse ( uri, out contentType ) ? contentType : DefaultContentType );
            Uri         = uri;
        }

        public    string? ContentType { get; }
        protected Stream? Content     { get; set; }
        protected Uri?    Uri         { get; set; }

        public virtual async Task < Stream > GetContentAsync ( CancellationToken cancellationToken = default )
        {
            if ( Content != null ) return Content;
            if ( Uri     == null ) throw new InvalidOperationException ( Strings.Error_InvalidAttachment );

            using var web = new WebClient ( );

            return await web.OpenReadTaskAsync ( Uri ).ConfigureAwait ( false );
        }

        public virtual async Task < Uri > GetUriAsync ( CancellationToken cancellationToken = default )
        {
            if ( Uri     != null ) return Uri;
            if ( Content == null ) throw new InvalidOperationException ( Strings.Error_InvalidAttachment );

            using var buffer = new MemoryStream ( );

            await Content.CopyToAsync ( buffer, cancellationToken ).ConfigureAwait ( false );

            return Uri = DataUri.Generate ( buffer.ToArray ( ), ContentType );
        }
    }
}