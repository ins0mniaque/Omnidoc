﻿using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Omnidoc.Core;
using Omnidoc.IO;
using Omnidoc.Services;

namespace Omnidoc.Pdf
{
    public class PdfFormatDetector : IFileFormatDetector
    {
        private static readonly IServiceDescriptor descriptor = new ServiceDescriptor
        (
            new [ ] { FileFormats.Pdf }
        );

        private static readonly FileSignature [ ] signatures = new [ ]
        {
            new FileSignature ( "%PDF" )
        };

        public IServiceDescriptor Descriptor => descriptor;

        public async Task < FileFormat? > DetectAsync ( Stream file, CancellationToken cancellationToken = default )
        {
            if ( file is null )
                throw new ArgumentNullException ( nameof ( file ) );

            return await file.MatchAsync ( signatures ).ConfigureAwait ( false ) switch
            {
                0 => FileFormats.Pdf,
                _ => null
            };
        }
    }
}