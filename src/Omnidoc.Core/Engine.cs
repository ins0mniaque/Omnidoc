using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Omnidoc.Services;

namespace Omnidoc
{
    public class Engine : IEngine
    {
        private readonly Lazy < IService [ ] > services;

        private Engine ( Func < IService [ ] > services )
        {
            this.services = new Lazy < IService [ ] > ( services );
        }

        public Engine ( )                                   : this ( new ServiceProvider ( )             ) { }
        public Engine ( IEnumerable < IService > services ) : this ( services.ToArray ( )                ) { }
        public Engine ( IServiceProvider         provider ) : this ( ( ) => ResolveServices ( provider ) ) { }
        public Engine ( params IService [ ]      services ) : this ( ( ) => services                     ) { }

        public IEnumerable < IService > Services => services.Value;

        private static IService [ ] ResolveServices ( IServiceProvider provider )
        {
            if ( provider is null )
                throw new ArgumentNullException ( nameof ( provider ) );

            if ( provider.GetService ( typeof ( IEnumerable < IService > ) ) is IEnumerable < IService > services )
                return services.ToArray ( );

            throw new ArgumentException ( string.Format ( CultureInfo.InvariantCulture, Strings.Error_NoServicesRegistered, typeof ( IService ).FullName ), nameof ( provider ) );
        }
    }
}