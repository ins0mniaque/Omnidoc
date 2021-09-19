using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;

using Xunit;

namespace Omnidoc.Xps.Tests
{
    [ SuppressMessage ( "Reliability", "CA2000:Dispose objects before losing scope", Justification = "False positive on await using with ConfigureAwait" ) ]
    public class XpsTests
    {
        [ Theory ]
        [ Samples ( "Xps/" ) ]
        public async Task DetectsXpsFiles ( Stream sample )
        {
            var detector = new XpsFormatDetector ( );

            await using ( detector.ConfigureAwait ( false ) )
            {
                var format = await detector.DetectAsync ( sample ).ConfigureAwait ( false );

                Assert.Equal ( FileFormats.Xps, format );
            }
        }

        [ Theory ]
        [ Samples ( "Zip/" ) ]
        public async Task DoesNotDetectZipFilesAsXpsFiles ( Stream sample )
        {
            var detector = new XpsFormatDetector ( );

            await using ( detector.ConfigureAwait ( false ) )
            {
                var format = await detector.DetectAsync ( sample ).ConfigureAwait ( false );

                Assert.Null ( format );
            }
        }
    }
}