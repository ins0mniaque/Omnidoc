namespace Omnidoc.Content
{
    public abstract class DocumentContent
    {
        public double? Left   { get; set; }
        public double? Top    { get; set; }
        public double? Right  { get; set; }
        public double? Bottom { get; set; }
    }

    public class DocumentText : DocumentContent
    {
        public DocumentText ( string text )
        {
            Text = text;
        }

        public string? Color    { get; set; }
        public string? Font     { get; set; }
        public double? FontSize { get; set; }
        public string  Text     { get; }
    }

    public class DocumentImage : DocumentContent
    {
        // Add url + way to get data? Auto-encode as data Uri?
    }
}