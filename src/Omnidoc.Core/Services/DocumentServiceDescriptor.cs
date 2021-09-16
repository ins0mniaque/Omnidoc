using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Omnidoc.Model;

namespace Omnidoc.Services
{
    public sealed class DocumentServiceDescriptor : IDocumentServiceDescriptor
    {
        public DocumentServiceDescriptor ( IEnumerable < DocumentType > types )                                            : this ( types, Array.Empty < DocumentType > ( ), Type.EmptyTypes ) { }
        public DocumentServiceDescriptor ( IEnumerable < DocumentType > types, IEnumerable < Type >         contentTypes ) : this ( types, Array.Empty < DocumentType > ( ), contentTypes    ) { }
        public DocumentServiceDescriptor ( IEnumerable < DocumentType > types, IEnumerable < DocumentType > outputTypes  ) : this ( types, outputTypes,                      Type.EmptyTypes ) { }
        public DocumentServiceDescriptor ( IEnumerable < DocumentType > types, IEnumerable < DocumentType > outputTypes, IEnumerable < Type > contentTypes )
        {
            if ( contentTypes.FirstOrDefault ( type => ! typeof ( Content ).IsAssignableFrom ( type ) ) is Type nonContentType )
                throw new ArgumentException ( string.Format ( CultureInfo.InvariantCulture, Strings.Error_TypeMismatch, nonContentType.Name, nameof ( Content ) ), nameof ( contentTypes ) );

            Types        = types       .ToHashSet ( );
            OutputTypes  = outputTypes .ToHashSet ( );
            ContentTypes = contentTypes.ToHashSet ( );
        }

        public IEnumerable < DocumentType > Types        { get; }
        public IEnumerable < DocumentType > OutputTypes  { get; }
        public IEnumerable < Type >         ContentTypes { get; }
    }
}