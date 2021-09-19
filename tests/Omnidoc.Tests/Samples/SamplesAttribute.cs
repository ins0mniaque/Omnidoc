using System;
using System.Collections.Generic;
using System.Reflection;

using Xunit.Sdk;

namespace Omnidoc
{
    public sealed class SamplesAttribute : DataAttribute
    {
        public SamplesAttribute ( string path )
        {
            Path = path;
        }

        public string Path { get; }

        public override IEnumerable < object? [ ] > GetData ( MethodInfo testMethod )
        {
            var assembly = typeof ( SamplesAttribute ).Assembly;
            var root     = $"{ assembly.GetName ( ).Name }.Samples.";
            var path     = root + Path.Replace ( '/', '.' );

            foreach ( var name in assembly.GetManifestResourceNames ( ) )
            {
                if ( ! name.StartsWith ( path, StringComparison.Ordinal ) )
                    continue;

                yield return new [ ] { assembly.GetManifestResourceStream ( name ) };
            }
        }
    }
}