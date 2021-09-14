using System;
using System.IO;

using PDFiumCore;
using PDFiumCore.Delegates;

namespace Omnidoc.Pdf
{
    public static class PdfStream
    {
        public static FPDF_FILEACCESS ToFileAccess ( this Stream stream, long? length = null )
        {
            if ( stream is null )
                throw new ArgumentNullException ( nameof ( stream ) );

            return new FPDF_FILEACCESS ( )
            {
                MFileLen  = (uint) ( length ?? stream.Length - stream.Position ),
                MGetBlock = stream.ToFileAccessBlock ( ),
                MParam    = IntPtr.Zero
            };
        }

        private static unsafe Func_int___IntPtr_uint_bytePtr_uint ToFileAccessBlock ( this Stream stream )
        {
            var start = stream.Position;

            return (_, position, buffer, size) =>
            {
                stream.Position = start + position;

                return stream.Read ( new Span < byte > ( buffer, (int) size ) ) == size ? (int) size : 0;
            };
        }
    }
}