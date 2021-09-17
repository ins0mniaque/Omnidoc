﻿using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;

using Omnidoc.Core;
using Omnidoc.Model;
using Omnidoc.Services;

namespace Omnidoc.Xps
{
    public class XpsDocumentParser : IDocumentParser
    {
        private static readonly IServiceDescriptor descriptor = new ServiceDescriptor
        (
            new [ ] { FileFormats.Xps, FileFormats.Oxps },
            new [ ] { typeof ( Element ) }
        );

        public IServiceDescriptor Descriptor => descriptor;

        public Task < IPager < IPageParser > > LoadAsync ( Stream document, CancellationToken cancellationToken = default )
        {
            return Task.Run ( ( ) => Load ( document ), cancellationToken );
        }

        private static IPager < IPageParser > Load ( Stream document )
        {
            return new XpsPager < IPageParser > ( new ZipArchive ( document ),
                                                  page => new XpsPageParser ( page ) );
        }
    }
}