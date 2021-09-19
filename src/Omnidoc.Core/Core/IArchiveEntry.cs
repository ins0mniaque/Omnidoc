using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Omnidoc.IO;

namespace Omnidoc.Core
{
    public interface IArchiveEntry
    {
        string        Path     { get; }
        FileMetadata? Metadata { get; }

        Task < Stream > ReadAsync ( CancellationToken cancellationToken = default );
    }
}