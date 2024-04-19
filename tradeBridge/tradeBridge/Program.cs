using System;
using aaa_library;

namespace tradeBridge
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            using (DBConnect db = new DBConnect())
            {
                // TradeBridge Process

                string ffname = "ClaimOutput_" + DateTime.Now.ToString("yyyyMMdd") + ".csv";
                //string ffname = "ClaimOutput_20230619.csv";
                Console.WriteLine(ffname);
                string basehome = Environment.GetEnvironmentVariable("HOME");
                string path = basehome+@"/files/Tradebridge/" + ffname;
                trade_bridge tb = new trade_bridge();
                tb.fetchDetails(path);
                Console.WriteLine("This is done");
                //Console.ReadLine();
            }
        }
    }
}
