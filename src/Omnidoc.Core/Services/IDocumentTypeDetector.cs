using System.IO;

namespace Omnidoc.Services
{
    public interface IDocumentTypeDetector : IDocumentService
    {
        DocumentType? DetectType ( Stream stream );
    }
}