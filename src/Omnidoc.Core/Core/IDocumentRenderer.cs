namespace Omnidoc.Core
{
    public interface IDocumentRenderer : IDocumentLoader < IPager < IPageRenderer >, IPageRenderer > { }
}