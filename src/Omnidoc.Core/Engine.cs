using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Omnidoc.Core;
using Omnidoc.Core.Disposables;
using Omnidoc.IO;
using Omnidoc.Services;

namespace Omnidoc
{
    public class Engine : AsyncDisposableContainer < IService >, IEngine
    {
        private readonly Lazy < ICollection < IService > > services;

        public Engine ( )                                   : this ( new ServiceProvider ( )             ) { }
        public Engine ( IServiceProvider         provider ) : this ( ( ) => ResolveServices ( provider ) ) { }
        public Engine ( IEnumerable < IService > services ) : this ( ( ) => services                     ) { }
        public Engine ( params IService [ ]      services ) : this ( services.AsEnumerable ( )           ) { }

        protected Engine ( Func < IEnumerable < IService > > services )
            : this ( new Lazy < ICollection < IService > > ( ( ) => services ( ).OrderByFileFormatDependency ( service => service.Descriptor.Formats ).ToList ( ) ) ) { }

        protected Engine ( Lazy < ICollection < IService > > services )
        {
            this.services = services;
        }

        protected static IEnumerable < IService > ResolveServices ( IServiceProvider provider )
        {
            if ( provider is null )
                throw new ArgumentNullException ( nameof ( provider ) );

            if ( provider.GetService ( typeof ( IEnumerable < IService > ) ) is IEnumerable < IService > services )
                return services;

            throw new ArgumentException ( $"No services of type { typeof ( IService ).FullName } have been registered", nameof ( provider ) );
        }

        public IEnumerable < IService > Services
        {
            get
            {
                ThrowIfDisposed ( );

                return services.Value;
            }
        }

        public virtual async Task < FileFormat? > DetectFileFormatAsync ( Stream file, CancellationToken cancellationToken = default )
        {
            foreach ( var detector in Services.OfType < IFileFormatDetector > ( ) )
                if ( await detector.DetectAsync ( file, cancellationToken ).ConfigureAwait ( false ) is FileFormat format )
                    return format;

            return null;
        }

        public virtual T? FindService < T > ( FileFormat format ) where T : IService
        {
            return FindServices < T > ( format ).FirstOrDefault ( );
        }

        public virtual T? FindService < T > ( FileFormat inputFormat, FileFormat outputFormat ) where T : IService
        {
            return FindServices < T > ( inputFormat, outputFormat ).FirstOrDefault ( );
        }

        public virtual IEnumerable < T > FindServices < T > ( FileFormat format ) where T : IService
        {
            return Services.OfType < T > ( ).Where ( service => service.Descriptor.Supports ( format ) );
        }

        public virtual IEnumerable < T > FindServices < T > ( FileFormat inputFormat, FileFormat outputFormat ) where T : IService
        {
            return Services.OfType < T > ( ).Where ( service => service.Descriptor.Supports ( inputFormat  ) &&
                                                                service.Descriptor.Outputs  ( outputFormat ) );
        }

        protected override IEnumerable < IService >? BeginDispose ( ) => services != null && services.IsValueCreated ? services.Value : null;
    }
}