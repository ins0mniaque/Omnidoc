namespace Omnidoc.Pdf
{
    public static class DocumentTypes
    {
        static DocumentTypes ( ) => DocumentType.Register ( Pdf );

        public static DocumentType Pdf { get; } = new DocumentType ( "Portable Document Format", "application/pdf", "pdf" );
    }
}