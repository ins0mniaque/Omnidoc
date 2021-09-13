using System.Diagnostics.CodeAnalysis;

using Omnidoc.Dynamic;

namespace Omnidoc
{
    [ SuppressMessage ( "Naming", "CA1710:Identifiers should have correct suffix", Justification = "Dynamic object" ) ]
    public class DocumentMetadata : DynamicDictionary { }
}