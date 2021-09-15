using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Tesseract;

using Omnidoc.Services;

namespace Omnidoc.Image
{
    public class ImageDocumentParser : IDocumentParser
    {
        public static Func < TesseractEngine > CreateDefaultEngine { get; set; } = ( ) => new TesseractEngine ( @"tessdata", "eng" );

        public ImageDocumentParser ( ) : this ( CreateDefaultEngine ) { }
        public ImageDocumentParser ( Func < TesseractEngine > createEngine )
        {
            CreateEngine = createEngine;
        }

        private Func < TesseractEngine > CreateEngine { get; }

        public IReadOnlyCollection < DocumentType > Types { get; } = new [ ] { DocumentTypes.Bmp, DocumentTypes.Gif, DocumentTypes.Jpeg, DocumentTypes.Png, DocumentTypes.Tiff };

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