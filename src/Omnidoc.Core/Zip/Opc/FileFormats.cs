using Omnidoc.IO;

namespace Omnidoc.Zip.Opc
{
    public static class FileFormats
    {
        static FileFormats ( ) => FileFormat.Register ( Opc );

        public static FileFormat Opc { get; } = new FileFormat ( "OPC Package", "application/vnd.openxmlformats-package+zip", string.Empty );
    }
}