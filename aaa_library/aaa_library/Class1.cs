using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using Renci.SshNet;
using Renci.SshNet.Common;
using Renci.SshNet.Sftp;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Text.RegularExpressions;

namespace aaa_library
{
    public static class ftpDocs
    {

        private const string Host = @"ftp.ambledown.co.za";
        private const int Port = 22;
        private const string Username = "medclaimassist";
        private const string Password = "?E-Yp7PK";
        public const string ambledownFolder = @"second\";
        public const string ambledownFolder1 = @"second";
        public const string xcFolder = @"second\XC\";
        public const string xcFolder1 = @"second\XC";
        public const string reportsFolder = @"C:\xc\Reports\";
        public static string getName { get; set; }


        private const string Host1 = @"greenwest.co.za";
        private const int Port1 = 22;
        private const string Username1 = "greenwhc";
        private const string Password1 = "MCAFTPn0v17";
        //private const string Source = @"/2017-61230";
        // private const string een = @"C:\Users\Tendaif\Documents\second";

        public static void startDoc(string Source)
        {
            string Destination = "";


            using (DBConnect db = new DBConnect())
            {

                string gg = @db.getConfigs().Tables[0].Rows[0]["destination_folder"].ToString() + ambledownFolder1;
                //@"C:\Users\Tendaif\Documents\second";
                //MessageBox.Show(Source);
                Destination = gg;
            }
            KeyboardInteractiveAuthenticationMethod keybAuth = new KeyboardInteractiveAuthenticationMethod(Username);
            keybAuth.AuthenticationPrompt += new EventHandler<AuthenticationPromptEventArgs>(HandleKeyEvent);
            PasswordAuthenticationMethod pauth = new PasswordAuthenticationMethod(Username, Password);
            ConnectionInfo connectionInfo = new ConnectionInfo(Host, Port, Username, pauth, keybAuth);
            using (SftpClient sftp = new SftpClient(connectionInfo))
            {
                sftp.Connect();
                DownloadDirectory(sftp, Source, Destination);

            }

        }

        public static void startDoc1(string Source)
        {
            string Destination = "";


            using (DBConnect db = new DBConnect())
            {

                string gg = @db.getConfigs().Tables[0].Rows[0]["destination_folder"].ToString() + ambledownFolder1;
                //@"C:\Users\Tendaif\Documents\second";
                //MessageBox.Show(Source);
                //Destination = @"C:\Users\TendaiF\Documents";
                Destination = gg;
                Console.WriteLine("pathe==" + Source + "-----" + Destination);
            }
            KeyboardInteractiveAuthenticationMethod keybAuth = new KeyboardInteractiveAuthenticationMethod(Username1);
            keybAuth.AuthenticationPrompt += new EventHandler<AuthenticationPromptEventArgs>(HandleKeyEvent);
            PasswordAuthenticationMethod pauth = new PasswordAuthenticationMethod(Username1, Password1);
            ConnectionInfo connectionInfo = new ConnectionInfo(Host1, Port1, Username1, pauth, keybAuth);
            using (SftpClient sftp = new SftpClient(connectionInfo))
            {
                sftp.Connect();
                DownloadDirectory(sftp, Source, Destination);

            }

        }
        public static void startDoc2(string Source)
        {
            string Destination = "";

            const string Host1 = @"ftp.zestlife.co.za";
            const int Port1 = 22;
            const string Username1 = "MedClaimAssist";
            const string Password1 = "?rE4XC9pc2>&7W!";
            using (DBConnect db = new DBConnect())
            {


                //@"C:\Users\Tendaif\Documents\second";
                //MessageBox.Show(Source);
                Destination = @"C:\xc\Zestlife";
            }
            KeyboardInteractiveAuthenticationMethod keybAuth = new KeyboardInteractiveAuthenticationMethod(Username1);
            keybAuth.AuthenticationPrompt += new EventHandler<AuthenticationPromptEventArgs>(HandleKeyEvent);
            PasswordAuthenticationMethod pauth = new PasswordAuthenticationMethod(Username1, Password1);
            ConnectionInfo connectionInfo = new ConnectionInfo(Host1, Port1, Username1, pauth, keybAuth);
            using (SftpClient sftp = new SftpClient(connectionInfo))
            {
                sftp.Connect();
                DownloadDirectory(sftp, Source, Destination);

            }

        }
        private static void HandleKeyEvent(object sender, AuthenticationPromptEventArgs e)
        {
            foreach (AuthenticationPrompt prompt in e.Prompts)
            {
                if (prompt.Request.IndexOf("Password:", StringComparison.InvariantCultureIgnoreCase) != -1)
                {
                    prompt.Response = Password;
                }
            }
        }
        private static void DownloadDirectory(SftpClient client, string source, string destination)
        {
            var files = client.ListDirectory(@"/" + source);
            foreach (var file in files)
            {
                if (!file.IsDirectory && !file.IsSymbolicLink)
                {
                    DownloadFile(client, file, destination);
                }
                else if (file.IsSymbolicLink)
                {
                    // Console.WriteLine("Ignoring symbolic link {0}", file.FullName);
                }
                else if (file.Name != "." && file.Name != "..")
                {
                    var dir = Directory.CreateDirectory(Path.Combine(destination, file.Name));
                    DownloadDirectory(client, file.FullName, dir.FullName);
                }
                getName = file.Name;
            }
        }

        private static void DownloadFile(SftpClient client, SftpFile file, string directory)
        {
            // Console.WriteLine("Downloading {0}", file.FullName);
            using (Stream fileStream = File.OpenWrite(Path.Combine(directory, file.Name)))
            {
                client.DownloadFile(file.FullName, fileStream);
            }
        }


    }

    public class MySftp
    {

        private String Hostname;
        private String Username;
        private String Password;
        private int Port;
        private SftpClient m_sftpClient;
        public static string rr { get; set; }

        public MySftp(String Hostname, String Username, String Password,int Port=2222)
        {
            this.Hostname = Hostname;
            this.Username = Username;
            this.Password = Password;
            this.Port = Port;

            Initialize_SftpClient(Hostname, Username, Password);

        }

        private void Initialize_SftpClient(String Hostname, String Username, String Password)
        {
            PasswordAuthenticationMethod authenticationMethods = new PasswordAuthenticationMethod(Username, Password);
            //PrivateKeyAuthenticationMethod privateKeyAuthenticationMethod = new PrivateKeyAuthenticationMethod(“rsa.key”);
            ConnectionInfo ci = new ConnectionInfo(Hostname, Username, authenticationMethods);
            m_sftpClient = new SftpClient(ci);
        }

        public void Connect()
        {
            m_sftpClient.Connect();
        }

        public void ChangeDir(String dir)
        {
            m_sftpClient.ChangeDirectory(dir);
        }
        public void DisConnect()
        {
            m_sftpClient.Disconnect();
        }

        public void uploadFile(String srcFilename, String destFilename)
        {
            Console.WriteLine(srcFilename + "--------" + destFilename);
            FileStream fs = File.OpenRead(srcFilename);
            m_sftpClient.UploadFile(fs, destFilename);

            fs.Close();
        }

        public static void testDrive(int claim_id)
        {

            string Hostname = @"greenwest.co.za";
            string Username = @"greenwhc";
            string Password = @"MCAFTPn0v17";
            using (DBConnect db = new DBConnect())
            {

                //Random bvb = new Random();
                //int bvb1 = bvb.Next(1, 1000);
                //string bvb2 = bvb1.ToString();
                string srcFilename = @db.getConfigs().Tables[0].Rows[0]["destination_folder"].ToString() + ftpDocs.ambledownFolder + ftpDocs.getName;
                //MessageBox.Show(srcFilename);
                Random ran = new Random();
                rr = rr = ran.Next(1, 1000).ToString();
                string destFilename = @"public_html/mca/documents/" + rr + ftpDocs.getName;
                MySftp sftp = new MySftp(Hostname, Username, Password);
                sftp.Connect();
                sftp.uploadFile(srcFilename, destFilename);
                sftp.DisConnect();
                db.insertDocument(claim_id, ftpDocs.getName,33);
            }
        }


        public static void kxlUpload(string claim_number, string filename, string folder, DBConnect db,int email_id=0)
        {
            try
            {
                string Hostname = @"greenwest.co.za";
                string Username = @"greenwhc";
                string Password = @"MCAFTPn0v17";
                int Port = 2222;

                string srcFilename = folder + filename;              
                rr = claim_number;
                string destFilename = @"public_html/mca/documents/" + rr + filename;
                MySftp sftp = new MySftp(Hostname, Username, Password);
                sftp.Connect();
                sftp.uploadFile(srcFilename, destFilename);
                sftp.DisConnect();                
                db.insertDocument(db.myClaim_id, filename,33);
            }
            catch (Exception e)
            {
                Console.WriteLine("Errrosr -> "+e.Message);
            }
        }


        public static void xc(string doc, string name)
        {
            string Hostname = @"greenwest.co.za";
            string Username = @"greenwhc";
            string Password = @"MCAFTPn0v17";
            using (DBConnect db = new DBConnect())
            {
                try
                {
                    string srcFilename = @doc;

                    //MessageBox.Show(srcFilename);
                    Random ran = new Random();
                    rr = ran.Next(1, 1000).ToString();
                    string destFilename = @"public_html/mca/documents/" + rr + name;
                    MySftp sftp = new MySftp(Hostname, Username, Password);
                    sftp.Connect();
                    sftp.uploadFile(srcFilename, destFilename);
                    sftp.DisConnect();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("There ftp error " + ex.Message);
                }

            }
        }

        public static void UploadReports(string ftp_server, string username, string password, string path, string doc_name,string fullpath="")
        {

            string Hostname = @ftp_server;
            string Username = @username;
            string Password = @password;
            string fullName = doc_name + ".xlsx";
            using (DBConnect db = new DBConnect())
            {
                string srcFilename = fullpath;
                //MessageBox.Show(srcFilename);
                Random ran = new Random();
                rr = ran.Next(1, 1000).ToString();
                string destFilename = @"/" + path + fullName;
                MySftp sftp = new MySftp(Hostname, Username, Password);
                sftp.Connect();
                sftp.uploadFile(srcFilename, destFilename);
                sftp.DisConnect();

            }
        }


        public static void DeleteFile(string server, int port, string username, string password, string sftpPath)
        {
            using (SftpClient sftpClient = new SftpClient(server, port, username, password))
            {
                sftpClient.Connect();
                sftpClient.DeleteFile(sftpPath);
                sftpClient.Disconnect();
            }
        }


        public static void generalUpload(string Hostname, string Username, string Password, string destFilename, string srcFilename)
        {
            try
            {


                MySftp sftp = new MySftp(Hostname, Username, Password);
                sftp.Connect();
                sftp.uploadFile(srcFilename, destFilename);
                sftp.DisConnect();
            }
            catch (Exception ex)
            {
                Console.WriteLine("There ftp error " + ex.Message);
            }

        }



        public static void DownloadAll(string remoteDirectory, string localDirectory, string remoteFileName)
        {

            string host = @"greenwest.co.za";
            string username = @"greenwhc";
            string password = @"MCAFTPn0v17";
            //string remoteDirectory = "/RemotePath/";
            localDirectory = @localDirectory + @"\";

            using (var sftp = new SftpClient(host, username, password))
            {
                Stream file1 = File.OpenWrite(localDirectory + remoteFileName);
                sftp.Connect();
                sftp.DownloadFile(remoteDirectory + remoteFileName, file1);
                sftp.Disconnect();


            }
        }

       

        public static void DownloadLatest(string HostName, string UserName, string Password, string localPath, string remotePath,string load_date,int ftpid,DBConnect db)
        {
            string str1 = DateTime.Now.ToString("yyyy/MM/dd");
            string allstrings = "";
            if (load_date.Length > 5)
            {
                Console.WriteLine("There is date");
                str1 = load_date;
            }
            Console.WriteLine("Takaenda");
            using (var sftp = new SftpClient(HostName, UserName, Password))
            {
                sftp.Connect();
                var files = sftp.ListDirectory(remotePath);

                foreach (var file in files)
                {
                    string remoteFileName = file.Name;
                    string sftpdate = DateTime.Parse(file.LastWriteTime.Date.ToString()).ToString("yyyy/MM/dd");
                   
if (sftpdate.IndexOf(str1.ToString()) >= 0 && file.FullName.IndexOf("/.") < 0)
                        {

                        using (Stream file1 = File.Create(localPath + remoteFileName))
                        {
                            allstrings += "," + remoteFileName;
                            Console.WriteLine(file.LastWriteTime.Date + "---" + remoteFileName);                   
                            sftp.DownloadFile(remotePath+@"/"+remoteFileName, file1);
                        }
                    }
                }

            }
            db.updateAAA(ftpid,allstrings);
        }

    }
}
