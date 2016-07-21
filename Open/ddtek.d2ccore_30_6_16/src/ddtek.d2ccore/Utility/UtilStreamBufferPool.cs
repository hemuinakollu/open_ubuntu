using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace ddtek.d2ccore.Utility
{
    public class UtilStreamBufferPool
    {
        public static readonly int DEFAULT_MAX_QUEUE_SIZE = 10;
        public static readonly int DEFAULT_INIT_BIG_BUFFER_SIZE = 32768;

        static BlockingCollection<UtilStreamBuffer> bigBufferQueue;
         int initBufferSize;

        private int poolHits;
        private int buffersCreated;
        private int buffersDiscarded;

        public UtilStreamBufferPool(int maxQueueSize, int initBufferSize)
        {
            bigBufferQueue = new BlockingCollection<UtilStreamBuffer>(maxQueueSize);
            this.initBufferSize = initBufferSize;
        }

        //public UtilStreamBuffer getBigBuffer()
        //{
        //    UtilStreamBuffer buffer = bigBufferQueue.poll();
        //    if (buffer == null)
        //    {
        //        buffer = new UtilStreamBuffer(initBufferSize);
        //        buffersCreated++;
        //    }
        //    else
        //    {
        //        poolHits++;
        //    }
        //    return buffer;
        //}

        //public void putBigBuffer(UtilStreamBuffer buffer)
        //{
        //    if (!bigBufferQueue.offer(buffer))
        //    {
        //        buffersDiscarded++;
        //    }
        //}

        public string describeStats()
        {
            return "poolHits=" + poolHits + ";buffersCreated=" + buffersCreated + ";buffersDiscarded=" + buffersDiscarded;
        }
    }
}
