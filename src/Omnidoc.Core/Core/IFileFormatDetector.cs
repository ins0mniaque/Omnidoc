using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Omnidoc.IO;

namespace Omnidoc.Core
{
    public interface IFileFormatDetector : IService
    {
        Task < FileFormat? > DetectAsync ( Stream file, CancellationToken cancellationToken = default );
    }
}