using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;

using Omnidoc.Core;
using Omnidoc.Core.Disposables;
using Omnidoc.Services;

namespace Omnidoc.Zip.Opc
{
    public class OpcDocumentPreviewer : AsyncDisposable, IDocumentPreviewer
    {
        private static readonly IServiceDescriptor descriptor = new ServiceDescriptor
        (
            new [ ] { FileFormats.Opc }
        );

        public IServiceDescriptor Descriptor => descriptor;

        public async Task < bool > TryPreviewAsync ( Stream input, Stream output, RenderingOptions options, CancellationToken cancellationToken = default )
        {
            using var archive = new ZipArchive ( input );

            var thumbnail = await archive.TryOpenThumbnailAsync ( cancellationToken ).ConfigureAwait ( false );
            if ( thumbnail is null )
                return false;

            await thumbnail.CopyToAsync ( output, cancellationToken ).ConfigureAwait ( false );

            return true;
        }
    }
}