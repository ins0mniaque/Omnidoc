namespace Omnidoc.Content
{
    public class DocumentText : DocumentContent
    {
        public DocumentText ( string text )
        {
            Text = text;
        }

        public string? Color      { get; set; }
        public string? Font       { get; set; }
        public double? FontSize   { get; set; }
        public int?    FontWeight { get; set; }
        public string  Text       { get; }
    }
}