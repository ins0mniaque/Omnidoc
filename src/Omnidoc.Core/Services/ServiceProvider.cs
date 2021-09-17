using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Omnidoc.Services
{
    public class ServiceProvider : IServiceProvider
    {
        private readonly Lazy < IService [ ] > services;

        public ServiceProvider ( )
        {
            services = new Lazy < IService [ ] > ( ( ) => ResolveServices ( ) );
        }

        public object? GetService ( Type serviceType )
        {
            if ( serviceType == typeof ( IEnumerable < IService > ) )
                return services.Value;
            else if ( typeof ( IService ).IsAssignableFrom ( serviceType ) )
                return services.Value.FirstOrDefault ( service => serviceType.IsAssignableFrom ( service.GetType ( ) ) );

            return null;
        }

        private static IService [ ] ResolveServices ( )
        {
            var services = new List < IService > ( );

            foreach ( var serviceType in GetReferencedAssemblies ( ).SelectMany ( ServiceTypes ) )
                services.Add ( (IService) Activator.CreateInstance ( serviceType ) );

            return services.ToArray ( );
        }

        private static IEnumerable < Type > ServiceTypes ( Assembly assembly )
        {
            return assembly.ExportedTypes.Where ( typeof ( IService ).IsAssignableFrom );
        }

        private static IEnumerable < Assembly > GetReferencedAssemblies ( )
        {
            var loaded = new HashSet < string   > ( );
            var queue  = new Queue   < Assembly > ( );

            queue.Enqueue ( Assembly.GetEntryAssembly ( ) );

            while ( queue.TryDequeue ( out var assembly ) )
            {
                yield return assembly;

                foreach ( var reference in assembly.GetReferencedAssemblies ( ) )
                {
                    if ( ! IsSystemReference ( reference ) && ! loaded.Contains ( reference.FullName ) )
                    {
                        queue .Enqueue ( Assembly.Load ( reference ) );
                        loaded.Add     ( reference.FullName );
                    }
                }
            }
        }

        private static bool IsSystemReference ( AssemblyName name )
        {
            return name.Name.StartsWith ( "System.",    StringComparison.Ordinal ) ||
                   name.Name.StartsWith ( "Microsoft.", StringComparison.Ordinal );
        }
    }
}