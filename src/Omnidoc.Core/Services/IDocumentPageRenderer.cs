using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Omnidoc.Services
{
    public interface IDocumentPageRenderer : IDisposable
    {
        public double PageWidth  { get; }
        public double PageHeight { get; }

        Task RenderAsync ( Stream output, RenderingOptions options, CancellationToken cancellationToken = default );
    }
}