using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using S22.Imap;
using System.Threading;
using System.IO;

namespace aaa_library
{
    public class ttt
    {
    }



    public class iwyze
    {
        string subject { get; set; }
        string body { get; set; }
        string claim { get; set; }
        public void iwyzemailChecker()
        {
            Console.WriteLine("Fummaa 1");
            using (ImapClient Client = new ImapClient("imap.gmail.com", 993,
          "iwyze@medclaimassist.co.za", "P@ssw0rd!", AuthMethod.Login, true))
            {

                uint[] uids = Client.Search(SearchCondition.Unseen());
                Console.WriteLine("Fummaa 2");
                MailMessage[] messages = Client.GetMessages(uids);
                Console.WriteLine("Fummaa 3");

                foreach (MailMessage mes in messages)
                {

                    Console.WriteLine("-------------tereeeeeeeeeeeeeeeeeeeeee");
                    string sub = mes.Subject;
                    body = mes.Body;
                    body = myStaticMethods.StripHtml(mes.Body).Trim();
                    if (body.IndexOf("Iwyze claim number") >= 0)
                    {
                        try
                        {


                            //Client.CopyMessage(uid, "fummah3@outlook.com");
                            this.subject = myStaticMethods.StripHtml(sub).Trim();
                            body = myStaticMethods.StripHtml(mes.Body).Trim();
                            Console.WriteLine("Fummaa 4");
                            body = body.Substring(0, 700);
                            string subj = this.subject.Replace(" ", "").Replace("(TrialVersion)", "").Replace(":", "").Trim();
                            string cleanbody = body.Replace("-", "").Replace(":", "").Replace("*", "");
                            Console.WriteLine("Fummaa 10");
                            string claim_number = myStaticMethods.Between(cleanbody, "Iwyze claim number", "Policy Number").Trim();
                            string policy_number = myStaticMethods.Between(cleanbody, "Policy Number", "Patient Name").Trim();
                            string patient_firstname = myStaticMethods.Between(cleanbody, "Patient Name", "Patient Surname").Trim();
                            string patient_surname = myStaticMethods.Between(cleanbody, "Patient Surname", "Policy holder Name").Trim();
                            string first_name = myStaticMethods.Between(cleanbody, "Policy holder Name", "Policy holder surname").Trim();
                            Console.WriteLine("Fummaa 5");
                            string surname = myStaticMethods.Between(cleanbody, "Policy holder surname", "Policy holder cell phone number").Trim();
                            string cell = myStaticMethods.Between(cleanbody, "Policy holder cell phone number", "Policy holder email address").Trim();
                            string telephone = "";
                            string email1 = myStaticMethods.Between(cleanbody, "Policy holder email address", "Iwyze Gap amount").Trim();
                            ZestlifeClass zc = new ZestlifeClass();
                            string gap = zc.chechAmount1(myStaticMethods.Between(cleanbody, "Iwyze Gap amount", "Date sent to MCA").Replace("R", "").Trim());
                            double gap1 = double.Parse(gap);
                            string medical_aid = "Unknown";
                            Console.WriteLine(claim_number + "=====" + gap);

                            string pathString = @"C:\xc\iwyze\" + subj + @"\";
                            if (!Directory.Exists(pathString))
                            {
                                Directory.CreateDirectory(pathString);
                            }
                            /*  Task t = Task.Factory.StartNew(() =>
                              {
                                  foreach (Attachment attachment in mes.Attachments)
                                  {
                                      byte[] allBytes = new byte[attachment.ContentStream.Length];
                                      int bytesRead = attachment.ContentStream.Read(allBytes, 0, (int)attachment.ContentStream.Length);
                                      string destinationFile = @pathString + attachment.Name;
                                      BinaryWriter writer = new BinaryWriter(new FileStream(destinationFile, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None));
                                      writer.Write(allBytes);
                                      writer.Close();
                                  }

                              }, TaskCreationOptions.LongRunning);
                              //t.Start();
                              t.Wait();
                             * */
                            Thread.Sleep(500);



                            DBConnect db = new DBConnect();

                            int num = db.getDuplicate1(claim_number,14).Tables[0].Rows.Count;
                            Console.WriteLine("Claim Number " + claim_number);
                            Console.WriteLine("This is available === " + num);
                            if (num < 1)
                            {

                                string id = db.getDetails().Tables[0].Rows[0]["id"].ToString();
                                int user_id = int.Parse(id);
                                int client_id = 14;
                                string username = db.getDetails().Tables[0].Rows[0]["username"].ToString();
                                string email = db.getDetails().Tables[0].Rows[0]["email"].ToString();
                                int num1 = db.getMember1(policy_number,client_id).Tables[0].Rows.Count;
                                Console.WriteLine("I m here now " + num1);
                                if (num1 < 1)
                                {
                                    db.InsertMember(client_id, policy_number, first_name, surname, telephone, email1, medical_aid, cell);

                                }

                                Thread.Sleep(3);
                                db.member_id = int.Parse(db.getMember1(policy_number,client_id).Tables[0].Rows[0]["member_id"].ToString());

                                Console.WriteLine("Memberi id " + db.member_id);
                                Thread.Sleep(3);

                                db.InsertClaim(db.member_id, claim_number, username, "", null, "", 0, 0, gap1);

                                Thread.Sleep(3);
                                db.myClaim_id = int.Parse(db.getDuplicate1(claim_number,14).Tables[0].Rows[0]["claim_id"].ToString());
                                Console.WriteLine("claim id " + db.myClaim_id);
                                Thread.Sleep(3);
                                //MessageBox.Show(db.myClaim_id.ToString());
                                string patient_name = patient_firstname + " " + patient_surname;
                                db.InsertPatient(db.myClaim_id, patient_name);
                                Thread.Sleep(3);
                                db.Update(user_id);

                                myStaticMethods.sendMail(email, claim_number, policy_number);

                            }


                            string[] fileNames = System.IO.Directory.GetFiles(pathString);
                            foreach (string s in fileNames)
                            {
                                System.IO.FileInfo fi = null;
                                try
                                {
                                    fi = new System.IO.FileInfo(s);
                                }
                                catch (System.IO.FileNotFoundException pt)
                                {
                                    Console.WriteLine(pt.Message);

                                    continue;
                                }

                                //MessageBox.Show(charged.ToString());
                                try
                                {
                                    Thread.Sleep(2000);
                                    string fullP = pathString + @"\" + fi.Name;
                                    MySftp.xc(fullP, fi.Name);
                                    db.insertDocument(db.myClaim_id, fi.Name);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("There wwwwwwwwwwwwww" + ex.Message);
                                }


                            }


                        }
                        catch (Exception ex)
                        {

                            Console.WriteLine("there is an error " + ex.Message);


                        }
                        finally
                        {

                            Console.WriteLine("tapedza");
                        }

                    }

                    //MessageBox.Show(uids.Length.ToString());

                }


            }
        }
    }


}
