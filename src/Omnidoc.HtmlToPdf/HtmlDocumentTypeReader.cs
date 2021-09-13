using System;
using System.Collections.Generic;
using System.IO;

using Omnidoc.IO;
using Omnidoc.Services;

namespace Omnidoc.HtmlToPdf
{
    public class HtmlToPdfDocumentTypeReader : IDocumentTypeReader
    {
        public static IReadOnlyCollection < byte [ ] > MagicNumbers { get; } = new [ ] { MagicNumber.From ( "<" ), MagicNumber.From ( "%PDF" ) };

        public IReadOnlyCollection < DocumentType > Types { get; } = new [ ] { DocumentTypes.Html, DocumentTypes.Pdf };

        public DocumentType? ReadDocumentType ( Stream stream )
        {
            if ( stream is null )
                throw new ArgumentNullException ( nameof ( stream ) );

            // TODO: Improve HTML detection
            return stream.Match ( MagicNumbers ) switch
            {
                0 => DocumentTypes.Html,
                1 => DocumentTypes.Pdf,
                _ => null
            };
        }
    }
}