using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Omnidoc
{
    public class DocumentType : IEquatable < DocumentType? >
    {
        private static Dictionary < string, DocumentType > cache = new Dictionary < string, DocumentType > ( );

        public static void Register ( params DocumentType [ ] types )
        {
            if ( types is null )
                throw new ArgumentNullException ( nameof ( types ) );

            lock ( cache )
                foreach ( var type in types )
                    cache.TryAdd ( type.Extension, type );
        }

        [ SuppressMessage ( "Globalization", "CA1308:Normalize strings to uppercase", Justification = "Extensions are lowercase" ) ]
        public static DocumentType? FromExtension ( string extension )
        {
            if ( extension is null )
                throw new ArgumentNullException ( nameof ( extension ) );

            return cache.TryGetValue ( extension.TrimStart ( '.' ).ToLowerInvariant ( ), out var type ) ? type : null;
        }

        [ SuppressMessage ( "Globalization", "CA1308:Normalize strings to uppercase", Justification = "Extensions are lowercase" ) ]
        public DocumentType ( string name, string contentType, string extension )
        {
            Name        = name        ?? throw new ArgumentNullException ( nameof ( name        ) );
            ContentType = contentType ?? throw new ArgumentNullException ( nameof ( contentType ) );
            Extension   = extension   ?? throw new ArgumentNullException ( nameof ( extension   ) );
            Extension   = Extension.TrimStart ( '.' ).ToLowerInvariant ( );
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