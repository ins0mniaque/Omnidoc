using Omnidoc.IO;

namespace Omnidoc.Html.Pdf
{
    public static class FileFormats
    {
        static FileFormats ( ) => FileFormat.Register ( Pdf );

        public static FileFormat Pdf { get; } = new FileFormat ( "Portable Document Format",  "application/pdf", "pdf" );
    }
}