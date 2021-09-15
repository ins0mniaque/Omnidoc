using System.Collections.Generic;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Xml.Linq;

using Omnidoc.Content;
using Omnidoc.Services;

namespace Omnidoc.Xps
{
    public sealed class XpsPageParser : IPageParser
    {
        public XpsPageParser ( ZipArchiveEntry page )
        {
            Page = page;
        }

        public ZipArchiveEntry Page { get; }

        public async IAsyncEnumerable < DocumentContent > ParseAsync ( [ EnumeratorCancellation ] CancellationToken cancellationToken = default )
        {
            var page = await XDocument.LoadAsync      ( Page.Open ( ), LoadOptions.None, cancellationToken )
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

        public void Dispose ( )
        {

        }
    }
}