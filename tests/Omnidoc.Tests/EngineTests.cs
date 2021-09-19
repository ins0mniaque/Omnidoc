using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Xunit;

namespace Omnidoc.Tests
{
    [ SuppressMessage ( "Reliability", "CA2000:Dispose objects before losing scope", Justification = "False positive on await using with ConfigureAwait" ) ]
    public class EngineTests
    {
        [ Fact ]
        public async Task ResolvesServices ( )
        {
            var engine = new Engine ( );

            await using ( engine.ConfigureAwait ( false ) )
            {
                Assert.NotEmpty ( engine.Services );
            }
        }
    }
}