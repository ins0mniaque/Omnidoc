using System;
using System.Collections.Generic;
using System.Threading;

using Omnidoc.Model;

namespace Omnidoc.Core
{
    public interface IPageParser : IDisposable
    {
        IAsyncEnumerable < Element > ParseAsync ( ParserOptions options, CancellationToken cancellationToken = default );
    }
}