using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Tesseract;

using Omnidoc.Core;
using Omnidoc.Model;
using Omnidoc.Services;

namespace Omnidoc.Image
{
    public class ImageDocumentParser : IDocumentParser
    {
        private static readonly IServiceDescriptor descriptor = new ServiceDescriptor
        (
            new [ ] { FileFormats.Bmp, FileFormats.Gif, FileFormats.Jpeg, FileFormats.Png, FileFormats.Tiff },
            new [ ] { typeof ( Content ) }
        );

        public static Func < TesseractEngine > CreateDefaultEngine { get; set; } = ( ) => new TesseractEngine ( @"tessdata", "eng" );

        public ImageDocumentParser ( ) : this ( CreateDefaultEngine ) { }
        public ImageDocumentParser ( Func < TesseractEngine > createEngine )
        {
            CreateEngine = createEngine;
        }

        private Func < TesseractEngine > CreateEngine { get; }

        public IServiceDescriptor Descriptor => descriptor;

        public Task < IPager < IPageParser > > LoadAsync ( Stream document, CancellationToken cancellationToken = default )
        {
            return Task.Run ( ( ) => Load ( document ), cancellationToken );
        }

        private IPager < IPageParser > Load ( Stream document )
        {
            var engine = CreateEngine ( );

            return new ImagePager < IPageParser > ( document, engine,
                                                    page => new ImagePageParser ( engine, page ) );
        }
    }
}