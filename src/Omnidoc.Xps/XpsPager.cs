using System;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

using Omnidoc.Core;
using Omnidoc.Zip.Opc;

namespace Omnidoc.Xps
{
    public sealed class XpsPager < T > : IPager < T >
    {
        public XpsPager ( ZipArchive document, Func < ZipArchiveEntry, T > factory )
        {
            Document = document;
            Factory  = factory;
        }

        public XpsPager ( ZipArchive document, IDisposable disposable, Func < ZipArchiveEntry, T > factory )
        {
            Document   = document;
            Disposable = disposable;
            Factory    = factory;
        }

        public  ZipArchive                  Document   { get; }
        private IDisposable?                Disposable { get; }
        private Func < ZipArchiveEntry, T > Factory    { get; }
        private string [ ]?                 Pages      { get; set; }

        public async Task < int > GetPageCountAsync ( CancellationToken cancellationToken )
        {
            Pages ??= await LoadPagesAsync ( cancellationToken ).ConfigureAwait ( false );

            return Pages.Length;
        }

        public async Task < T > GetPageAsync ( int index, CancellationToken cancellationToken )
        {
            Pages ??= await LoadPagesAsync ( cancellationToken ).ConfigureAwait ( false );

            return Factory ( Document.GetEntry ( Pages [ index ] ) );
        }

        private async Task < string [ ] > LoadPagesAsync ( CancellationToken cancellationToken )
        {
            var relationships = await Document.TryReadRelationshipsAsync ( cancellationToken ).ConfigureAwait ( false );
            if ( relationships is null )
                return Array.Empty < string > ( );

            var xmlns = XpsSchema.Namespace;
            var fdseq = relationships.Find ( XpsSchema.FixedDocumentSequence );

            if ( fdseq is null )
            {
                xmlns = OpenXpsSchema.Namespace;
                fdseq = relationships.Find ( OpenXpsSchema.FixedDocumentSequence );
            }

            if ( fdseq is null || fdseq.Entry is null )
                return Array.Empty < string > ( );

            var xml = await XDocument.LoadAsync      ( fdseq.Entry.Open ( ), LoadOptions.None, cancellationToken )
                                     .ConfigureAwait ( false );

            var document = xml.Element  ( xmlns + "FixedDocumentSequence" )
                              .Elements ( xmlns + "DocumentReference"  )
                              .Select   ( element => element.Attribute ( "Source" ).Value )
                              .FirstOrDefault ( );

            var documentEntry = document is null ? null : Document.GetEntry ( document );
            if ( documentEntry is null )
                return Array.Empty < string > ( );

            xml = await XDocument.LoadAsync      ( documentEntry.Open ( ), LoadOptions.None, cancellationToken )
                                 .ConfigureAwait ( false );

            return xml.Element  ( xmlns + "FixedDocument" )
                      .Elements ( xmlns + "PageContent"   )
                      .Select   ( element => element.Attribute ( "Source" ).Value )
                      .ToArray  ( );
        }

        private bool isDisposed;

        public void Dispose ( )
        {
            if ( ! isDisposed )
            {
                Document   .Dispose ( );
                Disposable?.Dispose ( );

                isDisposed = true;
            }
        }
    }
}