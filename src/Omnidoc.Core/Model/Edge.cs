namespace Omnidoc.Model
{
    public class Edge : Content
    {
        public Edge ( EdgeType type, Content source, Content target, Content content )
        {
            Type    = type;
            Source  = source;
            Target  = target;
            Content = content;
        }

        public EdgeType Type    { get; }
        public Content  Source  { get; }
        public Content  Target  { get; }
        public Content  Content { get; }
    }
}