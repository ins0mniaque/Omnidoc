using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

using PDFiumCore;

using Omnidoc.Core.Disposables;

namespace Omnidoc.Pdf
{
    using static PDFiumCore.fpdfview;

    public static class PdfRenderer
    {
        public static void Render ( this FpdfPageT page, Stream output, RenderingOptions options )
        {
            if ( page    is null ) throw new ArgumentNullException ( nameof ( page    ) );
            if ( output  is null ) throw new ArgumentNullException ( nameof ( output  ) );
            if ( options is null ) throw new ArgumentNullException ( nameof ( options ) );

            var dpi    = options.Dpi ?? 96;
            var width  = ( options.Width  ?? FPDF_GetPageWidth  ( page ) ) / 72.0 * dpi;
            var height = ( options.Height ?? FPDF_GetPageHeight ( page ) ) / 72.0 * dpi;

            using var image = FPDFBitmapCreate ( (int) width, (int) height, ( options.Alpha ?? false ) ? 1 : 0 ).AsDisposable ( FPDFBitmapDestroy );

            FPDF_RenderPageBitmap ( image, page, 0, 0, (int) width, (int) height, 0, 0 );

            using var bitmap = new PdfBitmapStream ( image, dpi, dpi, options.Alpha ?? false );

            bitmap.CopyTo ( output );
        }

        public static Task RenderAsync ( this FpdfPageT page, Stream output, RenderingOptions options, CancellationToken cancellationToken = default )
        {
            return Task.Run ( ( ) => page.Render ( output, options ), cancellationToken );
        }

        public static void Render ( this Disposable < FpdfPageT > page, Stream output, RenderingOptions options )
        {
            if ( page is null ) throw new ArgumentNullException ( nameof ( page ) );

            page.Unwrap ( ).Render ( output, options );
        }

        public static Task RenderAsync ( this Disposable < FpdfPageT > page, Stream output, RenderingOptions options, CancellationToken cancellationToken = default )
        {
            if ( page is null ) throw new ArgumentNullException ( nameof ( page ) );

            return page.Unwrap ( ).RenderAsync ( output, options, cancellationToken );
        }

        private class PdfBitmapStream : Stream
        {
            private const uint BytesPerPixel     = 4;
            private const uint BmpHeaderSize     = 14;
            private const uint DibHeaderSize     = 108;
            private const uint PixelArrayOffset  = BmpHeaderSize + DibHeaderSize;
            private const uint CompressionMethod = 3;

            private const uint MaskR = 0x00_FF_00_00;
            private const uint MaskG = 0x00_00_FF_00;
            private const uint MaskB = 0x00_00_00_FF;
            private const uint MaskA = 0xFF_00_00_00;

            private readonly IntPtr   scan0;
            private readonly bool     sameStride;
            private readonly byte [ ] header;
            private readonly uint     length;
            private readonly uint     stride;
            private readonly uint     rowLength;
            private          uint     position;

            public PdfBitmapStream ( FpdfBitmapT bitmap, double dpiX, double dpiY, bool alpha )
            {
                var width  = (uint) FPDFBitmapGetWidth  ( bitmap );
                var height = (uint) FPDFBitmapGetHeight ( bitmap );

                scan0      = FPDFBitmapGetBuffer ( bitmap );
                rowLength  = BytesPerPixel * width;
                stride     = ( rowLength * 8 + 31 ) / 32 * 4;
                length     = PixelArrayOffset + stride * height;
                header     = GenerateHeader ( length, width, height, dpiX, dpiY, alpha );
                sameStride = stride == FPDFBitmapGetStride ( bitmap );
            }

            private static byte [ ] GenerateHeader ( uint fileSize, uint width, uint height, double dpiX, double dpiY, bool alpha )
            {
                const double MetersPerInch = 0.0254;

                var header = new byte [ BmpHeaderSize + DibHeaderSize ];

                using var stream = new MemoryStream ( header );
                using var writer = new BinaryWriter ( stream );

                writer.Write ( (byte) 'B' );
                writer.Write ( (byte) 'M' );
                writer.Write ( fileSize );
                writer.Write ( 0u );
                writer.Write ( PixelArrayOffset );
                writer.Write ( DibHeaderSize );
                writer.Write ( width );
                writer.Write ( -height );
                writer.Write ( (ushort) 1 );
                writer.Write ( (ushort) ( BytesPerPixel * 8 ) );
                writer.Write ( CompressionMethod );
                writer.Write ( 0 );
                writer.Write ( (int) Math.Round ( dpiX / MetersPerInch ) );
                writer.Write ( (int) Math.Round ( dpiY / MetersPerInch ) );
                writer.Write ( 0L );
                writer.Write ( MaskR );
                writer.Write ( MaskG );
                writer.Write ( MaskB );

                if ( alpha )
                    writer.Write ( MaskA );

                return header;
            }

            public override bool CanRead  => true;
            public override bool CanSeek  => true;
            public override bool CanWrite => false;
            public override long Length   => length;

            public override long Position
            {
                get => position;
                set
                {
                    if ( value < 0 || value >= length )
                        throw new ArgumentOutOfRangeException ( nameof ( value ) );

                    position = (uint) value;
                }
            }

            public override int Read ( byte [ ] buffer, int offset, int count )
            {
                var bytesToRead = count;
                var bytesRead   = 0;

                if ( position < PixelArrayOffset )
                {
                    bytesRead = Math.Min ( count, (int) ( PixelArrayOffset - position ) );

                    Buffer.BlockCopy ( header, (int) position, buffer, offset, bytesRead );

                    position    += (uint) bytesRead;
                    offset      += bytesRead;
                    bytesToRead -= bytesRead;
                }

                if ( bytesToRead <= 0 )
                    return bytesRead;

                bytesToRead = Math.Min ( bytesToRead, (int) ( length - position ) );

                var scanOffset = position - PixelArrayOffset;

                if ( sameStride )
                {
                    Marshal.Copy ( scan0 + (int) scanOffset, buffer, offset, bytesToRead );

                    bytesRead += bytesToRead;
                    position  += (uint) bytesToRead;

                    return bytesRead;
                }

                while ( bytesToRead > 0 )
                {
                    var column    = (int) ( scanOffset / stride );
                    var leftInRow = Math.Max ( 0, (int) rowLength - column );
                    var padding   = (int) ( stride - rowLength );

                    var read = Math.Min ( bytesToRead, leftInRow );
                    if ( read > 0 )
                        Marshal.Copy ( scan0 + (int) scanOffset, buffer, offset, read );

                    offset      += read;
                    scanOffset  += (uint) read;
                    bytesToRead -= read;
                    bytesRead   += read;

                    read = Math.Min ( bytesToRead, padding );
                    for ( var index = 0; index < read; index++ )
                        buffer [ offset + index ] = 0;

                    offset      += read;
                    scanOffset  += (uint) read;
                    bytesToRead -= read;
                    bytesRead   += read;
                }

                position = PixelArrayOffset + scanOffset;

                return bytesRead;
            }

            public override long Seek ( long offset, SeekOrigin origin )
            {
                if      ( origin == SeekOrigin.Begin   ) Position  = offset;
                else if ( origin == SeekOrigin.Current ) Position += offset;
                else if ( origin == SeekOrigin.End     ) Position  = Length + offset;

                return Position;
            }

            public override void SetLength ( long value )                             => throw new NotSupportedException ( );
            public override void Write     ( byte [ ] buffer, int offset, int count ) => throw new NotSupportedException ( );
            public override void Flush     ( ) { }
        }
    }
}