using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Omnidoc.Collections
{
    [ SuppressMessage ( "Naming", "CA1710:Identifiers should have correct suffix", Justification = "Graph is the correct suffix" ) ]
    public class Graph < TVertex, TEdge > : HashSet < TVertex >, IGraph < TVertex, TEdge >
        where TVertex : notnull
        where TEdge   : notnull, IEdge < TVertex >
    {
        protected const int DefaultCapacity = 16;

        public Graph ( )
            : this ( DefaultCapacity, DefaultCapacity ) { }

        public Graph ( int verticesCapacity, int edgesCapacity )
            : this ( edgesCapacity, verticesCapacity,
                     EqualityComparer < TVertex >.Default,
                     EqualityComparer < TEdge   >.Default ) { }

        public Graph ( IEqualityComparer < TVertex > vertexComparer,
                       IEqualityComparer < TEdge   > edgeComparer )
            : this ( DefaultCapacity, DefaultCapacity,
                     vertexComparer,  edgeComparer ) { }

        public Graph ( int verticesCapacity, int edgesCapacity,
                       IEqualityComparer < TVertex > vertexComparer,
                       IEqualityComparer < TEdge   > edgeComparer )
            : base ( verticesCapacity, vertexComparer )
        {
            Edges = new HashSet < TEdge > ( edgesCapacity, edgeComparer );
        }

        public ISet < TEdge > Edges { get; }

        public IEnumerable < TVertex > InEdges  ( TVertex vertex ) => Edges.Where ( edge => Comparer.Equals ( edge.Target, vertex ) ).Select ( edge => edge.Source );
        public IEnumerable < TVertex > OutEdges ( TVertex vertex ) => Edges.Where ( edge => Comparer.Equals ( edge.Source, vertex ) ).Select ( edge => edge.Target );

        public ILookup < TVertex, TVertex > InEdges  ( ) => Edges.ToLookup ( edge => edge.Target, edge => edge.Source, Comparer );
        public ILookup < TVertex, TVertex > OutEdges ( ) => Edges.ToLookup ( edge => edge.Source, edge => edge.Target, Comparer );
    }

    [ SuppressMessage ( "Naming", "CA1710:Identifiers should have correct suffix", Justification = "Graph is the correct suffix" ) ]
    public class Graph < TVertex > : Graph < TVertex, Edge < TVertex > > where TVertex : notnull
    {
        public Graph ( )
            : this ( EqualityComparer < TVertex >.Default ) { }

        public Graph ( int verticesCapacity, int edgesCapacity )
            : this ( edgesCapacity, verticesCapacity, EqualityComparer < TVertex >.Default ) { }

        public Graph ( IEqualityComparer < TVertex > vertexComparer )
            : this ( DefaultCapacity, DefaultCapacity, vertexComparer ) { }

        public Graph ( int verticesCapacity, int edgesCapacity, IEqualityComparer < TVertex > vertexComparer )
            : base ( verticesCapacity, edgesCapacity, vertexComparer, EqualityComparer < Edge < TVertex > >.Default ) { }

        public bool AddEdge    ( TVertex source, TVertex target ) => Edges.Add    ( new Edge < TVertex > ( source, target, Comparer ) );
        public bool RemoveEdge ( TVertex source, TVertex target ) => Edges.Remove ( new Edge < TVertex > ( source, target, Comparer ) );
    }
}