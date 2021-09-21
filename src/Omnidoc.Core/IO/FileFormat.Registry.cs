using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Omnidoc.IO
{
    public partial class FileFormat
    {
        private static readonly List < FileFormat > registry = new ( );

        public static void Register ( params FileFormat [ ] formats )
        {
            if ( formats is null )
                throw new ArgumentNullException ( nameof ( formats ) );

            lock ( registry )
                foreach ( var format in formats )
                    registry.Add ( format );
        }

        public static void Unregister ( params FileFormat [ ] formats )
        {
            if ( formats is null )
                throw new ArgumentNullException ( nameof ( formats ) );

            lock ( registry )
                foreach ( var format in formats )
                    registry.Remove ( format );
        }

        public static FileFormat? FromContentType ( string contentType )
        {
            if ( contentType is null )
                throw new ArgumentNullException ( nameof ( contentType ) );

            lock ( registry )
                return registry.FirstOrDefault ( format => string.Equals ( format.ContentType, contentType, StringComparison.OrdinalIgnoreCase ) );
        }

        public static FileFormat? FromExtension ( string extension )
        {
            if ( extension is null )
                throw new ArgumentNullException ( nameof ( extension ) );

            extension = extension.TrimStart ( '.' );

            lock ( registry )
                return registry.FirstOrDefault ( format => string.Equals ( format.Extension, extension, StringComparison.OrdinalIgnoreCase ) );
        }

        public static FileFormat? FromPath ( string path )
        {
            return FromExtension ( Path.GetExtension ( path ) );
        }
    }
}