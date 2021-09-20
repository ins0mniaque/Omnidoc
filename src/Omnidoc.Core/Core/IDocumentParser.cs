namespace Omnidoc.Core
{
    public interface IDocumentParser : IDocumentLoader < IPager < IPageParser >, IPageParser >
    {

    }
}