using System;
using System.Diagnostics.CodeAnalysis;

namespace Omnidoc.IO
{
    public partial class FileFormat : IEquatable < FileFormat? >
    {
        [ SuppressMessage ( "Globalization", "CA1308:Normalize strings to uppercase", Justification = "Extensions are lowercase" ) ]
        public FileFormat ( string name, string contentType, string extension )
        {
            Name        = name        ?? throw new ArgumentNullException ( nameof ( name        ) );
            ContentType = contentType ?? throw new ArgumentNullException ( nameof ( contentType ) );
            Extension   = extension   ?? throw new ArgumentNullException ( nameof ( extension   ) );
            Extension   = Extension.TrimStart ( '.' ).ToLowerInvariant ( );
        }

        public string Name        { get; }
        public string ContentType { get; }
        public string Extension   { get; }

        public bool            Equals      ( FileFormat? other ) => ! ( other is null ) && ContentType == other.ContentType;
        public override bool   Equals      ( object      other ) => other is FileFormat format ? Equals ( format ) : false;
        public override int    GetHashCode ( )                   => ContentType.GetHashCode ( StringComparison.Ordinal );
        public override string ToString    ( )                   => ContentType;
    }
}