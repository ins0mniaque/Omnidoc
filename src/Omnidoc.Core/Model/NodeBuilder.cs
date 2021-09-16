using System;
using System.Collections.Generic;
using System.Globalization;

namespace Omnidoc.Model
{
    public class NodeBuilder
    {
        private readonly Stack < Node > stack = new Stack < Node > ( );
        private readonly Queue < Node > nodes = new Queue < Node > ( );
        private          Node?          node;

        public NodeBuilder ( ParserOptions options )
        {
            Options = options;
        }

        public ParserOptions Options { get; }

        public bool HasNodes     => nodes.Count > 0;
        public Node ReadNode ( ) => nodes.Dequeue ( );

        public IEnumerable < Node > ReadNodes ( )
        {
            while ( nodes.Count > 0 )
                yield return nodes.Dequeue ( );
        }

        public void Add ( Content content )
        {
            if ( content is null )
                throw new ArgumentNullException ( nameof ( content ) );

            var level = content.Levels.Match ( Options.Levels ).Top ( );

            if ( level != Level.None )
            {
                if ( content.Levels.Is ( Levels.Start ) )
                {
                    if ( node != null )
                        stack.Push ( node );

                    node = new Node ( level, content );
                }

                if ( content.Levels.Is ( Levels.End ) )
                {
                    if ( Options.Strict && node == null )
                        throw new InvalidOperationException ( string.Format ( CultureInfo.InvariantCulture, Strings.Error_MalformedDocument, level, Levels.End, Levels.Start ) );

                    if ( stack.TryPop ( out var parent ) )
                    {
                        node = parent;
                    }
                    else if ( node != null )
                    {
                        nodes.Enqueue ( node );
                        node = null;
                    }
                }
            }

            if ( node != null )
                node.Children.Add ( new Node ( content.Levels.Top ( ), content ) );
        }

        public void Flush ( )
        {
            if ( node != null )
            {
                if ( Options.Strict )
                    throw new InvalidOperationException ( string.Format ( CultureInfo.InvariantCulture, Strings.Error_MalformedDocument, node.Level, Levels.Start, Levels.End ) );

                nodes.Enqueue ( node );
                node = null;
            }
        }
    }
}