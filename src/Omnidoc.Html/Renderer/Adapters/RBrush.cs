using System;

namespace Omnidoc.Html.Renderer.Adapters
{
    /// <summary>
    /// Adapter for platform specific brush objects - used to fill graphics (rectangles, polygons and paths).<br/>
    /// The brush can be solid color, gradient or image.
    /// </summary>
    public abstract class RBrush : IDisposable
    {
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) { }
    }
}