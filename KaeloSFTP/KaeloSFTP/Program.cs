using System;
using System.IO;
using aaa_library;
using System.Threading;
using System.Threading.Tasks;
using System.Data;
namespace KaeloSFTP
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            string homebase = Environment.GetEnvironmentVariable("HOME");
           
            CHFClass ks = new CHFClass();
            using (DBConnect db = new DBConnect())
            {
                string afilesy = db.getAAA(1).Tables[0].Rows[0]["files"].ToString();
                string[] afilesx = afilesy.Split(',');
                for (int xc = 1; xc < afilesx.Length; xc++)
                {
                    try
                    {
                        string afile = afilesx[xc];
                        string actfilename = afile;
                        Console.WriteLine(actfilename);
                        int cc = db.getFilecheck(afile).Tables[0].Rows.Count;
                        //int cc = 0;
                        if (cc < 1)
                        {

                            string filename = homebase + @"/files/Split/" + actfilename;
                            Console.WriteLine("here");
                            Thread.Sleep(5);
                            Console.WriteLine(filename+"----"+actfilename);
                            ks.KS(filename, actfilename,"");

                            Thread.Sleep(5);
                        }
                        else
                        {
                            Console.WriteLine("Already there");
                            Console.WriteLine(actfilename);
                        }

                    }
                    catch(Exception e)
                    {
                        Console.WriteLine("Error : "+e.Message);
                        Console.ReadLine();
                    }
                }
            }
            /*
            System.IO.DirectoryInfo di = new DirectoryInfo(homebase + @"/files/Split");

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }         
            */

            Console.WriteLine("The end");
            Console.ReadLine();
        }
    }
}
