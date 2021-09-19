using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Omnidoc.Core
{
    public interface IPageRenderer : IPage
    {
        public Size PageSize { get; }

        Task RenderAsync ( Stream output, RenderingOptions options, CancellationToken cancellationToken = default );
    }
}