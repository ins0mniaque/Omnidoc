using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Omnidoc.IO;

namespace Omnidoc.Services
{
    public class DocumentConverterChain : IDocumentConverter
    {
        public DocumentConverterChain ( IEnumerable < IDocumentConverter > chain ) : this ( chain.ToArray ( ) ) { }
        public DocumentConverterChain ( params IDocumentConverter [ ] chain )
        {
            Chain      = chain;
            Descriptor = new DocumentServiceDescriptor ( chain [  0 ].Descriptor.Types,
                                                         chain [ ^1 ].Descriptor.OutputTypes );
        }

        protected IReadOnlyList < IDocumentConverter > Chain { get; }

        public IDocumentServiceDescriptor Descriptor { get; }

        public virtual async Task ConvertAsync ( Stream document, Stream output, OutputOptions options, CancellationToken cancellationToken = default )
        {
            if ( document is null ) throw new ArgumentNullException ( nameof ( document ) );
            if ( output   is null ) throw new ArgumentNullException ( nameof ( output   ) );
            if ( options  is null ) throw new ArgumentNullException ( nameof ( options  ) );

            var memory = (Stream?) null;

            try
            {
                for ( var index = 0; index < Chain.Count; index++ )
                {
                    var isLast           = index == Chain.Count - 1;
                    var converter        = Chain [ index ];
                    var converterOptions = isLast ? options : new OutputOptions ( SelectOutputType ( converter, Chain [ index + 1 ] ) );
                    var converterOutput  = isLast ? output  : memory = CreateBufferStream ( ) ?? throw new InvalidOperationException ( Strings.Error_FailedToCreateBufferStream );

                    await converter.ConvertAsync   ( document, converterOutput, converterOptions, cancellationToken )
                                   .ConfigureAwait ( false );

                    if ( index > 0 && ! isLast )
                    {
                        memory = document;
                        memory.Dispose ( );
                        memory = null;
                    }

                    document = converterOutput;
                    document.Seek ( 0, SeekOrigin.Begin );
                }
            }
            finally
            {
                memory?.Dispose ( );
            }
        }

        protected virtual Stream CreateBufferStream ( ) => new VirtualStream ( );

        protected virtual DocumentType SelectOutputType ( IDocumentConverter converter, IDocumentConverter nextConverter )
        {
            if ( converter     is null ) throw new ArgumentNullException ( nameof ( converter     ) );
            if ( nextConverter is null ) throw new ArgumentNullException ( nameof ( nextConverter ) );

            return converter.Descriptor.OutputTypes.Intersect ( nextConverter.Descriptor.Types ).FirstOrDefault ( ) ??
                   throw new NotSupportedException ( string.Format ( CultureInfo.InvariantCulture,
                                                                     Strings.Error_UnsupportedConverterChain,
                                                                     converter    .GetType ( ).Name,
                                                                     nextConverter.GetType ( ).Name ) );
        }
    }
}