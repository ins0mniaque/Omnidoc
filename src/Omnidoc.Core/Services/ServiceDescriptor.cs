using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Omnidoc.IO;
using Omnidoc.Model;

namespace Omnidoc.Services
{
    public sealed class ServiceDescriptor : IServiceDescriptor
    {
        public ServiceDescriptor ( IEnumerable < FileFormat > formats )                                           : this ( formats, Array.Empty < FileFormat > ( ), Type.EmptyTypes ) { }
        public ServiceDescriptor ( IEnumerable < FileFormat > formats, IEnumerable < Type >       contentTypes  ) : this ( formats, Array.Empty < FileFormat > ( ), contentTypes    ) { }
        public ServiceDescriptor ( IEnumerable < FileFormat > formats, IEnumerable < FileFormat > outputFormats ) : this ( formats, outputFormats,                  Type.EmptyTypes ) { }
        public ServiceDescriptor ( IEnumerable < FileFormat > formats, IEnumerable < FileFormat > outputFormats, IEnumerable < Type > contentTypes )
        {
            if ( contentTypes.FirstOrDefault ( type => ! typeof ( Content ).IsAssignableFrom ( type ) ) is Type nonContentType )
                throw new ArgumentException ( string.Format ( CultureInfo.InvariantCulture, Strings.Error_TypeMismatch, nonContentType.Name, nameof ( Content ) ), nameof ( contentTypes ) );

            Formats       = formats      .ToHashSet ( );
            OutputFormats = outputFormats.ToHashSet ( );
            ContentTypes  = contentTypes .ToHashSet ( );
        }

        public IEnumerable < FileFormat > Formats        { get; }
        public IEnumerable < FileFormat > OutputFormats  { get; }
        public IEnumerable < Type >       ContentTypes { get; }
    }
}