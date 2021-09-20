using System;
using System.Collections.Generic;

namespace Omnidoc.Collections
{
    public static class GraphAlgorithm
    {
        private enum GraphColor { White, Gray, Black }

        /// <summary>
        /// Directed acyclic graph topological sort
        /// </summary>
        /// <remarks>O(V + E)</remarks>
        /// <exception cref="InvalidOperationException">The graph is not acyclic</exception>
        public static TVertex [ ] TopologicalSort < TVertex, TEdge > ( this IGraph < TVertex, TEdge > graph )
            where TVertex : notnull
            where TEdge   : notnull, IEdge < TVertex >
        {
            if ( graph is null )
                throw new ArgumentNullException ( nameof ( graph ) );

            var frames   = new Stack < (TVertex, IEnumerator < TVertex >) > ( );
            var colors   = new Dictionary < TVertex, GraphColor > ( graph.Count );
            var sorted   = new TVertex [ graph.Count ];
            var index    = graph.Count - 1;
            var outEdges = graph.OutEdges ( );

            foreach ( var v in graph )
                colors [ v ] = GraphColor.White;

            foreach ( var root in graph )
            {
                if ( colors [ root ] != GraphColor.White )
                    continue;

                colors [ root ] = GraphColor.Gray;
                frames.Push ( (root, outEdges [ root ].GetEnumerator ( )) );

                while ( frames.Count > 0 )
                {
                    var (u, edges) = frames.Pop ( );

                    while ( edges.MoveNext ( ) )
                    {
                        var edge = edges.Current;
                        var v    = edge;

                        switch ( colors [ v ] )
                        {
                            case GraphColor.Gray  : edges.Dispose ( ); throw new InvalidOperationException ( "Non-acyclic graph" );
                            case GraphColor.Black : continue;
                            case GraphColor.White :
                            {
                                frames.Push ( (u, edges) );
                                colors [ u = v ] = GraphColor.Gray;
                                edges = outEdges [ v ].GetEnumerator ( );

                                break;
                            }
                        }
                    }

                    edges.Dispose ( );

                    colors [ u ] = GraphColor.Black;
                    sorted [ index-- ] = u;
                }
            }

            return sorted;
        }
    }
}