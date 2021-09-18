using Xunit;

namespace Omnidoc.Tests
{
    public class EngineTests
    {
        [ Fact ]
        public void ResolvesServices ( )
        {
            var engine = new Engine ( );

            Assert.NotEmpty ( engine.Services );
        }
    }
}