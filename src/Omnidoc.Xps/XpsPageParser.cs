using System.Collections.Generic;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Xml.Linq;

using Omnidoc.Model;
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

        public async IAsyncEnumerable < Content > ParseAsync ( [ EnumeratorCancellation ] CancellationToken cancellationToken = default )
        {
            var page = await XDocument.LoadAsync      ( Page.Open ( ), LoadOptions.None, cancellationToken )
                                      .ConfigureAwait ( false );

            foreach ( var element in page.Descendants ( ) )
            {
                if ( element.Name.LocalName == "Glyphs" && element.HasAttributes )
                {
                    yield return new Glyphs ( element.Attribute ( "UnicodeString" ).Value )
                    {
                        Position = double.TryParse ( element.Attribute ( "OriginX" ).Value, out var left ) &&
                                   double.TryParse ( element.Attribute ( "OriginY" ).Value, out var top  ) ? new Point ( left, top ) : null,
                        Fill     = element.Attribute ( "Fill" ).Value,
                        Font     = new Font { Name = element.Attribute ( "FontUri" ).Value,
                                              Size = double.TryParse ( element.Attribute ( "FontRenderingEmSize" ).Value, out var fontSize ) ? fontSize : null }
                    };
                }
            }
        }

        public void Dispose ( )
        {

        }
    }
}