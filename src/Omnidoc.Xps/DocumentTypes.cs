namespace Omnidoc.Xps
{
    public static class DocumentTypes
    {
        static DocumentTypes ( ) => DocumentType.Register ( Xps, Oxps );

        public static DocumentType Xps  { get; } = new DocumentType ( "Microsoft XML Paper Specification", "application/vnd.ms-xpsdocument", "xps"  );
        public static DocumentType Oxps { get; } = new DocumentType ( "Open XML Paper Specification",      "application/oxps",               "oxps" );
    }
}