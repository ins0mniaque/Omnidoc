namespace Omnidoc
{
    public class OutputOptions
    {
        public OutputOptions ( DocumentType type )
        {
            Type = type;
        }

        public DocumentType Type { get; }
    }
}