using Omnidoc.IO;

namespace Omnidoc.HtmlToPdf
{
    public static class FileFormats
    {
        static FileFormats ( ) => FileFormat.Register ( Html, Pdf );

        public static FileFormat Html { get; } = new FileFormat ( "HyperText Markup Language", "text/html",       "html" );
        public static FileFormat Pdf  { get; } = new FileFormat ( "Portable Document Format",  "application/pdf", "pdf"  );
    }
}