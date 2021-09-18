using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;

using Omnidoc.Core;
using Omnidoc.Services;

namespace Omnidoc.Zip.Opc
{
    public class OpcDocumentPreviewer : IDocumentPreviewer
    {
        private static readonly IServiceDescriptor descriptor = new ServiceDescriptor
        (
            new [ ] { FileFormats.Opc }
        );

        public IServiceDescriptor Descriptor => descriptor;

        public async Task PreviewAsync ( Stream document, Stream output, RenderingOptions options, CancellationToken cancellationToken = default )
        {
            using var archive = new ZipArchive ( document );

            var thumbnail = await archive.TryOpenThumbnailAsync ( cancellationToken ).ConfigureAwait ( false );
            if ( thumbnail != null )
                await thumbnail.CopyToAsync ( output, cancellationToken ).ConfigureAwait ( false );
        }
    }
}