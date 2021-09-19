using System.IO;
using System.Threading.Tasks;

using Xunit;

namespace Omnidoc.Xps.Tests
{
    public class XpsTests
    {
        [ Theory ]
        [ Samples ( "Xps/" ) ]
        public async Task DetectsXpsFiles ( Stream sample )
        {
            await using var detector = new XpsFormatDetector ( );

            var format = await detector.DetectAsync ( sample ).ConfigureAwait ( false );

            Assert.Equal ( FileFormats.Xps, format );
        }

        [ Theory ]
        [ Samples ( "Zip/" ) ]
        public async Task DoesNotDetectZipFilesAsXpsFiles ( Stream sample )
        {
            await using var detector = new XpsFormatDetector ( );

            var format = await detector.DetectAsync ( sample ).ConfigureAwait ( false );

            Assert.Null ( format );
        }
    }
}