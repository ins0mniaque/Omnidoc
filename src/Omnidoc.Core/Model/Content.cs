using System.Diagnostics.CodeAnalysis;

namespace Omnidoc.Model
{
    [ SuppressMessage ( "Usage", "CA2227:Collection properties should be read only", Justification = "Dynamic object" ) ]
    public abstract class Content
    {
        public Point?    Position { get; set; }
        public Size?     Size     { get; set; }
        public Cell?     Cell     { get; set; }
        public Levels    Levels   { get; set; }
        public Metadata? Metadata { get; set; }
    }
}