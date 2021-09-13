using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Xml.Linq;

using Omnidoc.Content;
using Omnidoc.Services;

namespace Omnidoc.Xps
{
    public class XpsDocumentReader : IDocumentReader
    {
        public IReadOnlyCollection < DocumentType > Types { get; } = new [ ] { DocumentTypes.Xps, DocumentTypes.Oxps };

        public async IAsyncEnumerable < DocumentContent > ReadAsync ( Stream stream, [ EnumeratorCancellation ] CancellationToken cancellationToken = default )
        {
            using ( var archive = new ZipArchive ( stream ) )
            {
                foreach ( var entry in archive.Entries )
                {
                    if ( entry.FullName.EndsWith ( ".fpage", StringComparison.OrdinalIgnoreCase ) )
                    {
                        var page = await XDocument.LoadAsync      ( entry.Open ( ), LoadOptions.None, cancellationToken )
                                                  .ConfigureAwait ( false );

                        foreach ( var element in page.Descendants ( ) )
                        {
                            if ( element.Name.LocalName == "Glyphs" && element.HasAttributes )
                            {
                                yield return new DocumentText ( element.Attribute ( "UnicodeString" ).Value )
                                {
                                    Left     = double.TryParse ( element.Attribute ( "OriginX" ).Value, out var left ) ? left : null,
                                    Top      = double.TryParse ( element.Attribute ( "OriginY" ).Value, out var top  ) ? top  : null,
                                    Color    = element.Attribute ( "Fill"    ).Value,
                                    Font     = element.Attribute ( "FontUri" ).Value,
                                    FontSize = double.TryParse ( element.Attribute ( "FontRenderingEmSize" ).Value, out var fontSize ) ? fontSize : null
                                };
                            }
                        }
                    }
                }
            }
        }
    }
}