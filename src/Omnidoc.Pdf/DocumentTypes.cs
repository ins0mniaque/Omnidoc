namespace Omnidoc.Pdf
{
    public static class DocumentTypes
    {
        static DocumentTypes ( ) => DocumentType.Register ( Bmp, Pdf );

        public static DocumentType Bmp { get; } = new DocumentType ( "Bitmap image",             "image/bmp",       "bmp" );
        public static DocumentType Pdf { get; } = new DocumentType ( "Portable Document Format", "application/pdf", "pdf" );
    }
}