using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDInt.Utility;
using System.IO;

namespace ddtek.d2ccore.Common
{
    public class BinaryMessageWriter : BinaryDataOutput
    {


        //    //private final static int INIT_BUFFER_SIZE = 32768;

        //        private byte[] buffer = new byte[16];
        private Stream outputStream;
        readonly private UtilTransliterator transliterator;

        private int totalBytesWritten;

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

        //        // For compressed integers and longs

        //        public static readonly int MAX_POSITIVE = BinaryMessageReader.MAX_POSITIVE;
        //        private static readonly int MAX_NEGATIVE = BinaryMessageReader.MAX_NEGATIVE;
        //        private static readonly int OFFSET = BinaryMessageReader.OFFSET;

        //        private static readonly int MAX_1_BYTE = BinaryMessageReader.MAX_1_BYTE;
        //        private static readonly int MAX_2_BYTE = BinaryMessageReader.MAX_2_BYTE;
        //        private static readonly int MAX_3_BYTE = BinaryMessageReader.MAX_3_BYTE;

        //        private static readonly long MAX_4_BYTE = BinaryMessageReader.MAX_4_BYTE;
        //        private static readonly long MAX_5_BYTE = BinaryMessageReader.MAX_5_BYTE;
        //        private static readonly long MAX_6_BYTE = BinaryMessageReader.MAX_6_BYTE;
        //        private static readonly long MAX_7_BYTE = BinaryMessageReader.MAX_7_BYTE;

        //        private static readonly int MIN_1_BYTE = BinaryMessageReader.MIN_1_BYTE;
        //        private static readonly int MIN_2_BYTE = BinaryMessageReader.MIN_2_BYTE;
        //        private static readonly int MIN_3_BYTE = BinaryMessageReader.MIN_3_BYTE;

        //        private static readonly long MIN_4_BYTE = BinaryMessageReader.MIN_4_BYTE;
        //        private static readonly long MIN_5_BYTE = BinaryMessageReader.MIN_5_BYTE;
        //        private static readonly long MIN_6_BYTE = BinaryMessageReader.MIN_6_BYTE;
        //        private static readonly long MIN_7_BYTE = BinaryMessageReader.MIN_7_BYTE;

        public BinaryMessageWriter(UtilTransliterator transliterator)
        {
            this.transliterator = transliterator;
        }

        public void initialize(Stream outputStream)
        {
            totalBytesWritten = 0;
            this.outputStream = outputStream;
        }

        public UtilTransliterator getTransliterator()
        {
            return transliterator;
        }

        //        public void writeBoolean(bool value) 
        //        {
        //            outputStream.write(value ? 1 : 0);

        //		++totalBytesWritten;
        //        }

        //        public void writeShort(short value) 
        //        {
        //            outputStream.write((value >>>  8) & 0xFF);
        //            outputStream.write((value >>>  0) & 0xFF);

        //            totalBytesWritten += 2;
        //        }

        //        public void writeInt(int value) 
        //        {
        //            outputStream.write((int)((uint)value >> 24)& 0xFF);
        //            //outputStream.write((value >>> 24) & 0xFF);
        //            outputStream.write((int)((uint)value >> 16) & 0xFF);
        //            outputStream.write((int)((uint)value >> 8) & 0xFF);
        //            outputStream.write((int)((uint)value >> 0) & 0xFF);

        //            totalBytesWritten += 4;
        //        }

        //        public void writeLong(long value) 
        //        {

        //            //buffer[0] = (byte)(value >>> 56);
        //            buffer[1] = (byte)(value >>> 48);
        //		    buffer[2] = (byte)(value >>> 40);
        //		    buffer[3] = (byte)(value >>> 32);
        //		    buffer[4] = (byte)(value >>> 24);
        //		    buffer[5] = (byte)(value >>> 16);
        //		    buffer[6] = (byte)(value >>>  8);
        //		    buffer[7] = (byte)(value);
        //		    outputStream.write(buffer, 0, 8);

        //		    totalBytesWritten += 8;
        //	}

        //    public void writeCompressedLong(long value) 
        //    {

        //		long	tmp;
        //		int	len;
        //            //byte	buf [];
        //        var buf = new byte[1024];

        //		if ((value >= MAX_NEGATIVE) && (value <= MAX_POSITIVE)) {
        //            outputStream.write(((int)value) + OFFSET);
        //            ++totalBytesWritten;
        //            return;
        //        }

        //		if ((value >= int.MinValue) && (value <= int.MaxValue)) {
        //            writeCompressedInt((int)value);
        //            return;
        //        }

        //        buf = buffer;

        //		if (value > 0) {
        //            if (value <= MAX_4_BYTE)
        //            {
        //                int tmp2 = (int)(value - (MAX_3_BYTE + 1));

        //                buf[0] = 3;
        //                buf[1] = (byte)(tmp2 >> 24);
        //                buf[2] = (byte)(tmp2 >> 16);
        //                buf[3] = (byte)(tmp2 >> 8);
        //                buf[4] = (byte)tmp2;
        //                len = 5;
        //            }
        //            else if (value <= MAX_5_BYTE)
        //            {
        //                tmp = value - (MAX_4_BYTE + 1);

        //                buf[0] = 4;
        //                buf[1] = (byte)(tmp >> 32);
        //                buf[2] = (byte)(tmp >> 24);
        //                buf[3] = (byte)(tmp >> 16);
        //                buf[4] = (byte)(tmp >> 8);
        //                buf[5] = (byte)tmp;
        //                len = 6;
        //            }
        //            else if (value <= MAX_6_BYTE)
        //            {
        //                tmp = value - (MAX_5_BYTE + 1);

        //                buf[0] = 5;
        //                buf[1] = (byte)(tmp >> 40);
        //                buf[2] = (byte)(tmp >> 32);
        //                buf[3] = (byte)(tmp >> 24);
        //                buf[4] = (byte)(tmp >> 16);
        //                buf[5] = (byte)(tmp >> 8);
        //                buf[6] = (byte)tmp;
        //                len = 7;
        //            }
        //            else if (value <= MAX_7_BYTE)
        //            {
        //                tmp = value - (MAX_6_BYTE + 1);

        //                buf[0] = 6;
        //                buf[1] = (byte)(tmp >> 48);
        //                buf[2] = (byte)(tmp >> 40);
        //                buf[3] = (byte)(tmp >> 32);
        //                buf[4] = (byte)(tmp >> 24);
        //                buf[5] = (byte)(tmp >> 16);
        //                buf[6] = (byte)(tmp >> 8);
        //                buf[7] = (byte)tmp;
        //                len = 8;
        //            }
        //            else
        //            {
        //                tmp = value - (MAX_7_BYTE + 1);

        //                buf[0] = 7;
        //                buf[1] = (byte)(tmp >> 56);
        //                buf[2] = (byte)(tmp >> 48);
        //                buf[3] = (byte)(tmp >> 40);
        //                buf[4] = (byte)(tmp >> 32);
        //                buf[5] = (byte)(tmp >> 24);
        //                buf[6] = (byte)(tmp >> 16);
        //                buf[7] = (byte)(tmp >> 8);
        //                buf[8] = (byte)tmp;
        //                len = 9;
        //            }
        //        }
        //		else if ((value = - value) > 0) {
        //            if (value <= MIN_4_BYTE)
        //            {
        //                int tmp2 = (int)(value - (MIN_3_BYTE + 1));

        //                buf[0] = 11;
        //                buf[1] = (byte)(tmp2 >> 24);
        //                buf[2] = (byte)(tmp2 >> 16);
        //                buf[3] = (byte)(tmp2 >> 8);
        //                buf[4] = (byte)tmp2;
        //                len = 5;
        //            }
        //            else if (value <= MIN_5_BYTE)
        //            {
        //                tmp = value - (MIN_4_BYTE + 1);

        //                buf[0] = 12;
        //                buf[1] = (byte)(tmp >> 32);
        //                buf[2] = (byte)(tmp >> 24);
        //                buf[3] = (byte)(tmp >> 16);
        //                buf[4] = (byte)(tmp >> 8);
        //                buf[5] = (byte)tmp;
        //                len = 6;
        //            }
        //            else if (value <= MIN_6_BYTE)
        //            {
        //                tmp = value - (MIN_5_BYTE + 1);

        //                buf[0] = 13;
        //                buf[1] = (byte)(tmp >> 40);
        //                buf[2] = (byte)(tmp >> 32);
        //                buf[3] = (byte)(tmp >> 24);
        //                buf[4] = (byte)(tmp >> 16);
        //                buf[5] = (byte)(tmp >> 8);
        //                buf[6] = (byte)tmp;
        //                len = 7;
        //            }
        //            else if (value <= MIN_7_BYTE)
        //            {
        //                tmp = value - (MIN_6_BYTE + 1);

        //                buf[0] = 14;
        //                buf[1] = (byte)(tmp >> 48);
        //                buf[2] = (byte)(tmp >> 40);
        //                buf[3] = (byte)(tmp >> 32);
        //                buf[4] = (byte)(tmp >> 24);
        //                buf[5] = (byte)(tmp >> 16);
        //                buf[6] = (byte)(tmp >> 8);
        //                buf[7] = (byte)tmp;
        //                len = 8;
        //            }
        //            else
        //            {
        //                tmp = value - (MIN_7_BYTE + 1);

        //                buf[0] = 15;
        //                buf[1] = (byte)(tmp >> 56);
        //                buf[2] = (byte)(tmp >> 48);
        //                buf[3] = (byte)(tmp >> 40);
        //                buf[4] = (byte)(tmp >> 32);
        //                buf[5] = (byte)(tmp >> 24);
        //                buf[6] = (byte)(tmp >> 16);
        //                buf[7] = (byte)(tmp >> 8);
        //                buf[8] = (byte)tmp;
        //                len = 9;
        //            }
        //        }
        //		else {
        //            tmp = value - (MIN_7_BYTE + 1);

        //            buf[0] = 15;
        //            buf[1] = (byte)0x80;
        //            buf[2] = 0;
        //            buf[3] = 0;
        //            buf[4] = 0;
        //            buf[5] = 0;
        //            buf[6] = 0;
        //            buf[7] = 0;
        //            buf[8] = 0;
        //            len = 9;
        //        }

        //        outputStream.write(buf, 0, len);
        //        totalBytesWritten += len;
        //    }

        //    public void writeByte(byte value) 
        //    {
        //        outputStream.write(value);
        //		++totalBytesWritten;
        //    }

        //    public void writeUnsignedByte(int value) 
        //    {
        //        outputStream.write(value);
        //		++totalBytesWritten;
        //    }

        //        //public void writeDouble(double value) 
        //        //{
        //        //    writeLong(Double.doubleToLongBits(value));
        //        //}

        //        //public void writeFloat(float value) 
        //        //{
        //        //    writeInt(Float.floatToIntBits(value));
        //        //}

        //        //  public void writeString(String value) 
        //        //  {
        //        //if (value == null) {
        //        //          writeCompressedInt(NULL);
        //        //      }
        //        //else {
        //        //          int sourceLength = value.length();
        //        //          if (sourceLength == 0)
        //        //          {
        //        //              writeCompressedInt(0);
        //        //          }
        //        //          else
        //        //          {
        //        //              // Trying to avoid making another copy of the source chars be grabbing the char array directly 
        //        //              // from the target string.  It is sort sneaky since the char array is not accessible other than
        //        //              // reflection, but Scott B. recommends this approach.
        //        //              char[] sourceChars = UtilStringFunctions.getCharArray(value);
        //        //              int sourceOffset = UtilStringFunctions.getCharArrayOffset(value);
        //        //              int maxBytesNeeded = sourceLength * transliterator.getMaxBytesPerChar();

        //        //              byte[] byteBuffer = transliterator.getBytesCache(maxBytesNeeded);
        //        //              int bytesEncoded;
        //        //              try
        //        //              {
        //        //                  bytesEncoded = transliterator.encode(sourceChars, sourceOffset, sourceLength, byteBuffer, 0);
        //        //              }
        //        //              catch (UtilException e)
        //        //              {
        //        //                  throw new IOException(e.getMessage());
        //        //              }
        //        //              writeCompressedInt(bytesEncoded);
        //        //              outputStream.write(byteBuffer, 0, bytesEncoded);

        //        //              totalBytesWritten += bytesEncoded;
        //        //          }
        //        //      }
        //        //  }

        //        public void writeBytes(byte[] value, int offset, int length)


        //    {
        //        outputStream.write(value, offset, length);

        //        totalBytesWritten += length;
        //    }

        //  //  public void writeCompressedInt(int value) 
        //  //  {

        //		//int	tmp, len;
        //		//byte	buf [];

        //		//if ((value >= MAX_NEGATIVE) && (value <= MAX_POSITIVE)) {
        //  //          outputStream.write(value + OFFSET);
        //  //          ++totalBytesWritten;
        //  //          return;
        //  //      }

        //  //      buf = buffer;

        //		//if (value > 0) {
        //  //          if (value <= MAX_1_BYTE)
        //  //          {
        //  //              buf[0] = 0;
        //  //              buf[1] = (byte)(value - (MAX_POSITIVE + 1));
        //  //              len = 2;
        //  //          }
        //  //          else if (value <= MAX_2_BYTE)
        //  //          {
        //  //              tmp = value - (MAX_1_BYTE + 1);

        //  //              buf[0] = 1;
        //  //              buf[1] = (byte)(tmp >> 8);
        //  //              buf[2] = (byte)tmp;
        //  //              len = 3;
        //  //          }
        //  //          else if (value <= MAX_3_BYTE)
        //  //          {
        //  //              tmp = value - (MAX_2_BYTE + 1);

        //  //              buf[0] = 2;
        //  //              buf[1] = (byte)(tmp >> 16);
        //  //              buf[2] = (byte)(tmp >> 8);
        //  //              buf[3] = (byte)tmp;
        //  //              len = 4;
        //  //          }
        //  //          else
        //  //          {
        //  //              tmp = value - (MAX_3_BYTE + 1);

        //  //              buf[0] = 3;
        //  //              buf[1] = (byte)(tmp >> 24);
        //  //              buf[2] = (byte)(tmp >> 16);
        //  //              buf[3] = (byte)(tmp >> 8);
        //  //              buf[4] = (byte)tmp;
        //  //              len = 5;
        //  //          }
        //  //      }
        //		//else if ((value = - value) > 0) {
        //  //          if (value <= MIN_1_BYTE)
        //  //          {
        //  //              buf[0] = 8;
        //  //              buf[1] = (byte)(value + (MAX_NEGATIVE - 1));
        //  //              len = 2;
        //  //          }
        //  //          else if (value <= MIN_2_BYTE)
        //  //          {
        //  //              tmp = value - (MIN_1_BYTE + 1);

        //  //              buf[0] = 9;
        //  //              buf[1] = (byte)(tmp >> 8);
        //  //              buf[2] = (byte)tmp;
        //  //              len = 3;
        //  //          }
        //  //          else if (value <= MIN_3_BYTE)
        //  //          {
        //  //              tmp = value - (MIN_2_BYTE + 1);

        //  //              buf[0] = 10;
        //  //              buf[1] = (byte)(tmp >> 16);
        //  //              buf[2] = (byte)(tmp >> 8);
        //  //              buf[3] = (byte)tmp;
        //  //              len = 4;
        //  //          }
        //  //          else
        //  //          {
        //  //              tmp = value - (MIN_3_BYTE + 1);

        //  //              buf[0] = 11;
        //  //              buf[1] = (byte)(tmp >> 24);
        //  //              buf[2] = (byte)(tmp >> 16);
        //  //              buf[3] = (byte)(tmp >> 8);
        //  //              buf[4] = (byte)tmp;
        //  //              len = 5;
        //  //          }
        //  //      }
        //		//else {
        //  //          tmp = value - (MIN_3_BYTE + 1);

        //  //          buf[0] = 11;
        //  //          buf[1] = (byte)0x80;
        //  //          buf[2] = 0;
        //  //          buf[3] = 0;
        //  //          buf[4] = 0;
        //  //          len = 5;
        //  //      }

        //  //      outputStream.write(buf, 0, len);
        //  //      totalBytesWritten += len;
        //  //  }

        public void close()
        {
            outputStream.Flush();
            //outputStream.close();
            outputStream = null;
        }

        public void writeBoolean(bool value)
        {
            throw new NotImplementedException();
        }

        public void writeShort(short value)
        {
            throw new NotImplementedException();
        }

        public void writeInt(int value)
        {
            throw new NotImplementedException();
        }

        public void writeCompressedInt(int value)
        {
            throw new NotImplementedException();
        }

        public void writeLong(long value)
        {
            throw new NotImplementedException();
        }

        public void writeCompressedLong(long value)
        {
            throw new NotImplementedException();
        }

        public void writeByte(byte value)
        {
            throw new NotImplementedException();
        }

        public void writeUnsignedByte(int value)
        {
            throw new NotImplementedException();
        }

        public void writeDouble(double value)
        {
            throw new NotImplementedException();
        }

        public void writeFloat(float value)
        {
            throw new NotImplementedException();
        }

        public void writeString(string value)
        {
            throw new NotImplementedException();
        }

        public void writeBytes(byte[] value, int offset, int length)
        {
            throw new NotImplementedException();
        }

        public void writeByteERROR()
        {
            throw new NotImplementedException();
        }

        public void writeByteNULL()
        {
            throw new NotImplementedException();
        }

        public void writeByteREPEAT()
        {
            throw new NotImplementedException();
        }

        public void writeCompressedIntERROR()
        {
            throw new NotImplementedException();
        }

        public void writeCompressedIntNULL()
        {
            throw new NotImplementedException();
        }

        public void writeCompressedIntREPEAT()
        {
            throw new NotImplementedException();
        }

        public StreamWriter getOutputStream()
        {
            throw new NotImplementedException();
        }

        //    public OutputStream getOutputStream()
        //    {
        //        return outputStream;
        //    }

        //    //public int getTotalBytesWritten()
        //    //{
        //    //    return totalBytesWritten;
        //    //}

        //    //public void writeByteERROR()
        //    //{
        //    //    writeByte ((byte) ERROR);
        //    //}

        //    //public void writeByteNULL() 
        //    //{
        //    //    writeByte ((byte) NULL);
        //    //}

        //    //public void writeByteREPEAT() 
        //    //{
        //    //    writeByte ((byte) REPEAT);
        //    //}

        //    //public void writeCompressedIntERROR() 
        //    //{
        //    //    writeCompressedInt (ERROR);
        //    //}

        //    //public void writeCompressedIntNULL() 
        //    //{
        //    //    writeCompressedInt (NULL);
        //    //}

        //    //public void writeCompressedIntREPEAT() 
        //    //{
        //    //    writeCompressedInt (REPEAT);
        //    //}

    }
}