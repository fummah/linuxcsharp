using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using Renci.SshNet;
using Renci.SshNet.Common;
using Renci.SshNet.Sftp;
using aaa_library;
using System.Data;
using System.Threading;
namespace archive_documents
{
    class Program
    {
        static void Main(string[] args)
        {
            Action ac = new Action();
            // ac.DeleteFile();
            ac.alldocs();
            Thread.Sleep(50);
            //System.Diagnostics.Process.Start("https://medclaimassist.co.za/admin/backup.php");

            Console.ReadLine();
        }
    }

    class Action
    {
        private int doc_id { get; set; }

        public void DeleteFile(string filename)
        {
            const string server = @"greenwest.co.za";
            const string username = @"greenwhc";
            const string password = @"MCAFTPn0v17";
            string sftpPath = @"public_html/mca/documents/" + @filename;
            const int port = 22;
            Console.WriteLine("Starting");
            try
            {


                MySftp.DeleteFile(server, port, username, password, sftpPath);
                Console.WriteLine("File deleted");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error on deleting" + ex);
            }

        }

        public void alldocs()
        {

            // int num = db.getDuplicate1(claim).Tables[0].Rows.Count;

            using (DBConnect db = new DBConnect())
            {
                int all = 0;
                int error = 0;
                int num = db.getDocumentstodelete().Rows.Count;
                string homepath = Environment.GetEnvironmentVariable("HOME");
                string path = homepath + @"/files/MCA_Archive/";
                //Console.WriteLine(num.ToString());

                Console.WriteLine("Starting");
                Console.WriteLine(num.ToString());
                foreach (DataRow drow in db.getDocumentstodelete().Rows)
                {

                    string vv = "";
                    string ss = "";

                    try
                    {
                        string doc_id_s = drow["doc_id"].ToString();
                        this.doc_id = int.Parse(doc_id_s);
                        string claim_id = drow["claim_id"].ToString();
                        string date = drow["date"].ToString();
                        string doc = drow["doc_description"].ToString();
                        string rand = drow["randomNum"].ToString();
                        string client = drow["client_name"].ToString();
                        string usern = drow["username"].ToString();
                        string fulldoc = @rand + doc;
                        DateTime birthDate = DateTime.Parse(date);
                        string year = birthDate.ToString("yyyy");
                        string month = birthDate.ToString("MMMM");
                        string source = @"/public_html/mca/documents/";

                        string pathString = path + year + @"/" + month + @"/" + client + @"/" + usern;
                        //string pathString = @"Y:\MCA_Archive\documents";
                        vv = pathString;
                        ss = source;
                        if (!Directory.Exists(pathString))
                        {
                            //Console.WriteLine("Thefolse existes");
                            Directory.CreateDirectory(pathString);

                        }
                        Console.WriteLine("Test 1");
                        //ftpDocs.deletedownload(source, pathString);

                        DownloadFile(pathString, fulldoc);
                        Console.WriteLine("Test 2");
                        all++;
                        Console.WriteLine(claim_id + "----" + date + "----" + num.ToString() + "----" + usern + "----" + all.ToString());
                        DeleteFile(fulldoc);
                        Console.WriteLine("Test 3");
                        db.updateDocument(this.doc_id);

                    }
                    catch (Exception ex)
                    {
                        error++;
                        Console.WriteLine("There is an error ---> " + ex.Message + "=====" + num.ToString() + "=====" + error.ToString());
                        //db.updateDocument(this.doc_id);

                    }
                }
            }


        }


        public void DownloadFile(string localpath, string filename)
        {


            string host = @"greenwest.co.za";
            string username = @"greenwhc";
            string password = @"MCAFTPn0v17";
            string localFile = @localpath + @"/" + @filename;
            //string localFileName = Path.GetFileName(localFile);
            string remoteFileName = @"/public_html/mca/documents/" + @filename;
            //Console.WriteLine(remoteFileName);
            using (var sftp = new SftpClient(host, username, password))
            {

                sftp.Connect();
                Console.WriteLine("File Path ----> " + remoteFileName);
                using (Stream file = File.OpenWrite(localFile))
                {
                    sftp.DownloadFile(remoteFileName, file);
                    Console.WriteLine("File moved");
                }

                sftp.Disconnect();
            }
        }

        public void mytest()
        {
        }
    }
}
