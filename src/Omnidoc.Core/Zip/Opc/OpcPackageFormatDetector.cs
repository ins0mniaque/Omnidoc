using System;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;

using Omnidoc.IO;

namespace Omnidoc.Zip.Opc
{
    public abstract class OpcPackageFormatDetector : ZipContainerFormatDetector
    {
        protected override async Task < FileFormat? > DetectAsync ( ZipArchive archive, CancellationToken cancellationToken )
        {
            if ( archive is null )
                throw new ArgumentNullException ( nameof ( archive ) );

            var relationships = await archive.TryReadRelationshipsAsync ( cancellationToken ).ConfigureAwait ( false );
            if ( relationships is null )
                return null;

            return await DetectAsync ( archive, relationships, cancellationToken ).ConfigureAwait ( false );
        }

        protected abstract Task < FileFormat? > DetectAsync ( ZipArchive package, OpcRelationship [ ] relationships, CancellationToken cancellationToken );
    }
}