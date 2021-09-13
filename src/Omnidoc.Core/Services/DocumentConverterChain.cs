using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Omnidoc.Services
{
    public class DocumentConverterChain : IDocumentConverter
    {
        public DocumentConverterChain ( IEnumerable < IDocumentConverter > chain ) : this ( chain.ToArray ( ) ) { }
        public DocumentConverterChain ( params IDocumentConverter [ ] chain )
        {
            Chain = chain;
        }

        protected IReadOnlyList < IDocumentConverter > Chain { get; }

        public virtual IReadOnlyCollection < DocumentType > Types       => Chain [  0 ].Types;
        public virtual IReadOnlyCollection < DocumentType > OutputTypes => Chain [ ^1 ].OutputTypes;

        public virtual async Task ConvertAsync ( Stream document, Stream output, OutputOptions options, CancellationToken cancellationToken = default )
        {
            if ( document is null ) throw new ArgumentNullException ( nameof ( document ) );
            if ( output   is null ) throw new ArgumentNullException ( nameof ( output   ) );
            if ( options  is null ) throw new ArgumentNullException ( nameof ( options  ) );

            var memory = (MemoryStream?) null;

            try
            {
                for ( var index = 0; index < Chain.Count; index++ )
                {
                    var isLast           = index == Chain.Count - 1;
                    var converter        = Chain [ index ];
                    var converterOptions = isLast ? options : new OutputOptions ( SelectContentType ( converter, Chain [ index + 1 ] ) );
                    var converterOutput  = isLast ? output  : memory = new MemoryStream ( );

                    await converter.ConvertAsync   ( document, converterOutput, converterOptions, cancellationToken )
                                   .ConfigureAwait ( false );

                    if ( index > 0 && ! isLast )
                    {
                        memory = (MemoryStream) document;
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
                memory = null;
            }
        }

        public virtual DocumentType SelectContentType ( IDocumentConverter converter, IDocumentConverter nextConverter )
        {
            if ( converter     is null ) throw new ArgumentNullException ( nameof ( converter     ) );
            if ( nextConverter is null ) throw new ArgumentNullException ( nameof ( nextConverter ) );

            return converter.OutputTypes.Intersect ( nextConverter.Types ).FirstOrDefault ( ) ??
                   throw new NotSupportedException ( $"{ converter.GetType ( ).Name } cannot chain to { nextConverter.GetType ( ).Name }" );
        }
    }
}