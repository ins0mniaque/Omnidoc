using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Xunit.Sdk;

namespace Omnidoc
{
    [ CLSCompliant ( false ) ]
    public sealed class SampleAttribute : DataAttribute
    {
        public SampleAttribute ( params string [ ] paths )
        {
            Paths = paths;
        }

        public IReadOnlyCollection < string > Paths { get; }

        public override IEnumerable < object? [ ] > GetData ( MethodInfo testMethod )
        {
            var assembly = typeof(SampleAttribute).Assembly;
            var root     = $"{ assembly.GetName ( ).Name }.Samples.";

            yield return Paths.Select  ( path => assembly.GetManifestResourceStream ( root + path.Replace ( '/', '.' ) ) )
                              .ToArray ( );
        }
    }
}