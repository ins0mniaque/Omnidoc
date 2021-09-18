using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;

using Omnidoc.IO;
using Omnidoc.Services;
using Omnidoc.Zip.Opc;

namespace Omnidoc.Xps
{
    public class XpsFormatDetector : OpcPackageFormatDetector
    {
        private static readonly IServiceDescriptor descriptor = new ServiceDescriptor
        (
            new [ ] { FileFormats.Xps, FileFormats.OpenXps }
        );

        public override IServiceDescriptor Descriptor => descriptor;

        protected override Task < FileFormat? > DetectAsync ( ZipArchive package, OpcRelationship [ ] relationships, CancellationToken cancellationToken )
        {
            return Task.FromResult ( relationships.Find ( XpsSchema    .FixedDocumentSequence ) != null ? FileFormats.Xps     :
                                     relationships.Find ( OpenXpsSchema.FixedDocumentSequence ) != null ? FileFormats.OpenXps :
                                                                                                          null );
        }
    }
}