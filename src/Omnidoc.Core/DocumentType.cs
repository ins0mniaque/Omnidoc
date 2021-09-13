using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace Omnidoc
{
    public class DocumentType : IEquatable < DocumentType? >
    {
        private static ConcurrentDictionary < string, DocumentType > types = new ConcurrentDictionary < string, DocumentType > ( );

        public static void Register ( DocumentType type )
        {
            if ( type is null )
                throw new ArgumentNullException ( nameof ( type ) );

            types.TryAdd ( type.Extension, type );
        }

        [ SuppressMessage ( "Globalization", "CA1308:Normalize strings to uppercase", Justification = "Extensions are lowercase" ) ]
        public static DocumentType? FromExtension ( string extension )
        {
            if ( extension is null )
                throw new ArgumentNullException ( nameof ( extension ) );

            return types.TryGetValue ( extension.ToLowerInvariant ( ), out var type ) ? type : null;
        }

        [ SuppressMessage ( "Globalization", "CA1308:Normalize strings to uppercase", Justification = "Extensions are lowercase" ) ]
        public DocumentType ( string name, string contentType, string extension )
        {
            Name        = name                            ?? throw new ArgumentNullException ( nameof ( name        ) );
            ContentType = contentType                     ?? throw new ArgumentNullException ( nameof ( contentType ) );
            Extension   = extension?.ToLowerInvariant ( ) ?? throw new ArgumentNullException ( nameof ( extension   ) );
        }

        public string Name        { get; }
        public string ContentType { get; }
        public string Extension   { get; }

        public bool            Equals      ( DocumentType? other ) => ! ( other is null ) && ContentType == other.ContentType;
        public override bool   Equals      ( object        other ) => other is DocumentType type ? Equals ( type ) : false;
        public override int    GetHashCode ( )                     => ContentType.GetHashCode ( StringComparison.Ordinal );
        public override string ToString    ( )                     => ContentType;
    }
}