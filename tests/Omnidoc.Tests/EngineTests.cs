using System.Threading.Tasks;

using Xunit;

namespace Omnidoc.Tests
{
    public class EngineTests
    {
        [ Fact ]
        public async Task ResolvesServices ( )
        {
            await using var engine = new Engine ( );

            Assert.NotEmpty ( engine.Services );
        }
    }
}