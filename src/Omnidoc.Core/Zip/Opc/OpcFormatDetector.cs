using System;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;

using Omnidoc.IO;
using Omnidoc.Services;

namespace Omnidoc.Zip.Opc
{
    public class OpcFormatDetector : ZipContainerFormatDetector
    {
        private static readonly IServiceDescriptor descriptor = new ServiceDescriptor
        (
            new [ ] { FileFormats.Opc }
        );

        public override IServiceDescriptor Descriptor => descriptor;

        protected override Task < FileFormat? > DetectAsync ( ZipArchive archive, CancellationToken cancellationToken )
        {
            if ( archive is null )
                throw new ArgumentNullException ( nameof ( archive ) );

            return Task.FromResult ( archive.GetEntry ( OpcPath.Relationships ) != null ? FileFormats.Opc : null );
        }
    }
}