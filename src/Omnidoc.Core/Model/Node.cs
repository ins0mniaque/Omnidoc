namespace Omnidoc.Model
{
    public class Node
    {
        public Node ( Level level, Element element )
        {
            Level    = level;
            Element  = element;
            Children = new NodeCollection ( );
        }

        public Level          Level    { get; }
        public Element        Element  { get; }
        public NodeCollection Children { get; }
    }
}