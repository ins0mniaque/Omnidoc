using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Omnidoc.Core;
using Omnidoc.Core.Disposables;
using Omnidoc.Services;

namespace Omnidoc.Pdf
{
    using static PDFiumCore.fpdfview;

    public sealed class PdfDocumentRenderer : AsyncDisposable, IDocumentRenderer
    {
        private static readonly IServiceDescriptor descriptor = new ServiceDescriptor
        (
            new [ ] { FileFormats.Pdf },
            new [ ] { FileFormats.Bmp }
        );

        public IServiceDescriptor Descriptor => descriptor;

        public Task < IPager < IPageRenderer > > LoadAsync ( Stream document, CancellationToken cancellationToken = default )
        {
            return Task.Run ( ( ) => Load ( document ), cancellationToken );
        }

        private static IPager < IPageRenderer > Load ( Stream document )
        {
            var fileAccess = document.ToFileAccess ( );

            return new PdfPager < IPageRenderer > ( FPDF_LoadCustomDocument ( fileAccess, null ), fileAccess,
                                                    page => new PdfPageRenderer ( page ) );
        }
    }
}