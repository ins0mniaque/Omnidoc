using System.Collections.Generic;
using System.Linq;

using Omnidoc.Core;
using Omnidoc.IO;
using Omnidoc.Model;

namespace Omnidoc.Services
{
    public interface IServiceProvider
    {
        IEnumerable < IService > GetServices ( );

        IEnumerable < T > GetServices < T > ( FileFormat format ) where T : IService
        {
            return GetServices ( ).OfType < T > ( ).Where ( service => service.Descriptor.Supports ( format ) );
        }

        IEnumerable < IFileFormatConverter > GetConverters ( FileFormat inputFormat, FileFormat outputFormat )
        {
            return GetServices < IFileFormatConverter > ( inputFormat ).Where ( converter => converter.Descriptor.Outputs ( outputFormat ) );
        }

        IEnumerable < IFileMetadataReader > GetMetadataReaders ( FileFormat format ) => GetServices < IFileMetadataReader > ( format );
        IEnumerable < IDocumentParser >     GetParsers         ( FileFormat format ) => GetServices < IDocumentParser >     ( format );
        IEnumerable < IDocumentPreviewer >  GetPreviewers      ( FileFormat format ) => GetServices < IDocumentPreviewer >  ( format );
        IEnumerable < IDocumentBuilder >    GetBuilders        ( FileFormat format ) => GetServices < IDocumentBuilder >    ( format );
        IEnumerable < IDocumentRenderer >   GetRenderers       ( FileFormat format ) => GetServices < IDocumentRenderer >   ( format );
        IEnumerable < IDocumentComposer >   GetComposers       ( FileFormat format ) => GetServices < IDocumentComposer >   ( format );

        IEnumerable < IDocumentComposer > GetComposers < TContent > ( FileFormat format ) where TContent : Content
        {
            return GetServices < IDocumentComposer > ( format ).Where ( writer => writer.Descriptor.Supports < TContent > ( ) );
        }

        T? GetService < T > ( FileFormat format ) where T : IService
        {
            return GetServices < T > ( format ).FirstOrDefault ( );
        }

        IFileFormatConverter? GetConverter ( FileFormat inputFormat, FileFormat outputFormat )
        {
            return GetConverters ( inputFormat, outputFormat ).FirstOrDefault ( );
        }

        IFileMetadataReader? GetMetadataReader ( FileFormat format ) => GetMetadataReaders ( format ).FirstOrDefault ( );
        IDocumentParser?     GetParser         ( FileFormat format ) => GetParsers         ( format ).FirstOrDefault ( );
        IDocumentBuilder?    GetBuilder        ( FileFormat format ) => GetBuilders        ( format ).FirstOrDefault ( );
        IDocumentPreviewer?  GetPreviewer      ( FileFormat format ) => GetPreviewers      ( format ).FirstOrDefault ( );
        IDocumentRenderer?   GetRenderer       ( FileFormat format ) => GetRenderers       ( format ).FirstOrDefault ( );
        IDocumentComposer?   GetComposer       ( FileFormat format ) => GetComposers       ( format ).FirstOrDefault ( );

        IDocumentComposer? GetComposer < TContent > ( FileFormat format ) where TContent : Content
        {
            return GetComposers < TContent > ( format ).FirstOrDefault ( );
        }
    }
}