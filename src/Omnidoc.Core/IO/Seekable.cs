using System;
using System.IO;

namespace Omnidoc.IO
{
    public static class Seekable
    {
        public static Stream AsSeekable ( this Stream stream )
        {
            if ( stream is null )
                throw new ArgumentNullException ( nameof ( stream ) );

            return stream.CanSeek ? stream : stream.ToSeekable ( );
        }

        public static Stream AsSeekable ( this Stream stream, int bufferSize )
        {
            if ( stream is null )
                throw new ArgumentNullException ( nameof ( stream ) );

            return stream.CanSeek ? stream : stream.ToSeekable ( bufferSize );
        }

        public static Stream AsSeekable ( this Stream stream, Stream buffer )
        {
            if ( stream is null )
                throw new ArgumentNullException ( nameof ( stream ) );

            return stream.CanSeek ? stream : stream.ToSeekable ( buffer );
        }

        public static Stream ToSeekable ( this Stream stream )
        {
            if ( stream is null )
                throw new ArgumentNullException ( nameof ( stream ) );

            return new SeekableReadOnlyStream ( stream );
        }

        public static Stream ToSeekable ( this Stream stream, int bufferSize )
        {
            if ( stream is null )
                throw new ArgumentNullException ( nameof ( stream ) );

            return new SeekableReadOnlyStream ( stream, bufferSize );
        }

        public static Stream ToSeekable ( this Stream stream, Stream buffer )
        {
            if ( stream is null )
                throw new ArgumentNullException ( nameof ( stream ) );

            return new SeekableReadOnlyStream ( stream, buffer );
        }
    }
}