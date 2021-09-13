using System;
using System.Collections.Generic;
using System.IO;

using Omnidoc.Services;

namespace Omnidoc.Xps
{
    public class XpsDocumentTypeReader : IDocumentTypeReader
    {
        public IReadOnlyCollection < DocumentType > Types { get; } = new [ ] { DocumentTypes.Xps, DocumentTypes.Oxps };

        public DocumentType? ReadDocumentType ( Stream stream )
        {
            if ( stream is null )
                throw new ArgumentNullException ( nameof ( stream ) );

            var signature = new byte [ 4 ];

            if ( stream.Read ( signature, 0, 4 ) != 4 )
                return null;

            // TODO: Detect non-xps zip files and oxps files
            if ( signature [ 0 ] == 0x50 && signature [ 1 ] == 0x4B && signature [ 2 ] == 0x03 && signature [ 3 ] == 0x04 )
                return DocumentTypes.Xps;

            return null;
        }
    }
}