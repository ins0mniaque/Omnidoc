using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Tesseract;

using Omnidoc.Model;
using Omnidoc.Services;

namespace Omnidoc.Image
{
    public class ImageDocumentParser : IDocumentParser
    {
        private static readonly IDocumentServiceDescriptor descriptor = new DocumentServiceDescriptor
        (
            new [ ] { DocumentTypes.Bmp, DocumentTypes.Gif, DocumentTypes.Jpeg, DocumentTypes.Png, DocumentTypes.Tiff },
            new [ ] { typeof ( Content ) }
        );

        public static Func < TesseractEngine > CreateDefaultEngine { get; set; } = ( ) => new TesseractEngine ( @"tessdata", "eng" );

        public ImageDocumentParser ( ) : this ( CreateDefaultEngine ) { }
        public ImageDocumentParser ( Func < TesseractEngine > createEngine )
        {
            CreateEngine = createEngine;
        }

        private Func < TesseractEngine > CreateEngine { get; }

        public IDocumentServiceDescriptor Descriptor => descriptor;

        public Task < IPager < IPageParser > > PrepareAsync ( Stream document, CancellationToken cancellationToken = default )
        {
            return Task.Run ( ( ) => Prepare ( document ), cancellationToken );
        }

        private IPager < IPageParser > Prepare ( Stream document )
        {
            var engine = CreateEngine ( );

            return new ImagePager < IPageParser > ( document, engine,
                                                    page => new ImagePageParser ( engine, page ) );
        }
    }
}