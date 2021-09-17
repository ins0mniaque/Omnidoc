using Omnidoc.IO;

namespace Omnidoc.Pdf
{
    public static class FileFormats
    {
        static FileFormats ( ) => FileFormat.Register ( Bmp, Pdf );

        public static FileFormat Bmp { get; } = new FileFormat ( "Bitmap image",             "image/bmp",       "bmp" );
        public static FileFormat Pdf { get; } = new FileFormat ( "Portable Document Format", "application/pdf", "pdf" );
    }
}