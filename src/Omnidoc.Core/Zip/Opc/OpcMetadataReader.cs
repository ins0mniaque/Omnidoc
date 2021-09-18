using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;

using Omnidoc.Core;
using Omnidoc.IO;
using Omnidoc.Services;

namespace Omnidoc.Zip.Opc
{
    public class OpcMetadataReader : IFileMetadataReader
    {
        private static readonly IServiceDescriptor descriptor = new ServiceDescriptor
        (
            new [ ] { FileFormats.Opc }
        );

        public IServiceDescriptor Descriptor => descriptor;

        public async Task < FileMetadata? > TryReadAsync ( Stream file, CancellationToken cancellationToken = default )
        {
            using var archive = new ZipArchive ( file );

            return await archive.TryReadMetadataAsync ( cancellationToken ).ConfigureAwait ( false );
        }
    }
}