using DDInt.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ddtek.d2ccore.Common
{
    public class BinaryMessageReader : BinaryDataInput
    {


//        private readonly static string END_OF_STREAM_MSG = "unexpected end of stream reached";

//        private byte[] byteBuffer = new byte[32768];
//        private char[] charBuffer = new char[32768];
          private readonly UtilTransliterator transliterator;
	      private Stream inputStream;

        public int CONTINUE
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

        public int ERROR
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

        public int LAST
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

        public int NULL
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

        public int REPEAT
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

        public void close()
        {
            throw new NotImplementedException();
        }

        public bool readBoolean()
        {
            throw new NotImplementedException();
        }

        public byte readByte()
        {
            throw new NotImplementedException();
        }

        public int readCompressedInt()
        {
            throw new NotImplementedException();
        }

        public long readCompressedLong()
        {
            throw new NotImplementedException();
        }

        public double readDouble()
        {
            throw new NotImplementedException();
        }

        public float readFloat()
        {
            throw new NotImplementedException();
        }

        public int readInt()
        {
            throw new NotImplementedException();
        }

        public long readLong()
        {
            throw new NotImplementedException();
        }

        public short readShort()
        {
            throw new NotImplementedException();
        }

        public string readString()
        {
            throw new NotImplementedException();
        }

        public string readString(int len)
        {
            throw new NotImplementedException();
        }

        public int readUnsignedByte()
        {
            throw new NotImplementedException();
        }

                private int totalBytesRead;

        //        // For compressed integers and longs

        //        public static readonly int MAX_POSITIVE = 236;
        //        public static readonly int MAX_NEGATIVE = -3;
        //        public static readonly int OFFSET = 19;

        //        public static readonly int MAX_1_BYTE = MAX_POSITIVE + 0x100;
        //        public static readonly int MAX_2_BYTE = MAX_1_BYTE + 0x10000;
        //        public static readonly int MAX_3_BYTE = MAX_2_BYTE + 0x1000000;

        //        public static readonly long MAX_4_BYTE = MAX_3_BYTE + 0x100000000L;
        //        public static readonly long MAX_5_BYTE = MAX_4_BYTE + 0x10000000000L;
        //        public static readonly long MAX_6_BYTE = MAX_5_BYTE + 0x1000000000000L;
        //        public static readonly long MAX_7_BYTE = MAX_6_BYTE + 0x100000000000000L;

        //        public static readonly int MIN_1_BYTE = 0x100 - MAX_NEGATIVE;
        //        public static readonly int MIN_2_BYTE = MIN_1_BYTE + 0x10000;
        //        public static readonly int MIN_3_BYTE = MIN_2_BYTE + 0x1000000;

        //        public static readonly long MIN_4_BYTE = MIN_3_BYTE + 0x100000000L;
        //        public static readonly long MIN_5_BYTE = MIN_4_BYTE + 0x10000000000L;
        //        public static readonly long MIN_6_BYTE = MIN_5_BYTE + 0x1000000000000L;
        //        public static readonly long MIN_7_BYTE = MIN_6_BYTE + 0x100000000000000L;

        //        public int LAST
        //        {
        //            get
        //            {
        //                throw new NotImplementedException();
        //            }

        //            set
        //            {
        //                throw new NotImplementedException();
        //            }
        //        }

        //        public int NULL
        //        {
        //            get
        //            {
        //                throw new NotImplementedException();
        //            }

        //            set
        //            {
        //                throw new NotImplementedException();
        //            }
        //        }

        //        public int ERROR
        //        {
        //            get
        //            {
        //                throw new NotImplementedException();
        //            }

        //            set
        //            {
        //                throw new NotImplementedException();
        //            }
        //        }

        //        public int REPEAT
        //        {
        //            get
        //            {
        //                throw new NotImplementedException();
        //            }

        //            set
        //            {
        //                throw new NotImplementedException();
        //            }
        //        }

        //        public int CONTINUE
        //        {
        //            get
        //            {
        //                throw new NotImplementedException();
        //            }

        //            set
        //            {
        //                throw new NotImplementedException();
        //            }
        //        }

        public BinaryMessageReader(UtilTransliterator transliterator)
        {
            this.transliterator = transliterator;
        }

        public void initialize(Stream inputStream)
        {
            this.inputStream = inputStream;
            totalBytesRead = 0;
        }

        //        public bool readBoolean() 
        //        {

        //                int b = inputStream.read();
        //		        ++totalBytesRead;
        //		        if (b< 0) {
        //			        throw new Exception(END_OF_STREAM_MSG);
        //                }
        //		return b != 0;
        //	    }

        //public byte readByte()
        //{
        //		int b = inputStream.read();
        //		++totalBytesRead;
        //		if (b < 0) {
        //        throw new Exception(END_OF_STREAM_MSG);
        //    }
        //		return (byte)b;
        //}

        //public int readUnsignedByte() 
        //{
        //		int b = inputStream.read();
        //		++totalBytesRead;
        //		if (b < 0) {
        //        throw new Exception(END_OF_STREAM_MSG);
        //    }
        //		return b;
        //}

        //public short readShort() 
        //{
        //		int b1 = inputStream.read();
        //		int b2 = inputStream.read();
        //    totalBytesRead += 2;
        //		if ((b1 | b2) < 0) {
        //        throw new Exception(END_OF_STREAM_MSG);
        //    }

        //		return (short) ((b1 << 8) + b2);
        //}

        //public int readInt() 
        //{
        //        int b1 = inputStream.read();
        //        int b2 = inputStream.read();
        //        int b3 = inputStream.read;
        //        int b4 = inputStream.Read();
        //    totalBytesRead += 4;
        //        if ((b1 | b2 | b3 | b4) < 0) {
        //        throw new Exception(END_OF_STREAM_MSG);
        //    }

        //        return ((b1 << 24) + (b2 << 16) + (b3 << 8) + (b4 << 0));
        //}

        //private int readCompressedInt(int length) 
        //{
        //		int	val;

        //		if (length <= 7) {
        //        if (length == 0)
        //        {
        //            val = readUnsignedByte() + (MAX_POSITIVE + 1);
        //        }
        //        else if (length == 1)
        //        {
        //            val = (readUnsignedByte() << 8) + readUnsignedByte() + (MAX_1_BYTE + 1);
        //        }
        //        else if (length == 2)
        //        {
        //            val = (readUnsignedByte() << 16) + (readUnsignedByte() << 8) +
        //                  readUnsignedByte() + (MAX_2_BYTE + 1);
        //        }
        //        else if (length == 3)
        //        {
        //            val = (readUnsignedByte() << 24) + (readUnsignedByte() << 16) +
        //                  (readUnsignedByte() << 8) + readUnsignedByte() + (MAX_3_BYTE + 1);
        //            // Check for overflow?
        //        }
        //        else
        //        {
        //            throw new Exception("overflow reading compressed integer - length = " + length);
        //        }
        //    }
        //		else if (length == 8) {
        //        val = (MAX_NEGATIVE - 1) - readUnsignedByte();
        //    }
        //		else if (length == 9) {
        //        val = (-MIN_1_BYTE - 1) - ((readUnsignedByte() << 8) + readUnsignedByte());
        //    }
        //		else if (length == 10) {
        //        val = (-MIN_2_BYTE - 1) - ((readUnsignedByte() << 16) + (readUnsignedByte() << 8) + readUnsignedByte());
        //    }
        //		else if (length == 11) {
        //        val = (-MIN_3_BYTE - 1) - ((readUnsignedByte() << 24) + (readUnsignedByte() << 16) + (readUnsignedByte() << 8) + readUnsignedByte());
        //        // Check for overflow?
        //    }
        //		else {
        //        throw new Exception("overflow reading compressed integer - length = " + length);
        //    }

        //		return val;
        //}


        //public int readCompressedInt() 
        //{
        //		int length = readUnsignedByte();

        //		if (length >= (MAX_NEGATIVE + OFFSET)) {
        //        return (length - OFFSET);
        //    }

        //		return readCompressedInt(length);
        //}

        //public long readLong() 
        //{
        //    readBytes(byteBuffer, 0, 8);
        //		return (((long)byteBuffer [0] << 56) +
        //			  ((long)(byteBuffer [1] & 255) << 48) +
        //			  ((long)(byteBuffer [2] & 255) << 40) +
        //			  ((long)(byteBuffer [3] & 255) << 32) +
        //			  ((long)(byteBuffer [4] & 255) << 24) +
        //			  ((byteBuffer [5] & 255) << 16) +
        //			  ((byteBuffer [6] & 255) <<  8) +
        //			  ((byteBuffer [7] & 255)));
        //}

        //public long readCompressedLong() 
        //{

        //		long val;
        //            var buf = new byte[1024];
        //		//byte buf [];
        //		int	b = readUnsignedByte();

        //		if (b >= (MAX_NEGATIVE + OFFSET)) {
        //        return (b - OFFSET);
        //    }

        //		if ((b <= 2) || ((b >= 8) && (b <= 10))) {
        //        return readCompressedInt(b);
        //    }

        //		if (b <= 7) {
        //        readBytes(buf = byteBuffer, 0, b + 1);

        //        if (b == 3)
        //        {
        //            val = (((long)((buf[1] << 24) + ((buf[2] & 0xff) << 16) + ((buf[3] & 0xff) << 8) +
        //                    (buf[4] & 0xff))) & 0xffffffffL) + (MAX_3_BYTE + 1);
        //        }
        //        else if (b == 4)
        //        {
        //            val = (((long)((buf[2] << 24) + ((buf[3] & 0xff) << 16) + ((buf[4] & 0xff) << 8) +
        //                    (buf[5] & 0xff))) & 0xffffffffL) + (((long)(buf[1] & 0xff)) << 32) +
        //                  (MAX_4_BYTE + 1);
        //        }
        //        else if (b == 5)
        //        {
        //            val = (((long)((buf[3] << 24) + ((buf[4] & 0xff) << 16) + ((buf[5] & 0xff) << 8) +
        //                    (buf[6] & 0xff))) & 0xffffffffL) + (((long)(buf[2] & 0xff)) << 32) +
        //                  (((long)(buf[1] & 0xff)) << 40) + (MAX_5_BYTE + 1);
        //        }
        //        else if (b == 6)
        //        {
        //            val = (((long)((buf[4] << 24) + ((buf[5] & 0xff) << 16) + ((buf[6] & 0xff) << 8) +
        //                    (buf[7] & 0xff))) & 0xffffffffL) + (((long)(buf[3] & 0xff)) << 32) +
        //                  (((long)(buf[2] & 0xff)) << 40) + (((long)(buf[1] & 0xff)) << 48) + (MAX_6_BYTE + 1);
        //        }
        //        else
        //        {
        //            val = (((long)((buf[5] << 24) + ((buf[6] & 0xff) << 16) + ((buf[7] & 0xff) << 8) +
        //                    (buf[8] & 0xff))) & 0xffffffffL) + (((long)(buf[4] & 0xff)) << 32) +
        //                  (((long)(buf[3] & 0xff)) << 40) + (((long)(buf[2] & 0xff)) << 48) +
        //                  (((long)buf[1]) << 56) + (MAX_7_BYTE + 1);
        //        }
        //    }
        //		else {
        //        readBytes(buf = byteBuffer, 0, b - 7);

        //        if (b == 11)
        //        {
        //            val = (-MIN_3_BYTE - 1) - (((long)((buf[1] << 24) + ((buf[2] & 0xff) << 16) +
        //                ((buf[3] & 0xff) << 8) + (buf[4] & 0xff))) & 0xffffffffL);
        //        }
        //        else if (b == 12)
        //        {
        //            val = (-MIN_4_BYTE - 1) - (((long)((buf[2] << 24) + ((buf[3] & 0xff) << 16) +
        //                ((buf[4] & 0xff) << 8) + (buf[5] & 0xff))) & 0xffffffffL) -
        //                  (((long)(buf[1] & 0xff)) << 32);
        //        }
        //        else if (b == 13)
        //        {
        //            val = (-MIN_5_BYTE - 1) - (((long)((buf[3] << 24) + ((buf[4] & 0xff) << 16) +
        //                ((buf[5] & 0xff) << 8) + (buf[6] & 0xff))) & 0xffffffffL) -
        //                  (((long)(buf[2] & 0xff)) << 32) - (((long)(buf[1] & 0xff)) << 40);
        //        }
        //        else if (b == 14)
        //        {
        //            val = (-MIN_6_BYTE - 1) - (((long)((buf[4] << 24) + ((buf[5] & 0xff) << 16) +
        //                ((buf[6] & 0xff) << 8) + (buf[7] & 0xff))) & 0xffffffffL) -
        //                  (((long)(buf[3] & 0xff)) << 32) - (((long)(buf[2] & 0xff)) << 40) -
        //                  (((long)(buf[1] & 0xff)) << 48);
        //        }
        //        else
        //        {
        //            val = (-MIN_7_BYTE - 1) - (((long)((buf[5] << 24) + ((buf[6] & 0xff) << 16) +
        //                ((buf[7] & 0xff) << 8) + (buf[8] & 0xff))) & 0xffffffffL) -
        //                  (((long)(buf[4] & 0xff)) << 32) - (((long)(buf[3] & 0xff)) << 40) -
        //                  (((long)(buf[2] & 0xff)) << 48) - (((long)buf[1]) << 56);
        //        }
        //    }

        //		return val;
        //}

        //public double readDouble() 
        //{
        //		return Double.longBitsToDouble(readLong());
        //}

        //public float readFloat() 
        //{
        //		return Float.intBitsToFloat(readInt());
        //}

        //public String readString(int encodedBytes)  {
        //		if (encodedBytes == NULL) {
        //			return null;
        //		}
        //		if (encodedBytes == 0) {
        //			return "";
        //		}

        //		if (byteBuffer.Length<encodedBytes) {
        //			// round up to the next kb
        //			int size = (encodedBytes + 0x3ff) & 0xfffffc00;
        //// round up to the next 256 byte chunk
        ////maxBytesNeeded = (maxBytesNeeded  + 0x0ff) & 0xffffff00;
        //byteBuffer = new byte[size];
        //		}


        //        readBytes(byteBuffer, 0, encodedBytes);

        //int maxCharsNeeded = encodedBytes * transliterator.getMaxCharsPerByte();
        //		if (charBuffer.Length<maxCharsNeeded) {
        //			// round up to the next kb
        //			maxCharsNeeded = (maxCharsNeeded + 0x3ff) & 0xfffffc00;
        //			charBuffer = new char[maxCharsNeeded];
        //		}

        //		int charsDecoded = transliterator.decode(byteBuffer, 0, encodedBytes, charBuffer, 0);

        //		return new String(charBuffer, 0, charsDecoded);
        //	}

        //	public String readString() 
        //{
        //		try {
        //        return readString(readCompressedInt());
        //    }
        //		catch (UtilException e) {
        //        throw new IOException(e.getMessage());
        //    }
        //}

        ///**
        // * Read the specified number of bytes.  If the EOF is reached before the full requested amount is reached, this method
        // * will throw a EOF exception.
        // */
        //public void readBytes(byte[] bytes, int offset, int length)


        //{

        //		int bytesRead = 0;
        //		while (bytesRead < length) {
        //        int count = inputStream.Read(bytes, offset + bytesRead, length - bytesRead);
        //        if (count < 0)
        //        {
        //            throw new Exception("EOF Exception");
        //        }
        //        bytesRead += count;
        //        totalBytesRead += count;
        //    }

        //}

        ///**
        // * Read up to the specified number of bytes, but may return smaller number of bytes.  If no more bytes
        // * are available, then -1 is returned.
        // * @param bytes
        // * @param offset
        // * @param length
        // * @return Returns number of bytes read. Returns -1 if at EOF.
        // * @throws IOException
        // */
        //public int read(byte[] bytes, int offset, int length)


        //{
        //		int bytesRead = inputStream.Read(bytes, offset, length);
        //		if (bytesRead > 0) {
        //        totalBytesRead += bytesRead;
        //    }
        //		return bytesRead;
        //}

        //public void close() 
        //{
        //    //inputStream.close();
        //    inputStream = null;
        //}

        //public UtilTransliterator getTransliterator()
        //{
        //    return transliterator;
        //}

        //public int getTotalBytesRead()
        //{
        //    return totalBytesRead;
        //}

        //        UtilTransliterator BinaryDataInput.getTransliterator()
        //        {
        //            throw new NotImplementedException();
        //        }
    }
}

