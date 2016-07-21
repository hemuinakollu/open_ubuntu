using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDInt.Utility
{
    public sealed class UtilDebug
    {
        /**
     * Fooprint information field
     */
        static private String footprint = "$Revision: #1 $";


    //    /**
    //     * WriterOutput stream in which to put print messages.
    //     */
    //    public PrintWriter out;

   
    ///**
    // * Sets the the OutputStream associated with debugging information.
    // *
    // * @param outStream  - OutputStream associated with debugging information.
    // */
    //public sealed void setPrintWriter(
    //    PrintWriter printWriter)
    //    {
            
    //    out = printWriter;
    //    }


    //    /**
    //     * If the specified condition is false (and UtilDebugSwitch.debugBuild 
    //     * is true, this method prints the specified message and deliberately 
    //     * dereferences a null object. This will cause program termination
    //     * unless a method on the stack is catching runtime excpetions.
    //     *
    //     * @param finalMessage Message to print before termination.
    //     *
    //     * @param expr The expression to evaluate and, if false, causes
    //     * printing of specified message and program termination.
    //     */
    //    static public sealed void DDTek_assert(
    //        String finalMessage,
    //        bool expr)
    //    {

    //        if (UtilDebugSwitch.debugBuild && !expr)
    //        {
    //            //System.out.println(finalMessage);
    //            String nullObj = null;
    //            nullObj.getBytes();
    //        }
    //    }
    }
}
