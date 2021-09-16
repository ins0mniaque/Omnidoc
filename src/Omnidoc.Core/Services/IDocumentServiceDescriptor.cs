using System;
using System.Collections.Generic;
using System.Linq;

using Omnidoc.Model;

namespace Omnidoc.Services
{
    public interface IDocumentServiceDescriptor
    {
        IEnumerable < DocumentType > Types        { get; }
        IEnumerable < DocumentType > OutputTypes  { get; }
        IEnumerable < Type >         ContentTypes { get; }

        bool Supports ( DocumentType type ) => Types       .Contains ( type );
        bool Outputs  ( DocumentType type ) => OutputTypes .Contains ( type );
        bool Supports < TContent > ( )      => ContentTypes.Any      ( type => type.IsAssignableFrom ( typeof ( TContent ) ) );
        bool Supports ( Type    content )   => ContentTypes.Any      ( type => type.IsAssignableFrom ( content             ) );
        bool Supports ( Content content )   => ContentTypes.Any      ( type => type.IsAssignableFrom ( content.GetType ( ) ) );
    }
}