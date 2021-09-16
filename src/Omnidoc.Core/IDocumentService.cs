using Omnidoc.Services;

namespace Omnidoc
{
    public interface IDocumentService
    {
        IDocumentServiceDescriptor Descriptor { get; }
    }
}