using System;
using System.IO;
using System.Linq;

using Omnidoc.IO;
using Omnidoc.Services;

namespace Omnidoc
{
    public interface IDocumentEngine
    {
        IDocumentServiceProvider Services { get; }

        Stream PrepareStream ( Stream stream )
        {
            if ( stream is null )
                throw new ArgumentNullException ( nameof ( stream ) );

            return stream.CanSeek ? stream : new SeekableReadOnlyStream ( stream );
        }

        DocumentType? GetDocumentType ( Stream stream )
        {
            return Services.GetServices ( )
                           .OfType < IDocumentTypeReader > ( )
                           .Select         ( reader      => reader.ReadDocumentTypeAndSeekBack ( stream ) )
                           .FirstOrDefault ( contentType => contentType != null );
        }
    }
}