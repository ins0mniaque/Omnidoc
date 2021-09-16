namespace Omnidoc.Model
{
    public class Node
    {
        public Node ( Level level, Content content )
        {
            Level    = level;
            Content  = content;
            Children = new NodeCollection ( );
        }

        public Level          Level    { get; }
        public Content        Content  { get; }
        public NodeCollection Children { get; }
    }
}