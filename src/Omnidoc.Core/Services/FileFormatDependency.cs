using System;
using System.Collections.Generic;
using System.Linq;

using Omnidoc.Collections;
using Omnidoc.IO;

namespace Omnidoc.Services
{
    public static class FileFormatDependency
    {
        public static IEnumerable < IService > OrderByDependency ( this IEnumerable < IService > services )
        {
            var sorted = services.SelectMany ( service => service.Descriptor.Formats )
                                 .OrderByDependency ( );

            return services.OrderBy ( FileFormatDependency );

            int FileFormatDependency ( IService service )
            {
                return service.Descriptor.Formats.Select ( format => Array.IndexOf ( sorted, format ) )
                                                 .Min    ( );
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