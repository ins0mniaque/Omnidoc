using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Omnidoc.IO;

namespace Omnidoc.Core
{
    public interface IFileMetadataReader : IService
    {
        Task < FileMetadata > ReadAsync ( Stream file, CancellationToken cancellationToken = default );
    }
}