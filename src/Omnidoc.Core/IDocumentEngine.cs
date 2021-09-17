using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Omnidoc.Services;

namespace Omnidoc
{
    public interface IDocumentEngine
    {
        IDocumentServiceProvider Services { get; }

        async Task < DocumentType? > DetectTypeAsync ( Stream stream, CancellationToken cancellationToken )
        {
            foreach ( var detector in Services.GetServices ( ).OfType < IDocumentTypeDetector > ( ) )
                if ( await detector.DetectTypeAsync ( stream ).ConfigureAwait ( false ) is DocumentType type )
                    return type;

            return null;
        }
    }
}