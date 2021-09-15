using System;
using System.Collections.Generic;
using System.Threading;

using Omnidoc.Model;

namespace Omnidoc.Services
{
    public interface IPageParser : IDisposable
    {
        IAsyncEnumerable < Content > ParseAsync ( CancellationToken cancellationToken = default );
    }
}