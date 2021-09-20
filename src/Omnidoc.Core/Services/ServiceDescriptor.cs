using System;
using System.Collections.Generic;
using System.Linq;

using Omnidoc.IO;
using Omnidoc.Model;

namespace Omnidoc.Services
{
    public class ServiceDescriptor : IServiceDescriptor
    {
        public ServiceDescriptor ( IEnumerable < FileFormat > formats )                                           : this ( formats, Array.Empty < FileFormat > ( ), Type.EmptyTypes ) { }
        public ServiceDescriptor ( IEnumerable < FileFormat > formats, IEnumerable < Type >       elementTypes  ) : this ( formats, Array.Empty < FileFormat > ( ), elementTypes    ) { }
        public ServiceDescriptor ( IEnumerable < FileFormat > formats, IEnumerable < FileFormat > outputFormats ) : this ( formats, outputFormats,                  Type.EmptyTypes ) { }
        public ServiceDescriptor ( IEnumerable < FileFormat > formats, IEnumerable < FileFormat > outputFormats, IEnumerable < Type > elementTypes )
        {
            if ( elementTypes.FirstOrDefault ( type => ! typeof ( Element ).IsAssignableFrom ( type ) ) is Type nonElementType )
                throw new ArgumentException ( $"{ nonElementType.Name } does not derive from { nameof ( Element ) }", nameof ( elementTypes ) );

            Formats       = formats      .ToHashSet ( );
            OutputFormats = outputFormats.ToHashSet ( );
            ElementTypes  = elementTypes .ToHashSet ( );
        }

        public IEnumerable < FileFormat > Formats       { get; }
        public IEnumerable < FileFormat > OutputFormats { get; }
        public IEnumerable < Type >       ElementTypes  { get; }

        public bool Supports ( FileFormat format ) => Formats      .Contains ( format );
        public bool Outputs  ( FileFormat format ) => OutputFormats.Contains ( format );

        public bool Supports < TElement > ( )        => Supports ( typeof ( TElement ) );
        public bool Supports ( Element element     ) => Supports ( element?.GetType ( ) ?? throw new ArgumentNullException ( nameof ( element ) ) );
        public bool Supports ( Type    elementType ) => ElementTypes.Any ( type => type.IsAssignableFrom ( elementType ) );
    }
}