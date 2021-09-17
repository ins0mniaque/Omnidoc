using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Omnidoc.Core;
using Omnidoc.IO;
using Omnidoc.Services;

namespace Omnidoc
{
    public interface IEngine
    {
        IServiceProvider Services { get; }

        async Task < FileFormat? > DetectFileFormatAsync ( Stream file, CancellationToken cancellationToken )
        {
            foreach ( var detector in Services.GetServices ( ).OfType < IFileFormatDetector > ( ) )
                if ( await detector.DetectAsync ( file ).ConfigureAwait ( false ) is FileFormat format )
                    return format;

            return null;
        }
    }
}