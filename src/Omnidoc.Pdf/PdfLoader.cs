using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using PDFiumCore;

using Omnidoc.Core;
using Omnidoc.Core.Disposables;

namespace Omnidoc.Pdf
{
    using static PDFiumCore.fpdfview;

    public abstract class PdfLoader < T > : FileLoader < Disposable < FpdfDocumentT >, T >
    {
        protected PdfLoader ( Func < Disposable < FpdfDocumentT >, T > factory ) : base ( factory ) { }

        protected override Task < Disposable < FpdfDocumentT >? > LoadFileAsync ( Stream input, CancellationToken cancellationToken )
        {
            return Task.Run ( ( ) => Load ( input ), cancellationToken );
        }

        private static Disposable < FpdfDocumentT >? Load ( Stream input )
        {
            var disposable = (FPDF_FILEACCESS?) null;

            try
            {
                var fileAccess = disposable = input.ToFileAccess ( );
                var document   = FPDF_LoadCustomDocument ( fileAccess, null );

                if ( document is not null )
                    disposable = null;

                return document?.AsDisposable ( document =>
                {
                    FPDF_CloseDocument ( document );
                    fileAccess.Dispose ( );
                } );
            }
            finally
            {
                disposable?.Dispose ( );
            }
        }
    }
}