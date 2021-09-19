using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Omnidoc.Pdf
{
    public static class PdfString
    {
        public delegate int Handler        ( IntPtr     buffer, int length );
        public delegate int UnicodeHandler ( ref ushort buffer, int length );

        public static string Alloc ( Handler handler, Encoding encoding )
        {
            if ( handler  is null ) throw new ArgumentNullException ( nameof ( handler  ) );
            if ( encoding is null ) throw new ArgumentNullException ( nameof ( encoding ) );

            var length = handler ( IntPtr.Zero, 0 );
            if ( length == 0 )
                return string.Empty;

            var buffer = new byte [ length ];
            var handle = GCHandle.Alloc ( buffer, GCHandleType.Pinned );

            try     { handler ( handle.AddrOfPinnedObject ( ), length ); }
            finally { handle.Free ( ); }

            return encoding.GetString ( buffer, 0, length );
        }

        public static string Alloc ( UnicodeHandler handler )
        {
            if ( handler is null )
                throw new ArgumentNullException ( nameof ( handler ) );

            var zero   = (ushort) 0;
            var length = handler ( ref zero, 0 );
            if ( length == 0 )
                return string.Empty;

            var buffer = new ushort [ length ];

            handler ( ref buffer [ 0 ], length );

            unsafe
            {
                fixed ( ushort* data = &buffer [ 0 ] )
                    return new string ( (char*) data, 0, buffer.Length );
            }
        }
    }
}