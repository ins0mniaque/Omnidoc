using System;
using System.Collections.Generic;
using System.Threading;

using Omnidoc.Content;

namespace Omnidoc.Services
{
    public interface IPageParser : IDisposable
    {
        IAsyncEnumerable < DocumentContent > ParseAsync ( CancellationToken cancellationToken = default );
    }
}