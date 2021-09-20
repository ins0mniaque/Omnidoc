using Omnidoc.IO;

namespace Omnidoc.Html
{
    public static class FileFormats
    {
        static FileFormats ( ) => FileFormat.Register ( Html );

        public static FileFormat Html { get; } = new FileFormat ( "HyperText Markup Language", "text/html", "html" );
    }
}