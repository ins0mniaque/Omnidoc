namespace Omnidoc.Model
{
    public class Edge : Element
    {
        public Edge ( EdgeType type, Element element, Element source, Element target )
        {
            Type    = type;
            Element = element;
            Source  = source;
            Target  = target;
        }

        public EdgeType Type    { get; }
        public Element  Element { get; }
        public Element  Source  { get; }
        public Element  Target  { get; }
    }
}