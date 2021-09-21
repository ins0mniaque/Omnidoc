using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Omnidoc.Model
{
    public static class AsyncNodeBuilder
    {
        public static IAsyncEnumerable < Node > BuildAsync ( this IAsyncEnumerable < Element > elements, ParserOptions options, CancellationToken cancellationToken )
        {
            if ( elements is null )
                throw new ArgumentNullException ( nameof ( elements ) );

            return BuildAsync ( cancellationToken );

            async IAsyncEnumerable < Node > BuildAsync ( [ EnumeratorCancellation ] CancellationToken cancellationToken )
            {
                var builder = new NodeBuilder ( options );

                await foreach ( var element in elements.WithCancellation ( cancellationToken ) )
                {
                    builder.Add ( element );

                    while ( builder.HasNodes )
                        yield return builder.ReadNode ( );
                }

                builder.Flush ( );

                while ( builder.HasNodes )
                    yield return builder.ReadNode ( );
            }
        }
    }
}