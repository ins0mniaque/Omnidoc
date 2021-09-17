using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Omnidoc.IO;
using Omnidoc.Services;

namespace Omnidoc.Image
{
    public class ImageDocumentTypeDetector : IDocumentTypeDetector
    {
        private static readonly IDocumentServiceDescriptor descriptor = new DocumentServiceDescriptor
        (
            new [ ] { DocumentTypes.Bmp, DocumentTypes.Gif, DocumentTypes.Jpeg, DocumentTypes.Png, DocumentTypes.Tiff }
        );

        private static readonly FileSignature [ ] signatures = new [ ]
        {
            new FileSignature ( "BM" ),
            new FileSignature ( "GIF87a" ),
            new FileSignature ( "GIF89a" ),
            new FileSignature ( 0xFF, 0xD8 ),
            new FileSignature ( 0x89, (byte) 'P', (byte) 'N', (byte) 'G', 0x0D, 0x0A, 0x1A, 0x0A ),
            new FileSignature ( "I I"  ),
            new FileSignature ( "II*." ),
            new FileSignature ( "MM.*" ),
            new FileSignature ( "MM.+" )
        };

        public IDocumentServiceDescriptor Descriptor => descriptor;

        public async Task < DocumentType? > DetectTypeAsync ( Stream stream, CancellationToken cancellationToken = default )
        {
            if ( stream is null )
                throw new ArgumentNullException ( nameof ( stream ) );

            return await stream.MatchAsync ( signatures ).ConfigureAwait ( false ) switch
            {
                0 => DocumentTypes.Bmp,
                1 => DocumentTypes.Gif,
                2 => DocumentTypes.Gif,
                3 => DocumentTypes.Jpeg,
                4 => DocumentTypes.Png,
                5 => DocumentTypes.Tiff,
                6 => DocumentTypes.Tiff,
                7 => DocumentTypes.Tiff,
                8 => DocumentTypes.Tiff,
                _ => null
            };
        }
    }
}