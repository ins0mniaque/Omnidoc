using System.Diagnostics.CodeAnalysis;

using Omnidoc.Collections;

namespace Omnidoc.Model
{
    [ SuppressMessage ( "Naming", "CA1710:Identifiers should have correct suffix", Justification = "Dynamic object" ) ]
    [ SuppressMessage ( "Naming", "CA1724:TypeNamesShouldNotMatchNamespaces",      Justification = "System.Runtime.Remoting is not available in .NET Standard" ) ]
    public class Metadata : DynamicDictionary { }
}