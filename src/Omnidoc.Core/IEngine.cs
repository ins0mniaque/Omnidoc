using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Omnidoc.Core;
using Omnidoc.IO;

namespace Omnidoc
{
    public interface IEngine
    {
        IEnumerable < IService > Services { get; }

        IEnumerable < T > FindServices < T > ( FileFormat format ) where T : IService
        {
            return Services.OfType < T > ( ).Where ( service => service.Descriptor.Supports ( format ) );
        }

        IEnumerable < T > FindServices < T > ( FileFormat inputFormat, FileFormat outputFormat ) where T : IService
        {
            return Services.OfType < T > ( ).Where ( service => service.Descriptor.Supports ( inputFormat  ) &&
                                                                service.Descriptor.Outputs  ( outputFormat ) );
        }

        T? FindService < T > ( FileFormat format ) where T : IService
        {
            return FindServices < T > ( format ).FirstOrDefault ( );
        }

        T? FindService < T > ( FileFormat inputFormat, FileFormat outputFormat ) where T : IService
        {
            return FindServices < T > ( inputFormat, outputFormat ).FirstOrDefault ( );
        }

        async Task < FileFormat? > DetectFileFormatAsync ( Stream file, CancellationToken cancellationToken )
        {
            foreach ( var detector in Services.OfType < IFileFormatDetector > ( ).OrderBy ( Depth ) )
                if ( await detector.DetectAsync ( file ).ConfigureAwait ( false ) is FileFormat format )
                    return format;

            return null;
        }

        private static int Depth ( IFileFormatDetector detector )
        {
            return detector.Descriptor.Formats.Max ( Depth );
        }

        private static int Depth ( FileFormat format )
        {
            var depth = 0;

            while ( format.Base is FileFormat basedOn )
            {
                format = basedOn;
                depth--;
            }

            return depth;
        }
    }
}