using System;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;

using Omnidoc.IO;
using Omnidoc.Services;
using Omnidoc.Zip;

namespace Omnidoc.Xps
{
    public class XpsFormatDetector : ZipContainerFormatDetector
    {
        private static readonly IServiceDescriptor descriptor = new ServiceDescriptor
        (
            new [ ] { FileFormats.Xps, FileFormats.OpenXps }
        );

        public override IServiceDescriptor Descriptor => descriptor;

        protected override Task < FileFormat? > DetectAsync ( ZipArchive archive, CancellationToken cancellationToken )
        {
            if ( archive is null )
                throw new ArgumentNullException ( nameof ( archive ) );

            // TODO: Detect OpenXPS files
            return Task.FromResult ( archive.GetEntry ( "_rels/.rels" ) != null ? FileFormats.Xps : null );
        }
    }
}