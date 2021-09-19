using System;
using System.Collections.Generic;

namespace Omnidoc.Collections
{
    public sealed class Edge < TVertex > : IEdge < TVertex >, IEquatable < Edge < TVertex > > where TVertex : notnull
    {
        public Edge ( TVertex source, TVertex target ) : this ( source, target, EqualityComparer < TVertex >.Default ) { }
        public Edge ( TVertex source, TVertex target, IEqualityComparer < TVertex > vertexComparer )
        {
            Source   = source;
            Target   = target;
            Comparer = vertexComparer;
        }

        public TVertex Source { get; }
        public TVertex Target { get; }

        private IEqualityComparer < TVertex > Comparer { get; }

        public bool Equals ( Edge < TVertex > other ) => ! ( other is null ) && Comparer.Equals ( Source, other.Source ) &&
                                                                                Comparer.Equals ( Target, other.Target );

        public override bool   Equals      ( object obj ) => obj is Edge < TVertex > other ? Equals ( other ) : false;
        public override int    GetHashCode ( )            => HashCode.Combine ( Source, Target );
        public override string ToString    ( )            => $"{ Source } => { Target }";
    }
}