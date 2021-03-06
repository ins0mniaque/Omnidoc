using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Omnidoc.IO;

namespace Omnidoc.Core
{
    public interface IFileMetadataReader : IService
    {
        Task < FileMetadata? > TryReadAsync ( Stream input, CancellationToken cancellationToken = default );
    }
}