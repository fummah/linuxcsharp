using System;
using aaa_library;
using System.Threading;
using System.Threading.Tasks;

namespace CHF
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            string homebase = Environment.GetEnvironmentVariable("HOME");
            //CHF Process              
            CHFClass pr = new CHFClass();
            pr.subscribers("Medswitch");
            pr.subscribers("Healthbridge");
            pr.subscribers("cgm");
            string fileName = "MCA_CHF_Zestlife_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            string fileName2 = "CHF_KaeloWesternSanlam_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            string healthbridgefile = "Healthbridge_MCA.txt";
            string cgmfile = "CHF_" + DateTime.Now.ToString("MMyyyy") + ".txt";
            //there is an error Could not find file 'C:\xc\files\Healthbridge\CHF_KaeloWesternSanlam_20230420.txt'.
            //Run CHF for Healthbridge
            pr.runText(homebase+@"/files/CHF/kaelo.txt", homebase + @"/files/Healthbridge/" + healthbridgefile, "Healthbridge");
            //Run CHF for Medswitch
            pr.runText(homebase + @"/files/CHF/kaelo.txt", homebase + @"/files/Medswitch/" + fileName2, "Medswitch");
            //Run CHF for CGM
            pr.runText(homebase + @"/files/CHF/kaelo.txt", homebase + @"/files/CGM/" + cgmfile, "cgm");
            Thread.Sleep(300);
            //Merge CHF for Healthbridge
            pr.mergeFiles(homebase + @"/files/Healthbridge/" + healthbridgefile, homebase + @"/files/Healthbridge.txt");
            //Merge CHF for Medswitch
            pr.mergeFiles(homebase + @"/files/Medswitch/" + fileName2, homebase + @"/files/Medswitch.txt");
            //Merge CHF for CGM
            pr.mergeFiles(homebase + @"/files/CGM/" + cgmfile, homebase + @"/files/cgm.txt");

            Thread.Sleep(300);
            //Run Cinagi
            pr.runCinagi(homebase + @"/files/CHF/cinagi.txt", homebase + @"/files/Healthbridge/Cinagi_Healthbridge.txt", "Healthbridge");
            pr.runCinagi(homebase + @"/files/CHF/cinagi.txt", homebase + @"/files/Medswitch/CHF_Cinagi" + DateTime.Now.ToString("yyyyMMdd") + ".txt", "Medswitch");
            pr.runCinagi(homebase + @"/files/CHF/cinagi.txt", homebase + @"/files/CGM/Cinagi_" + DateTime.Now.ToString("MMyyyy") + ".txt", "cgm");
            Thread.Sleep(300);
            //Merge CHF for Healthbridge
            pr.mergeFiles(homebase + @"/files/Healthbridge/" + healthbridgefile, homebase + @"/files/Healthbridge/Cinagi_Healthbridge.txt");
            //Merge CHF for CGM
            pr.mergeFiles(homebase + @"/files/CGM/" + cgmfile, homebase + @"/files/CGM/Cinagi_" + DateTime.Now.ToString("MMyyyy") + ".txt");
            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}
