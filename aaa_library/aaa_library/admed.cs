using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using WinSCP;
using System.Windows.Forms;
using System.IO;
namespace aaa_library
{
    public class admed
    {
        public const string host = "eft.mmiholdings.co.za";
        public const string user = "GR_Admed_MedClaimAssist";
        public const string pass = "LI7faMr5p6Xz3u89VngirI6w5dLWIfdrFnczq8lM";

        public const string key = "ssh-rsa 1024 dWjOktpLmtMQ/bb2Xo5yPwOB3SZvW/3mpGMQZZ3N2ps=";

        private static void dbCheck(
      string claim,
      string name,
      string surname,
      string[] files,
      string folder,
      int client_id,
      string policy,
      string myFolder,
      string username)
        {
            Console.WriteLine("Starting loading");
            try
            {
                using (DBConnect db = new DBConnect())
                {
                    int count = db.getDuplicate1(claim, client_id).Tables[0].Rows.Count;
                    Console.WriteLine(claim + "----" + count.ToString());
                    if (count < 1)
                    {
                        if (db.getEmail(username).Tables[0].Rows.Count <= 0)
                            return;
                        db.InsertMember(client_id, policy, name, surname);
                        string recepient = db.getEmail(username).Tables[0].Rows[0]["email"].ToString();
                        Thread.Sleep(3);
                        db.member_id = int.Parse(db.getMember(policy).Tables[0].Rows[0]["member_id"].ToString());
                        Thread.Sleep(3);
                        db.InsertClaim(db.member_id, claim, username, myFolder, open: 5, preassessor: "Keasha");
                        Thread.Sleep(3);
                        db.myClaim_id = int.Parse(db.getDuplicate(claim, client_id).Tables[0].Rows[0]["claim_id"].ToString());
                        Thread.Sleep(3);
                        string patient_name = name + " " + surname;
                        db.InsertPatient(db.myClaim_id, patient_name);
                        Thread.Sleep(3);
                        int num = ((IEnumerable<string>)files).Count<string>();
                        if (num > 0)
                        {
                            for (int index = 0; index < num; ++index)
                                MySftp.kxlUpload(claim, files[index], folder, db, 1);
                            new admed().moveFile("/AwaitingNegotiation", "/Negotiating/", myFolder);
                        }
                        myStaticMethods.sendMail(recepient, claim, policy);
                        Console.WriteLine("End");
                    }
                    else
                    {
                        db.myClaim_id = int.Parse(db.getDuplicate1(claim, client_id).Tables[0].Rows[0]["claim_id"].ToString());
                        int num = ((IEnumerable<string>)files).Count<string>();
                        if (num > 0)
                        {
                            for (int index = 0; index < num; ++index)
                                MySftp.kxlUpload(claim, files[index], folder, db, 1);
                            new admed().moveFile("/AwaitingNegotiation", "/Negotiating/", myFolder);
                        }
                        Console.WriteLine("This is a duplicate please on");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("There is ab err " + ex.Message);
            }
        }

        public static void myAdmed()
        {
            try
            {
                SessionOptions sessionOptions1 = new SessionOptions()
                {
                    Protocol = Protocol.Sftp,
                    HostName = "eft.mmiholdings.co.za",
                    UserName = "GR_Admed_MedClaimAssist",
                    Password = "LI7faMr5p6Xz3u89VngirI6w5dLWIfdrFnczq8lM",
                    GiveUpSecurityAndAcceptAnySshHostKey = true
                };
                using (Session session = new Session())
                {
                    sessionOptions1.GiveUpSecurityAndAcceptAnySshHostKey = true;
                    session.Open(sessionOptions1);
                    string path = "/AwaitingNegotiation";
                    RemoteDirectoryInfo remoteDirectoryInfo = session.ListDirectory(path);
                    remoteDirectoryInfo.Files.Count<RemoteFileInfo>();
                    List<string> stringList = new List<string>();
                    string str1 = DateTime.Now.ToString("yyyy/MM/dd");
                    foreach (RemoteFileInfo file in remoteDirectoryInfo.Files)
                    {
                        if (file.LastWriteTime.ToString().IndexOf(str1.ToString()) >= 0)
                        {
                            string str2 = file.Name.Trim();
                            if (str2.IndexOf('_') > 0)
                            {
                                string[] strArray = str2.Split('_');
                                string claim_number = strArray[1].Trim().Replace(" ", "");
                                if (strArray[0].IndexOf(",") >= 0 && !admed.checknotinDB(claim_number))
                                    stringList.Add(file.Name);
                            }
                        }
                    }
                    string[] array = stringList.ToArray();
                    Console.WriteLine("Reasy to start now");
                    Thread.Sleep(2000);
                    SessionOptions sessionOptions2 = sessionOptions1;
                    string hlf = path;
                    admed.downloadFiles(array, sessionOptions2, hlf);
                }
                Thread.Sleep(2000);
            }
            catch (Exception ex)
            {
                myStaticMethods.plainMailSender("tendai@medclaimassist.co.za", "Admed Error, please attend", ex.ToString());
                throw;
            }
        }

        public static void downloadFiles(
          string[] availClaims,
          SessionOptions sessionOptions,
          string hlf)
        {
            int num1 = ((IEnumerable<string>)availClaims).Count<string>();
            Console.WriteLine("Total claims Vailable : " + num1.ToString());
            for (int index1 = 0; index1 < num1; ++index1)
            {
                Console.WriteLine("Claim Position : " + index1.ToString());
                using (Session session = new Session())
                {
                    string remotePath = hlf + "/" + availClaims[index1] + "/";
                    string str1 = "C:\\xc\\" + availClaims[index1].Trim() + "\\";
                    string myFolder = availClaims[index1].Trim();
                    session.Open(sessionOptions);
                    TransferOperationResult files = session.GetFiles(remotePath, str1, options: new TransferOptions()
                    {
                        TransferMode = TransferMode.Binary
                    });
                    files.Check();
                    List<string> stringList = new List<string>();
                    foreach (FileOperationEventArgs transfer in files.Transfers)
                    {
                        string[] source = transfer.FileName.Split('/');
                        string str2 = source[((IEnumerable<string>)source).Count<string>() - 1];
                        stringList.Add(str2);
                    }
                    string[] array = stringList.ToArray();
                    Thread.Sleep(5000);
                    string[] strArray1 = availClaims[index1].Trim().Split('_');
                    string username = strArray1[0].Trim().Replace(" ", "").Split(',')[1].Trim().Replace(" ", "");
                    string claim = strArray1[1].Trim();
                    string[] strArray2 = strArray1[2].Trim().Split(',');
                    string surname = strArray2[0];
                    string[] source1 = strArray2[1].Split(' ');
                    int num2 = ((IEnumerable<string>)source1).Count<string>();
                    string str3 = "";
                    for (int index2 = 0; index2 < num2 - 1; ++index2)
                    {
                        if (!(source1[index2] == ""))
                            str3 = str3 + source1[index2] + " ";
                    }
                    string policy = "";
                    string name = str3.Trim();
                    Console.WriteLine(claim + "----" + surname + "--------------" + username);
                    admed.dbCheck(claim, name, surname, array, str1, 6, policy, myFolder, username);
                    Console.WriteLine("End of Claim : " + claim);
                    Thread.Sleep(1000);
                }
                Console.WriteLine("Total : " + num1.ToString());
                Console.WriteLine("End;;;;;; : ");
            }
        }

        public static bool checknotinDB(string claim_number)
        {
            using (DBConnect dbConnect = new DBConnect())
                return dbConnect.getDuplicate(claim_number, 6).Tables[0].Rows.Count < 1;
        }

        public void moveFile(string from, string to, string folder)
        {
            try
            {
                SessionOptions sessionOptions = new SessionOptions()
                {
                    Protocol = Protocol.Sftp,
                    HostName = "eft.mmiholdings.co.za",
                    UserName = "GR_Admed_MedClaimAssist",
                    Password = "LI7faMr5p6Xz3u89VngirI6w5dLWIfdrFnczq8lM",
                    GiveUpSecurityAndAcceptAnySshHostKey = true
                };
                using (Session session = new Session())
                {
                    sessionOptions.GiveUpSecurityAndAcceptAnySshHostKey = true;
                    session.Open(sessionOptions);
                    string path = from;
                    string str1 = to;
                    session.ListDirectory(path);
                    session.ListDirectory(str1);
                    session.ListDirectory(path);
                    session.ListDirectory(str1);
                    string str2 = "/" + folder;
                    session.MoveFile(path + str2, str1);
                }
            }
            catch (Exception ex)
            {
                myStaticMethods.plainMailSender("tendai@medclaimassist.co.za", "Admed Error, please attend", ex.ToString());
            }
        }

        public void finalised()
        {
            using (DBConnect dbConnect = new DBConnect())
            {
                string from = "/Negotiating";
                string to = "/Finalised/";
                admed admed = new admed();
                int count = dbConnect.getToFinal().Tables[0].Rows.Count;
                Console.WriteLine("Number " + count.ToString());
                for (int index = 0; index < count; ++index)
                {
                    try
                    {
                        Thread.Sleep(3000);
                        string folder = dbConnect.getToFinal().Tables[0].Rows[index]["my_folder"].ToString();
                        int id = int.Parse(dbConnect.getToFinal().Tables[0].Rows[index]["claim_id"].ToString());
                        Console.WriteLine(folder + "===" + index.ToString() + "---------" + id.ToString());
                        admed.moveFile(from, to, folder);
                        Thread.Sleep(3);
                        dbConnect.UpdateFolder(id);
                        Console.WriteLine("Done and end");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error her : " + ex.Message);
                    }
                }
            }
        }

        public void csvDownload(string folder)
        {
            SessionOptions sessionOptions = new SessionOptions()
            {
                Protocol = Protocol.Sftp,
                HostName = "eft.mmiholdings.co.za",
                UserName = "GR_Admed_MedClaimAssist",
                Password = "LI7faMr5p6Xz3u89VngirI6w5dLWIfdrFnczq8lM",
                GiveUpSecurityAndAcceptAnySshHostKey = true
            };
            using (Session session = new Session())
            {
                Console.WriteLine("Connected Now");
                sessionOptions.GiveUpSecurityAndAcceptAnySshHostKey = true;
                session.Open(sessionOptions);
                string remotePath = "/ClaimFiles/"+ folder + "/";
                string localPath = "C:\\xc\\files\\Downloads\\Admed\\" + folder +"\\";
                string localqueryPath = @"C:\xc\files\Downloads\Admed\" + folder;
            
                /*if (!File.Exists(localqueryPath))
                {
                    var filecreated = File.Create(localqueryPath);
                    filecreated.Close();
                }
                */
              
                Console.WriteLine("Inside");
               TransferOperationResult files = session.GetFiles(remotePath, localPath, options: new TransferOptions()
                {
                    TransferMode = TransferMode.Binary
                });
                files.Check();
                Console.WriteLine("Get Files");
                foreach (FileOperationEventArgs transfer in files.Transfers)
                    Console.WriteLine("Download of {0} succeeded", (object)transfer.FileName);
              
                Thread.Sleep(1000);
            }
            Console.WriteLine("End;;;;;; : ");
        }
        public void test56()
        {
            //MessageBox.Show("myFunction");
        }
    }
}
