using Omnidoc.Collections;

namespace Omnidoc.Model.Elements
{
    public class Edge : Element, IEdge < Element >
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