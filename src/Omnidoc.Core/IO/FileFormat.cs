using System;
using System.Diagnostics.CodeAnalysis;

namespace Omnidoc.IO
{
    [ SuppressMessage ( "Globalization", "CA1308:Normalize strings to uppercase", Justification = "Extensions are lowercase" ) ]
    public partial class FileFormat : IEquatable < FileFormat? >
    {
        public FileFormat ( string name, string contentType, string extension ) : this ( name, contentType, extension, null! ) { }
        public FileFormat ( string name, string contentType, string extension, FileFormat basedOn )
        {
            Name        = name        ?? throw new ArgumentNullException ( nameof ( name        ) );
            ContentType = contentType ?? throw new ArgumentNullException ( nameof ( contentType ) );
            Extension   = extension   ?? throw new ArgumentNullException ( nameof ( extension   ) );
            Extension   = Extension.TrimStart ( '.' ).ToLowerInvariant ( );
            Base        = basedOn;
        }

        public string      Name        { get; }
        public string      ContentType { get; }
        public string      Extension   { get; }
        public FileFormat? Base        { get; }

        public bool            Equals      ( FileFormat? other ) => ! ( other is null ) && ContentType == other.ContentType;
        public override bool   Equals      ( object      other ) => other is FileFormat format ? Equals ( format ) : false;
        public override int    GetHashCode ( )                   => ContentType.GetHashCode ( StringComparison.Ordinal );
        public override string ToString    ( )                   => ContentType;
    }
}