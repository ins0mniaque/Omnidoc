using Omnidoc.Services;

namespace Omnidoc
{
    public interface IService
    {
        IServiceDescriptor Descriptor { get; }
    }
}