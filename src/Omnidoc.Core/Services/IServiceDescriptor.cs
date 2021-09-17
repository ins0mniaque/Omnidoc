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
        IEnumerable < Type >       ContentTypes  { get; }

        bool Supports ( FileFormat format ) => Formats      .Contains ( format );
        bool Outputs  ( FileFormat format ) => OutputFormats.Contains ( format );

        bool Supports < TContent > ( )        => Supports ( typeof ( TContent ) );
        bool Supports ( Content content     ) => Supports ( content?.GetType ( ) ?? throw new ArgumentNullException ( nameof ( content ) ) );
        bool Supports ( Type    contentType ) => ContentTypes.Any ( type => type.IsAssignableFrom ( contentType ) );
    }
}