using System;
using System.Collections.Generic;
using System.Linq;

using Omnidoc.Content;
using Omnidoc.Services;

namespace Omnidoc
{
    public interface IDocumentServiceProvider
    {
        IEnumerable < IDocumentService > GetServices ( );

        IEnumerable < T > GetServices < T > ( DocumentType type ) where T : IDocumentService
        {
            return GetServices ( ).OfType < T > ( ).Where ( service => service.Types.Contains ( type ) );
        }

        IEnumerable < IDocumentConverter > GetConverters ( DocumentType inputType, DocumentType outputType )
        {
            return GetServices < IDocumentConverter > ( inputType ).Where ( converter => converter.OutputTypes.Contains ( outputType ) );
        }

        IEnumerable < IDocumentMetadataReader > GetMetadataReaders ( DocumentType type ) => GetServices < IDocumentMetadataReader > ( type );
        IEnumerable < IDocumentParser >         GetParsers         ( DocumentType type ) => GetServices < IDocumentParser >         ( type );
        IEnumerable < IDocumentPreviewer >      GetPreviewers      ( DocumentType type ) => GetServices < IDocumentPreviewer >      ( type );
        IEnumerable < IDocumentReader >         GetReaders         ( DocumentType type ) => GetServices < IDocumentReader >         ( type );
        IEnumerable < IDocumentRenderer >       GetRenderers       ( DocumentType type ) => GetServices < IDocumentRenderer >       ( type );
        IEnumerable < IDocumentWriter >         GetWriters         ( DocumentType type ) => GetServices < IDocumentWriter >         ( type );

        IEnumerable < IDocumentWriter > GetWriters < TContent > ( DocumentType type ) where TContent : DocumentContent
        {
            return GetServices < IDocumentWriter > ( type ).Where ( writer => writer.Supports < TContent > ( ) );
        }

        T? GetService < T > ( DocumentType type ) where T : IDocumentService
        {
            return GetServices < T > ( type ).FirstOrDefault ( );
        }

        IDocumentConverter? GetConverter ( DocumentType inputType, DocumentType outputType )
        {
            return GetConverters ( inputType, outputType ).FirstOrDefault ( );
        }

        IDocumentMetadataReader? GetMetadataReader ( DocumentType type ) => GetMetadataReaders ( type ).FirstOrDefault ( );
        IDocumentParser?         GetParser         ( DocumentType type ) => GetParsers         ( type ).FirstOrDefault ( );
        IDocumentReader?         GetReader         ( DocumentType type ) => GetReaders         ( type ).FirstOrDefault ( );
        IDocumentPreviewer?      GetPreviewer      ( DocumentType type ) => GetPreviewers      ( type ).FirstOrDefault ( );
        IDocumentRenderer?       GetRenderer       ( DocumentType type ) => GetRenderers       ( type ).FirstOrDefault ( );
        IDocumentWriter?         GetWriter         ( DocumentType type ) => GetWriters         ( type ).FirstOrDefault ( );

        IDocumentWriter? GetWriter < TContent > ( DocumentType type ) where TContent : DocumentContent
        {
            return GetWriters < TContent > ( type ).FirstOrDefault ( );
        }
    }
}