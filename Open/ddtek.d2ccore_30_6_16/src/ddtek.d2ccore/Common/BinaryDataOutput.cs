using System;
//using System.IO.Stream;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ddtek.d2ccore.Common
{
    public interface BinaryDataOutput
    {
        //public static sealed int LAST = 0;
        //public static int NULL = -1;
        //public static int ERROR = -2;
        //public static int REPEAT = -3;
        //public static int CONTINUE = -4;

            // Property declaration:
        int LAST
        {
            get;
            set;
        }
        int NULL
        {
            get;
            set;
        }
        int ERROR
        {
            get;
            set;
        }
        int REPEAT
        {
            get;
            set;
        }
        int CONTINUE
        {
            get;
            set;
        }


        void writeBoolean(bool value);


        void writeShort(short value);

        void writeInt(int value);

        void writeCompressedInt(int value);

        void writeLong(long value);

        void writeCompressedLong(long value) ;

        void writeByte(byte value) ;

        void writeUnsignedByte(int value) ;

        void writeDouble(double value) ;

        void writeFloat(float value) ;

        void writeString(string value) ;

        void writeBytes(byte[] value, int offset, int length) ;

        void writeByteERROR() ;

        void writeByteNULL() ;

        void writeByteREPEAT() ;

        void writeCompressedIntERROR() ;

        void writeCompressedIntNULL() ;

        void writeCompressedIntREPEAT() ;

        //int getTotalBytesWritten();

        StreamWriter getOutputStream();

        void close() ;

        //UtilTransliterator getTransliterator();
    }
}

