using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

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
            return Task.FromResult ( Contains ( XpsSchema    .FixedDocumentSequence ) ? FileFormats.Xps     :
                                     Contains ( OpenXpsSchema.FixedDocumentSequence ) ? FileFormats.OpenXps :
                                                                                        null );

            bool Contains ( XNamespace type ) => relationships.Any ( relationship => relationship.Type == type );
        }
    }
}