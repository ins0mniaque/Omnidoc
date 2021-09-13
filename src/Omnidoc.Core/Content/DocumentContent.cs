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
        public double? FontSize { get; set; }
        public string? Text     { get; set; }
    }

    public class DocumentImage : DocumentContent
    {
        // Add url + way to get data? Auto-encode as data Uri?
    }
}