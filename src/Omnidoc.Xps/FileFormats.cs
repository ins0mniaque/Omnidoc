using Omnidoc.IO;

namespace Omnidoc.Xps
{
    public static class FileFormats
    {
        static FileFormats ( ) => FileFormat.Register ( OpenXps, Xps );

        public static FileFormat OpenXps { get; } = new FileFormat ( "Open XML Paper Specification",      "application/oxps",               "oxps", Zip.FileFormats.Zip );
        public static FileFormat Xps     { get; } = new FileFormat ( "Microsoft XML Paper Specification", "application/vnd.ms-xpsdocument", "xps",  Zip.FileFormats.Zip );
    }
}