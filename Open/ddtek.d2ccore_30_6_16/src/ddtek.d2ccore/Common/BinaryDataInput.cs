using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ddtek.d2ccore.Common
{
    public interface BinaryDataInput
    {
        //public static int LAST = BinaryDataOutput.LAST;
        //public static int NULL = BinaryDataOutput.NULL;
        //public static int ERROR = BinaryDataOutput.ERROR;
        //public static int REPEAT = BinaryDataOutput.REPEAT;
        //public static int CONTINUE = BinaryDataOutput.CONTINUE;

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

        bool readBoolean() ;

        byte readByte() ;

        int readUnsignedByte() ;

        short readShort() ;

        int readInt() ;

        int readCompressedInt() ;

        long readLong() ;

        long readCompressedLong() ;

        double readDouble() ;

        float readFloat() ;

        string readString() ;

        string readString(int len);

        //abstract void readBytes(byte[] bytes, int offset, int length)  ;

        //UtilTransliterator getTransliterator();

        //virtual public int getTotalBytesRead();

        void close() ;

    }
}

