using System.Collections.Generic;
using System.Linq;

namespace Omnidoc.Services
{
    public class ServiceProvider : IServiceProvider
    {
        private readonly IReadOnlyCollection < IService > services;

        public ServiceProvider ( IEnumerable < IService > services ) : this ( services.ToArray ( ) ) { }
        public ServiceProvider ( params IService [ ] services )
        {
            this.services = services;
        }

        public IEnumerable < IService > GetServices ( ) => services;
    }
}