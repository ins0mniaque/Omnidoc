using System;
using System.Collections.Generic;
using System.Linq;

using Omnidoc.IO;
using Omnidoc.Model;

namespace Omnidoc.Services
{
    public interface IServiceDescriptor
    {
        IEnumerable < FileFormat > Formats       { get; }
        IEnumerable < FileFormat > OutputFormats { get; }
        IEnumerable < Type >       ElementTypes  { get; }

        bool Supports ( FileFormat format ) => Formats      .Contains ( format );
        bool Outputs  ( FileFormat format ) => OutputFormats.Contains ( format );

        bool Supports < TElement > ( )        => Supports ( typeof ( TElement ) );
        bool Supports ( Element element     ) => Supports ( element?.GetType ( ) ?? throw new ArgumentNullException ( nameof ( element ) ) );
        bool Supports ( Type    elementType ) => ElementTypes.Any ( type => type.IsAssignableFrom ( elementType ) );
    }
}