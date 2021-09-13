using System.Collections.Generic;

namespace Omnidoc
{
    public class DocumentEngine : IDocumentEngine
    {
        public DocumentEngine ( IEnumerable < IDocumentService > services ) : this ( new DocumentServiceProvider ( services ) ) { }
        public DocumentEngine ( params IDocumentService [ ]      services ) : this ( new DocumentServiceProvider ( services ) ) { }
        public DocumentEngine ( IDocumentServiceProvider services )
        {
            Services = services;
        }

        public IDocumentServiceProvider Services { get; }
    }
}