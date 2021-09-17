using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Omnidoc.Core
{
    public interface IFileFormatConverter : IService
    {
        Task ConvertAsync ( Stream file, Stream output, OutputOptions options, CancellationToken cancellationToken = default );
    }
}