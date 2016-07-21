using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ddtek.d2ccore.Common
{
    public interface LogBuilder
    {
        void startLogMessage();
        void addLogPair(string name, string value);
        void addLogPair(string name, int value);
        void addLogPair(string name, long value);
        void addLogPair(string name, bool value);
        void addLogPair(string name, string[] values);

        void addText(string text);

        void addSection(string name);
        void endSection();
    }
}
