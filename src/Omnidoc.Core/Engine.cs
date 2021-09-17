using System.Collections.Generic;

using Omnidoc.Services;

namespace Omnidoc
{
    public class Engine : IEngine
    {
        public Engine ( IEnumerable < IService > services ) : this ( new ServiceProvider ( services ) ) { }
        public Engine ( params IService [ ]      services ) : this ( new ServiceProvider ( services ) ) { }
        public Engine ( IServiceProvider         services )
        {
            Services = services;
        }

        public IServiceProvider Services { get; }
    }
}