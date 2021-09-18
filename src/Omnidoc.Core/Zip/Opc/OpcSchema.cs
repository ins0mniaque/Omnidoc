using System.Xml.Linq;

namespace Omnidoc.Zip.Opc
{
    public static class OpcSchema
    {
        public static readonly XNamespace ContentTypes  = "http://schemas.openxmlformats.org/package/2006/content-types";
        public static readonly XNamespace Relationships = "http://schemas.openxmlformats.org/package/2006/relationships";
        public static readonly XNamespace Metadata      = "http://schemas.openxmlformats.org/package/2006/metadata/core-properties";
        public static readonly XNamespace Thumbnail     = "http://schemas.openxmlformats.org/package/2006/relationships/metadata/thumbnail";
    }
}