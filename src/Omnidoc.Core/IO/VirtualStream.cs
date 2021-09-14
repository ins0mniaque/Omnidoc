using System;
using System.IO;

namespace Omnidoc.IO
{
    /// <summary>
    /// Implements a virtual stream, i.e. the always seekable stream which
    /// uses configurable amount of memory to reduce a memory footprint and
    /// temporarily stores remaining data in a temporary file on disk.
    /// </summary>
    public sealed class VirtualStream : Stream
    {
        /// <summary>
        /// Memory handling
        /// </summary>
        public enum MemoryFlag
        {
            AutoOverFlowToDisk = 0,
            OnlyInMemory       = 1,
            OnlyToDisk         = 2
        }

        private const int MemoryThreshold   = 4 * 1024 * 1024; // The maximum possible memory consumption (4Mb)
        private const int DefaultMemorySize = 4 * 1024;        // Default memory consumption (4Kb)
        private const int DefaultBufferSize = 4 * 1024;        // Default memory consumption (4Kb)

        private Stream     wrappedStream;
        private bool       isDisposed;
        private bool       isInMemory;
        private int        thresholdSize;
        private MemoryFlag memoryFlag;

        /// <summary>
        /// Initializes a VirtualStream instance with default parameters (4K memory buffer,
        /// allow overflow to disk).
        /// </summary>
        public VirtualStream ( )
            : this ( DefaultMemorySize, MemoryFlag.AutoOverFlowToDisk, new MemoryStream ( ) )
        {
        }

        /// <summary>
        /// Initializes a VirtualStream instance with memory buffer size.
        /// </summary>
        /// <param name="bufferSize">Memory buffer size</param>
        public VirtualStream ( int bufferSize )
            : this ( bufferSize, MemoryFlag.AutoOverFlowToDisk, new MemoryStream ( bufferSize ) )
        {
        }

        /// <summary>
        /// Initializes a VirtualStream instance with a default memory size and memory flag specified.
        /// </summary>
        /// <param name="flag">Memory flag</param>
        public VirtualStream ( MemoryFlag flag )
            : this ( DefaultMemorySize, flag,
            ( flag == MemoryFlag.OnlyToDisk ) ? CreateVirtualMemoryStream ( ) : new MemoryStream ( ) )
        {
        }

        /// <summary>
        /// Initializes a VirtualStream instance with a memory buffer size and memory flag specified.
        /// </summary>
        /// <param name="bufferSize">Memory buffer size</param>
        /// <param name="flag">Memory flag</param>
        public VirtualStream ( int bufferSize, MemoryFlag flag )
            : this ( bufferSize, flag,
            ( flag == MemoryFlag.OnlyToDisk ) ? CreateVirtualMemoryStream ( ) : new MemoryStream ( bufferSize ) )
        {
        }

        /// <summary>
        /// Creates a FileStream with a unique name and the temporary and delete-on-close attributes.
        /// </summary>
        /// <returns>
        /// The virtual memory stream
        /// </returns>
        public static Stream CreateVirtualMemoryStream ( int bufferSize = DefaultBufferSize )
        {
            return new FileStream ( Path.GetTempFileName ( ), FileMode.Create, FileAccess.ReadWrite, FileShare.None, bufferSize, FileOptions.DeleteOnClose );
        }

        /// <summary>
        /// Initializes a VirtualStream instance with a memory buffer size, memory flag and underlying stream
        /// specified.
        /// </summary>
        /// <param name="bufferSize">Memory buffer size</param>
        /// <param name="flag">Memory flag</param>
        /// <param name="dataStream">Underlying stream</param>
        private VirtualStream ( int bufferSize, MemoryFlag flag, Stream dataStream )
        {
            if ( null == dataStream )
                throw new ArgumentNullException ( nameof ( dataStream ) );

            isInMemory = ( flag != MemoryFlag.OnlyToDisk );
            memoryFlag = flag;
            bufferSize = Math.Min ( bufferSize, MemoryThreshold );
            thresholdSize = bufferSize;

            if ( isInMemory )
                wrappedStream = dataStream;  // Don't want to double wrap memory stream
            else
                wrappedStream = new BufferedStream ( dataStream, bufferSize );
        }

        /// <summary>
        /// Gets a flag indicating whether a stream can be read.
        /// </summary>
        public override bool CanRead
        {
            get { return wrappedStream.CanRead; }
        }
        /// <summary>
        /// Gets a flag indicating whether a stream can be written.
        /// </summary>
        public override bool CanWrite
        {
            get { return wrappedStream.CanWrite; }
        }

        /// <summary>
        /// Gets a flag indicating whether a stream can seek.
        /// </summary>
        public override bool CanSeek
        {
            get { return true; }
        }

        /// <summary>
        /// Returns the length of the source stream.
        /// <seealso cref="GetLength()" />
        /// </summary>
        public override long Length
        {
            get { return wrappedStream.Length; }
        }

        /// <summary>
        /// Gets or sets a position in the stream.
        /// </summary>
        public override long Position
        {
            get { return wrappedStream.Position; }
            set { wrappedStream.Seek ( value, SeekOrigin.Begin ); }
        }

        /// <summary>
        /// <see cref="Stream.Close()" />
        /// </summary>
        /// <remarks>
        /// Calling other methods after calling Close() may result in a ObjectDisposedException beeing throwed.
        /// </remarks>
        public override void Close ( )
        {
            if ( ! isDisposed )
            {
                isDisposed = true;
                wrappedStream.Close ( );
            }

            base.Close ( );
        }

        /// <summary>
        /// <see cref="Stream.Flush()" />
        /// </summary>
        /// <remarks>
        /// </remarks>
        public override void Flush ( )
        {
            ThrowIfDisposed ( );

            wrappedStream.Flush ( );
        }

        /// <summary>
        /// <see cref="Stream.Read()" />
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns>
        /// The number of bytes read
        /// </returns>
        /// <remarks>
        /// May throw <see cref="ObjectDisposedException" />.
        /// It will read from cached persistence stream
        /// </remarks>
        public override int Read ( byte [ ] buffer, int offset, int count )
        {
            ThrowIfDisposed ( );

            return wrappedStream.Read ( buffer, offset, count );
        }

        /// <summary>
        /// <see cref="Stream.Seek()" />
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="origin"></param>
        /// <returns>
        /// The current position
        /// </returns>
        /// <remarks>
        /// May throw <see cref="ObjectDisposedException" />.
        /// It will cache any new data into the persistence stream
        /// </remarks>
        public override long Seek ( long offset, SeekOrigin origin )
        {
            ThrowIfDisposed ( );

            return wrappedStream.Seek ( offset, origin );
        }

        /// <summary>
        /// <see cref="Stream.SetLength()" />
        /// </summary>
        /// <param name="length"></param>
        /// <remarks>
        /// May throw <see cref="ObjectDisposedException" />.
        /// </remarks>
        public override void SetLength ( long length )
        {
            ThrowIfDisposed ( );

            // Check if new position is greater than allowed by threshold
            if ( memoryFlag == MemoryFlag.AutoOverFlowToDisk &&
                isInMemory &&
                length > thresholdSize )
            {
                // Currently in memory, and the new write will push it over the limit
                // Switching to Persist Stream
                var persistStream = new BufferedStream ( CreateVirtualMemoryStream ( ), thresholdSize );

                // Copy current wrapped memory stream to the persist stream
                CopyStreamContent ( (MemoryStream) wrappedStream, persistStream );

                // Close old wrapped stream
                wrappedStream.Close ( );

                wrappedStream = persistStream;
                isInMemory = false;
            }

            // Set new length for the wrapped stream
            wrappedStream.SetLength ( length );
        }

        /// <summary>
        /// <see cref="Stream.Write()" />
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// </summary>
        /// <remarks>
        /// Write to the underlying stream.
        /// </remarks>
        public override void Write ( byte [ ] buffer, int offset, int count )
        {
            ThrowIfDisposed ( );

            // Check if new position after write is greater than allowed by threshold
            if ( memoryFlag == MemoryFlag.AutoOverFlowToDisk &&
                isInMemory &&
                ( count + wrappedStream.Position ) > thresholdSize )
            {
                // Currently in memory, and the new write will push it over the limit
                // Switching to Persist Stream
                var persistStream = new BufferedStream ( CreateVirtualMemoryStream ( ), thresholdSize );

                // Copy current wrapped memory stream to the persist stream
                CopyStreamContent ( (MemoryStream) wrappedStream, persistStream );

                // Close old wrapped stream
                wrappedStream.Close ( );

                wrappedStream = persistStream;
                isInMemory = false;
            }

            wrappedStream.Write ( buffer, offset, count );
        }

        /// <summary>
        /// Copies source memory stream to the target stream.
        /// </summary>
        /// <param name="source">Source memory stream</param>
        /// <param name="target">Target stream</param>
        private void CopyStreamContent ( MemoryStream source, Stream target )
        {
            // Remember position for the source stream
            var currentPosition = source.Position;

            // Read and write in chunks each thresholdSize
            var tempBuffer = new Byte[thresholdSize];
            var read = 0;

            source.Position = 0;
            while ( ( read = source.Read ( tempBuffer, 0, tempBuffer.Length ) ) != 0 )
                target.Write ( tempBuffer, 0, read );

            // Set target's stream position to be the same as was in source stream. This is required because 
            // target stream is going substitute source stream.
            target.Position = currentPosition;

            // Restore source stream's position (just in case to preserve the source stream's state)
            source.Position = currentPosition;
        }

        /// <summary>
        /// Called by other methods to check the stream state.
        /// It will throw <see cref="ObjectDisposedException" /> if the stream was closed or disposed.
        /// </summary>
        private void ThrowIfDisposed ( )
        {
            if ( isDisposed )
                throw new ObjectDisposedException ( nameof ( VirtualStream ) );
        }
    }
}