namespace Omnidoc.Core
{
    public interface IDocumentBuilder : IDocumentLoader < IDocument < IPage >, IPage > { }
}