namespace Omnidoc.Core
{
    public interface IDocumentLoader < TDocument, TPage > : IService, IFileLoader < TDocument >
        where TDocument : IPager < TPage >
        where TPage     : IPage { }
}