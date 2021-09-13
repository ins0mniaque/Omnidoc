using System;
using System.Collections.Generic;
using System.IO;

using Omnidoc.IO;
using Omnidoc.Services;

namespace Omnidoc.Xps
{
    public class XpsDocumentTypeReader : IDocumentTypeReader
    {
        public static IReadOnlyCollection < byte [ ] > MagicNumbers { get; } = new [ ] { MagicNumber.From ( (byte) 'P', (byte) 'K', 0x03, 0x04 ) };

        public IReadOnlyCollection < DocumentType > Types { get; } = new [ ] { DocumentTypes.Xps, DocumentTypes.Oxps };

        public DocumentType? ReadDocumentType ( Stream stream )
        {
            if ( stream is null )
                throw new ArgumentNullException ( nameof ( stream ) );

            // TODO: Detect non-xps zip files and oxps files
            return stream.Match ( MagicNumbers ) switch
            {
                0 => DocumentTypes.Xps,
                _ => null
            };
        }
    }
}