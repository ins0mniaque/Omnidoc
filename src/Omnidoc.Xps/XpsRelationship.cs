using System;
using System.Linq;
using System.Xml.Linq;

using Omnidoc.Zip.Opc;

namespace Omnidoc.Xps
{
    public static class XpsRelationship
    {
        public static OpcRelationship? Find ( this OpcRelationship [ ] relationships, XNamespace type )
        {
            if ( relationships is null )
                throw new ArgumentNullException ( nameof ( relationships ) );

            return relationships.FirstOrDefault ( relationship => relationship.Type == type );
        }
    }
}