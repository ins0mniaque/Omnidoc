namespace Omnidoc.HtmlToPdf
{
    public static class DocumentTypes
    {
        static DocumentTypes ( ) => DocumentType.Register ( Html, Pdf );

        public static DocumentType Html { get; } = new DocumentType ( "HyperText Markup Language", "text/html",       "html" );
        public static DocumentType Pdf  { get; } = new DocumentType ( "Portable Document Format",  "application/pdf", "pdf"  );
    }
}