using System;

using Omnidoc.Services;

namespace Omnidoc
{
    public interface IService : IAsyncDisposable, IDisposable
    {
        IServiceDescriptor Descriptor { get; }
    }
}