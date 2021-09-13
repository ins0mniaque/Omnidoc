using System.Collections.Generic;
using System.Linq;

namespace Omnidoc
{
    public class DocumentServiceProvider : IDocumentServiceProvider
    {
        private readonly IReadOnlyCollection < IDocumentService > services;

        public DocumentServiceProvider ( IEnumerable < IDocumentService > services ) : this ( services.ToArray ( ) ) { }
        public DocumentServiceProvider ( params IDocumentService [ ] services )
        {
            this.services = services;
        }

        public IEnumerable < IDocumentService > GetServices ( ) => services;
    }
}