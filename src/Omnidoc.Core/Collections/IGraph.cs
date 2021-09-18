using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Omnidoc.Collections
{
    [ SuppressMessage ( "Naming", "CA1710:Identifiers should have correct suffix", Justification = "Graph is the correct suffix" ) ]
    public interface IGraph < TVertex, TEdge > : ISet < TVertex >
        where TVertex : notnull
        where TEdge   : IEdge < TVertex >
    {
        ISet < TEdge > Edges { get; }

        ILookup < TVertex, TVertex > InEdges  ( );
        ILookup < TVertex, TVertex > OutEdges ( );

        IEnumerable < TVertex > InEdges  ( TVertex vertex );
        IEnumerable < TVertex > OutEdges ( TVertex vertex );
    }
}