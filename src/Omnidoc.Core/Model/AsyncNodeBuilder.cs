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
        public static async IAsyncEnumerable < Node > BuildAsync ( this IAsyncEnumerable < Content > contents, ParserOptions options, [ EnumeratorCancellation ] CancellationToken cancellationToken )
        {
            if ( contents is null )
                throw new ArgumentNullException ( nameof ( contents ) );

            var builder = new NodeBuilder ( options );

            await foreach ( var content in contents.WithCancellation ( cancellationToken ) )
            {
                builder.Add ( content );

                while ( builder.HasNodes )
                    yield return builder.ReadNode ( );
            }

            builder.Flush ( );

            while ( builder.HasNodes )
                yield return builder.ReadNode ( );
        }
    }
}