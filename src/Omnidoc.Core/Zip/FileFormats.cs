using Omnidoc.IO;

namespace Omnidoc.Zip
{
    public static class FileFormats
    {
        static FileFormats ( ) => FileFormat.Register ( Zip );

        public static FileFormat Zip { get; } = new FileFormat ( "ZIP Archive", "application/zip", "zip" );
    }
}