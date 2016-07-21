using ddtek.d2ccore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApp
{
    public class Program
    {
        public static void Main(string[] args)

        {
            DDCloudImplConnection con = new DDCloudImplConnection();
            try
            {
                con.open();
                Console.WriteLine("Connection Successful");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }
    }
}
