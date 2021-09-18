using Omnidoc.IO;

namespace Omnidoc.Xps
{
    public static class FileFormats
    {
        static FileFormats ( ) => FileFormat.Register ( Xps, Oxps );

        public static FileFormat Zip  { get; } = new FileFormat ( "ZIP Archive",                       "application/zip",                "zip" );
        public static FileFormat Xps  { get; } = new FileFormat ( "Microsoft XML Paper Specification", "application/vnd.ms-xpsdocument", "xps",  Zip );
        public static FileFormat Oxps { get; } = new FileFormat ( "Open XML Paper Specification",      "application/oxps",               "oxps", Zip );
    }
}