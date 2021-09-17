namespace Omnidoc.Model.Elements
{
    public class Edge : Element
    {
        public Edge ( Element element, Element source, Element target )
        {
            Element = element;
            Source  = source;
            Target  = target;
        }

        public Element Element { get; }
        public Element Source  { get; }
        public Element Target  { get; }
    }
}