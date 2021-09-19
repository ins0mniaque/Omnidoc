using System;
using System.Collections.Generic;
using System.Threading;

using Omnidoc.Model;

namespace Omnidoc.Core
{
    public interface IPageParser : IPage
    {
        IAsyncEnumerable < Element > ParseAsync ( ParserOptions options, CancellationToken cancellationToken = default );
    }
}