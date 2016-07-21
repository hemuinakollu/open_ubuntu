using ddtek.d2ccore.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ddtek.d2ccore.Utility
{
    public class UtilStreamBuffer
    {
        static protected byte[] buf;
        static protected int count;

        public class BufferOutputStream : Stream
        {
            public override bool CanRead
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public override bool CanSeek
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public override bool CanWrite
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public override long Length
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public override long Position
            {
                get
                {
                    throw new NotImplementedException();
                }

                set
                {
                    throw new NotImplementedException();
                }
            }

            public BufferOutputStream()
            {
            }
            public void write(int b)
            {
                int newcount = count + 1;
                if (newcount > buf.Length)
                {
                    byte[] newbuf = new byte[Math.Max(buf.Length << 1, newcount)];
                    Array.Copy(buf, 0, newbuf, 0, count);
                    buf = newbuf;
                }
                buf[count] = (byte)b;
                count = newcount;
            }


            public void write(byte[] b, int off, int len)
            {
                if ((off < 0) || (off > b.Length) || (len < 0) ||
                      ((off + len) > b.Length) || ((off + len) < 0))
                {
                    throw new Exception("This error is kept by sravan in UtilBuffStream");
                }
                else if (len == 0)
                {
                    return;
                }
                int newcount = count + len;
                if (newcount > buf.Length)
                {
                    byte[] newbuf = new byte[Math.Max(buf.Length << 1, newcount)];
                    Array.Copy(buf, 0, newbuf, 0, count);
                    buf = newbuf;
                }
                Array.Copy(b, off, buf, count, len);
                count = newcount;
            }

            //public void writeTo(OutputStream out)
            //{
            //      out.write(buf, 0, count);
            //}

            public void reset()
            {
                count = 0;
            }


            public byte[] toByteArray()
            {
                byte[] newbuf = new byte[count];
                Array.Copy(buf, 0, newbuf, 0, count);
                return newbuf;
            }


            public int size()
            {
                return count;
            }

            public override void Flush()
            {
                throw new NotImplementedException();
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                throw new NotImplementedException();
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                throw new NotImplementedException();
            }

            public override void SetLength(long value)
            {
                throw new NotImplementedException();
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                throw new NotImplementedException();
            }
        }

        public class BufferInputStream : Stream
        {
            //        /**
            //		 * The index of the next character to read from the input stream buffer.
            //		 * This value should always be nonnegative
            //		 * and not larger than the value of <code>count</code>.
            //		 * The next byte to be read from the input stream buffer
            //		 * will be <code>buf[pos]</code>.
            //		 */
            protected int pos;

            //    /**
            //     * The currently marked position in the stream.
            //     * ByteArrayInputStream objects are marked at position zero by
            //     * default when constructed.  They may be marked at another
            //     * position within the buffer by the <code>mark()</code> method.
            //     * The current buffer position is set to this point by the
            //     * <code>reset()</code> method.
            //     * <p>
            //     * If no mark has been set, then the value of mark is the offset
            //     * passed to the constructor (or 0 if the offset was not supplied).
            //     *
            //     * @since   JDK1.1
            //     */
            protected int mark = 0;

            UtilStreamBufferPool bufferPool;

            public override bool CanRead
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public override bool CanSeek
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public override bool CanWrite
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public override long Length
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public override long Position
            {
                get
                {
                    throw new NotImplementedException();
                }

                set
                {
                    throw new NotImplementedException();
                }
            }

            public BufferInputStream()
            {
                bufferPool = null;
            }

            //private BufferInputStream(UtilStreamBufferPool bufferpool)
            //{
            //    this.bufferPool = bufferpool;
            //}

            public void initialize()
            {
                mark = 0;
                pos = 0;
            }

            //    /**
            //     * Reads the next byte of data from this input stream. The value
            //     * byte is returned as an <code>int</code> in the range
            //     * <code>0</code> to <code>255</code>. If no byte is available
            //     * because the end of the stream has been reached, the value
            //     * <code>-1</code> is returned.
            //     * <p>
            //     * This <code>read</code> method
            //     * cannot block.
            //     *
            //     * @return  the next byte of data, or <code>-1</code> if the end of the
            //     *          stream has been reached.
            //     */
            public int read()
            {
                return (pos < count) ? (buf[pos++] & 0xff) : -1;
            }

            //    /**
            //     * Reads up to <code>len</code> bytes of data into an array of bytes
            //     * from this input stream.
            //     * If <code>pos</code> equals <code>count</code>,
            //     * then <code>-1</code> is returned to indicate
            //     * end of file. Otherwise, the  number <code>k</code>
            //     * of bytes read is equal to the smaller of
            //     * <code>len</code> and <code>count-pos</code>.
            //     * If <code>k</code> is positive, then bytes
            //     * <code>buf[pos]</code> through <code>buf[pos+k-1]</code>
            //     * are copied into <code>b[off]</code>  through
            //     * <code>b[off+k-1]</code> in the manner performed
            //     * by <code>System.arraycopy</code>. The
            //     * value <code>k</code> is added into <code>pos</code>
            //     * and <code>k</code> is returned.
            //     * <p>
            //     * This <code>read</code> method cannot block.
            //     *
            //     * @param   b     the buffer into which the data is read.
            //     * @param   off   the start offset of the data.
            //     * @param   len   the maximum number of bytes read.
            //     * @return  the total number of bytes read into the buffer, or
            //     *          <code>-1</code> if there is no more data because the end of
            //     *          the stream has been reached.
            //     */
                public int read(byte[] b, int off, int len)
                {
                    if (b == null)
                    {
                        throw new Exception();
                    }
                    else if ((off < 0) || (off > b.Length) || (len < 0) ||
                             ((off + len) > b.Length) || ((off + len) < 0))
                    {
                        throw new Exception();
                    }
                    if (pos >= count)
                    {
                        return -1;
                    }
                    if (pos + len > count)
                    {
                        len = count - pos;
                    }
                    if (len <= 0)
                    {
                        return 0;
                    }
                    Array.Copy(buf, pos, b, off, len);
                    pos += len;
                    return len;
                }

            //    /**
            //     * Skips <code>n</code> bytes of input from this input stream. Fewer
            //     * bytes might be skipped if the end of the input stream is reached.
            //     * The actual number <code>k</code>
            //     * of bytes to be skipped is equal to the smaller
            //     * of <code>n</code> and  <code>count-pos</code>.
            //     * The value <code>k</code> is added into <code>pos</code>
            //     * and <code>k</code> is returned.
            //     *
            //     * @param   n   the number of bytes to be skipped.
            //     * @return  the actual number of bytes skipped.
            //     */
                //public long skip(long n)
                //{
                //    if (pos + n > count)
                //    {
                //        n = count - pos;
                //    }
                //    if (n < 0)
                //    {
                //        return 0;
                //    }
                //    pos = pos + n;
                //    return n;
                //}

            //    /**
            //     * Returns the number of bytes that can be read from this input
            //     * stream without blocking.
            //     * The value returned is
            //     * <code>count&nbsp;- pos</code>,
            //     * which is the number of bytes remaining to be read from the input buffer.
            //     *
            //     * @return  the number of bytes that can be read from the input stream
            //     *          without blocking.
            //     */
                public int available()
                {
                    return count - pos;
                }

            //    /**
            //     * Tests if this <code>InputStream</code> supports mark/reset. The
            //     * <code>markSupported</code> method of <code>ByteArrayInputStream</code>
            //     * always returns <code>true</code>.
            //     *
            //     * @since   JDK1.1
            //     */
                public bool markSupported()
                {
                    return true;
                }

            public override void Flush()
            {
                throw new NotImplementedException();
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                throw new NotImplementedException();
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                throw new NotImplementedException();
            }

            public override void SetLength(long value)
            {
                throw new NotImplementedException();
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                throw new NotImplementedException();
            }

            //    /**
            //     * Set the current marked position in the stream.
            //     * ByteArrayInputStream objects are marked at position zero by
            //     * default when constructed.  They may be marked at another
            //     * position within the buffer by this method.
            //     * <p>
            //     * If no mark has been set, then the value of the mark is the
            //     * offset passed to the constructor (or 0 if the offset was not
            //     * supplied).
            //     *
            //     * <p> Note: The <code>readAheadLimit</code> for this class
            //     *  has no meaning.
            //     *
            //     * @since   JDK1.1
            //     */
            //public void mark(int readAheadLimit)
            //{
            //    mark = pos;
            //}

            //    /**
            //     * Resets the buffer to the marked position.  The marked position
            //     * is 0 unless another position was marked or an offset was specified
            //     * in the constructor.
            //     */
            //public  void reset()
            //{
            //    pos = mark;
            //}

            //    /**
            //     * Closing a <tt>ByteArrayInputStream</tt> has no effect. The methods in
            //     * this class can be called after the stream has been closed without
            //     * generating an <tt>IOException</tt>.
            //     * <p>
            //     */
            //public void close()
            //{
            //    if (bufferPool != null)
            //    {
            //        bufferPool.putBigBuffer(UtilStreamBuffer.this);
            //    }
            //}
        }

            private BufferInputStream inputStream;
            private BufferOutputStream outputStream;

            public  bool CanRead
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public  bool CanSeek
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public  bool CanWrite
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public  long Length
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public  long Position
            {
                get
                {
                    throw new NotImplementedException();
                }

                set
                {
                    throw new NotImplementedException();
                }
            }

        public UtilStreamBuffer(int initialsize)
        {
            buf = new byte[initialsize];
            inputStream = new BufferInputStream();
            outputStream = new BufferOutputStream();
        }

        public Stream getOutputStream()
            {
                count = 0;
                return outputStream;
            }

        ///**
        // * This version returns a recycled input stream interface.  Should never be given to end users directly.
        // * @return
        // */
        public Stream getInputStream()
        {
            inputStream.initialize();
            return inputStream;
        }

        ///**
        // * This version of the input stream will put the stream buffer back in the buffer pool when it is closed.
        // * @param bufferPool - buffer pool to return stream buffer to when finished
        // * @return
        // */
        //public InputStream getInputStream(UtilStreamBufferPool bufferPool)
        //{
        //    return new BufferInputStream(bufferPool);
        //}

        //private static final int BUFFER_INCREMENT_SIZE = 1024 * 64; // 64 KB

        //public InputStream load(InputStream source) 
        //{
        //		this.inputStream.initialize();

        //		int offset = 0;
        //		int bytesRead = source.read(buf, 0, buf.length);
        //		while (bytesRead > 0) {
        //        offset += bytesRead;
        //        int bufferSpace = buf.length - offset;
        //        if (bufferSpace == 0)
        //        {
        //            byte[] newBuffer = new byte[buf.length + BUFFER_INCREMENT_SIZE];
        //            System.arraycopy(buf, 0, newBuffer, 0, buf.length);


        //            buf = newBuffer;
        //        }
        //        bufferSpace = buf.length - offset;
        //        bytesRead = source.read(buf, offset, bufferSpace);
        //    }

        //    count = offset;

        //		return this.inputStream;
        //}

        public byte[] getBuffer()
            {
                return buf;
            }

            public byte[] getBuffer(int length)
            {
                if (buf.Length < length)
                {
                    buf = new byte[length];
                }
                return buf;
            }

            public int getSize()
            {
                return count;
            }

            public  void Flush()
            {
                throw new NotImplementedException();
            }

            public  int Read(byte[] buffer, int offset, int count)
            {
                throw new NotImplementedException();
            }

            public  long Seek(long offset, SeekOrigin origin)
            {
                throw new NotImplementedException();
            }

            public  void SetLength(long value)
            {
                throw new NotImplementedException();
            }

            public  void Write(byte[] buffer, int offset, int count)
            {
                throw new NotImplementedException();
            }

            //public void setSize(int size)
            //{
            //    count = size;
            //}

            //    public string decode(UtilTransliterator transliterator) 
            //		    return transliterator.decode(buf, 0, count);   
        }
    }
