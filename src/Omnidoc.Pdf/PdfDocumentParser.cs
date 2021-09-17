﻿using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Omnidoc.Core;
using Omnidoc.Model;
using Omnidoc.Services;

namespace Omnidoc.Pdf
{
    using static PDFiumCore.fpdfview;

    public class PdfDocumentParser : IDocumentParser
    {
        private static readonly IServiceDescriptor descriptor = new ServiceDescriptor
        (
            new [ ] { FileFormats.Pdf },
            new [ ] { typeof ( Content ) }
        );

        public IServiceDescriptor Descriptor => descriptor;

        public Task < IPager < IPageParser > > LoadAsync ( Stream document, CancellationToken cancellationToken = default )
        {
            return Task.Run ( ( ) => Load ( document ), cancellationToken );
        }

        private IPager < IPageParser > Load ( Stream document )
        {
            var fileAccess = document.ToFileAccess ( );

            return new PdfPager < IPageParser > ( FPDF_LoadCustomDocument ( fileAccess, null ), fileAccess,
                                                  page => new PdfPageParser ( page ) );
        }
    }
}