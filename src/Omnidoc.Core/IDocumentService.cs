using System.Collections.Generic;

namespace Omnidoc
{
    public interface IDocumentService
    {
        IReadOnlyCollection < DocumentType > Types { get; }
    }
}