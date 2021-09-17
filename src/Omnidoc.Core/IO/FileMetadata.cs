using System.Diagnostics.CodeAnalysis;

using Omnidoc.Collections;

namespace Omnidoc.IO
{
    [ SuppressMessage ( "Naming", "CA1710:Identifiers should have correct suffix", Justification = "Dynamic object" ) ]
    public class FileMetadata : DynamicDictionary { }
}