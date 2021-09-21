using System;
using System.Collections.Generic;
using System.Linq;

using Omnidoc.Collections;

namespace Omnidoc.IO
{
    public static class FileFormatDependency
    {
        public static IEnumerable < T > OrderByFileFormatDependency < T > ( this IEnumerable < T > source, Func < T, IEnumerable < FileFormat > > formats )
        {
            var sorted = source.SelectMany ( formats )
                               .OrderByDependency ( );

            return source.OrderBy ( FileFormatDependency );

            int FileFormatDependency ( T service )
            {
                return formats ( service ).Min ( format => Array.IndexOf ( sorted, format ) );
            }
        }

        public static FileFormat [ ] OrderByDependency ( this IEnumerable < FileFormat > formats )
        {
            if ( formats is null )
                throw new ArgumentNullException ( nameof ( formats ) );

            var dependencies = new Graph < FileFormat > ( );

            foreach ( var format in formats )
            {
                dependencies.Add ( format );
                if ( format.Base is FileFormat basedOn )
                    dependencies.AddEdge ( format, basedOn );
            }

            return dependencies.TopologicalSort ( );
        }
    }
}