namespace Omnidoc.Image
{
    public static class DocumentTypes
    {
        static DocumentTypes ( ) => DocumentType.Register ( Bmp, Gif, Jpeg, Png, Tga, Tiff );

        public static DocumentType Bmp  { get; } = new DocumentType ( "Bitmap image", "image/bmp",  "bmp"  );
        public static DocumentType Gif  { get; } = new DocumentType ( "GIF image",    "image/gif",  "gif"  );
        public static DocumentType Jpeg { get; } = new DocumentType ( "JPEG image",   "image/jpeg", "jpg"  );
        public static DocumentType Png  { get; } = new DocumentType ( "PNG image",    "image/png",  "png"  );
        public static DocumentType Tga  { get; } = new DocumentType ( "TGA image",    "image/tga",  "tga"  );
        public static DocumentType Tiff { get; } = new DocumentType ( "TIFF image",   "image/tiff", "tiff" );
    }
}