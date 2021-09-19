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

        bool Supports ( FileFormat format );
        bool Outputs  ( FileFormat format );

        bool Supports < TElement > ( );
        bool Supports ( Element element     );
        bool Supports ( Type    elementType );
    }
}