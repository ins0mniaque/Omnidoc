namespace Omnidoc.Content
{
    public abstract class DocumentContent
    {
        public double?          Left    { get; set; }
        public double?          Top     { get; set; }
        public double?          Right   { get; set; }
        public double?          Bottom  { get; set; }
        public DocumentMarkers? Markers { get; set; }
    }
}