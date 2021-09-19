using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Omnidoc.IO;

namespace Omnidoc
{
    public interface IEngine
    {
        IEnumerable < IService > Services { get; }

        Task < FileFormat? > DetectFileFormatAsync ( Stream file, CancellationToken cancellationToken = default );

        T? FindService < T > ( FileFormat format )                               where T : IService;
        T? FindService < T > ( FileFormat inputFormat, FileFormat outputFormat ) where T : IService;

        IEnumerable < T > FindServices < T > ( FileFormat format )                               where T : IService;
        IEnumerable < T > FindServices < T > ( FileFormat inputFormat, FileFormat outputFormat ) where T : IService;
    }
}