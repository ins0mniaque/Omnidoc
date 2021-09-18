using System.IO.Compression;

namespace Omnidoc.Zip.Opc
{
    public class OpcRelationship
    {
        public OpcRelationship ( string id, string type, string target, ZipArchiveEntry? entry )
        {
            Id     = id;
            Type   = type;
            Target = target;
            Entry  = entry;
        }

        public string Id     { get; }
        public string Type   { get; }
        public string Target { get; }

        public ZipArchiveEntry? Entry { get; }
    }
}