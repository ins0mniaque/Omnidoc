using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Omnidoc.Core.Disposables;
using Omnidoc.IO;
using Omnidoc.Services;

namespace Omnidoc.Core
{
    public class FileFormatConverterChain : AsyncDisposableContainer < IService >, IFileFormatConverter
    {
        public FileFormatConverterChain ( IEnumerable < IFileFormatConverter > chain ) : this ( chain.ToArray ( ) ) { }
        public FileFormatConverterChain ( params IFileFormatConverter [ ]      chain )
        {
            Chain      = chain ?? throw new ArgumentNullException ( nameof ( chain ) );
            Descriptor = new ServiceDescriptor ( chain [  0 ].Descriptor.Formats,
                                                 chain [ ^1 ].Descriptor.OutputFormats );
        }

        protected IReadOnlyList < IFileFormatConverter > Chain { get; private set; }

        public IServiceDescriptor Descriptor { get; }

        public virtual async Task ConvertAsync ( Stream file, Stream output, OutputOptions options, CancellationToken cancellationToken = default )
        {
            if ( file    is null ) throw new ArgumentNullException   ( nameof ( file    ) );
            if ( output  is null ) throw new ArgumentNullException   ( nameof ( output  ) );
            if ( options is null ) throw new ArgumentNullException   ( nameof ( options ) );
            if ( Chain   is null ) throw new ObjectDisposedException ( GetType ( ).Name );

            var buffer = (Stream?) null;

            try
            {
                for ( var index = 0; index < Chain.Count; index++ )
                {
                    var isLast           = index == Chain.Count - 1;
                    var converter        = Chain [ index ];
                    var converterOptions = isLast ? options : new OutputOptions ( SelectOutputFormat ( converter, Chain [ index + 1 ] ) );
                    var converterOutput  = isLast ? output  : buffer = CreateBufferStream ( ) ?? throw new InvalidOperationException ( Strings.Error_FailedToCreateBufferStream );

                    await converter.ConvertAsync   ( file, converterOutput, converterOptions, cancellationToken )
                                   .ConfigureAwait ( false );

                    if ( index > 0 && ! isLast )
                    {
                        buffer = file;
                        buffer.Dispose ( );
                        buffer = null;
                    }

                    file = converterOutput;
                    file.Seek ( 0, SeekOrigin.Begin );
                }
            }
            finally
            {
                buffer?.Dispose ( );
            }
        }

        protected virtual Stream CreateBufferStream ( ) => new VirtualStream ( );

        protected virtual FileFormat SelectOutputFormat ( IFileFormatConverter converter, IFileFormatConverter nextConverter )
        {
            if ( converter     is null ) throw new ArgumentNullException ( nameof ( converter     ) );
            if ( nextConverter is null ) throw new ArgumentNullException ( nameof ( nextConverter ) );

            return converter.Descriptor.OutputFormats.Intersect ( nextConverter.Descriptor.Formats ).FirstOrDefault ( ) ??
                   throw new NotSupportedException ( string.Format ( CultureInfo.InvariantCulture,
                                                                     Strings.Error_UnsupportedConverterChain,
                                                                     converter    .GetType ( ).Name,
                                                                     nextConverter.GetType ( ).Name ) );
        }

        protected override IEnumerable < IService >? BeginDispose ( ) => Chain;
    }
}