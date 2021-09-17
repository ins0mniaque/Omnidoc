using Omnidoc.IO;

namespace Omnidoc.Image
{
    public static class FileFormats
    {
        static FileFormats ( ) => FileFormat.Register ( Bmp, Gif, Jpeg, Png, Tga, Tiff );

        public static FileFormat Bmp  { get; } = new FileFormat ( "Bitmap image", "image/bmp",  "bmp"  );
        public static FileFormat Gif  { get; } = new FileFormat ( "GIF image",    "image/gif",  "gif"  );
        public static FileFormat Jpeg { get; } = new FileFormat ( "JPEG image",   "image/jpeg", "jpg"  );
        public static FileFormat Png  { get; } = new FileFormat ( "PNG image",    "image/png",  "png"  );
        public static FileFormat Tga  { get; } = new FileFormat ( "TGA image",    "image/tga",  "tga"  );
        public static FileFormat Tiff { get; } = new FileFormat ( "TIFF image",   "image/tiff", "tiff" );
    }
}