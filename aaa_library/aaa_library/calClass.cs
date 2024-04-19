using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.Text.RegularExpressions;
namespace aaa_library
{
    public class calClass : IDisposable
    {
        bool disposed = false;
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
        public void statement(string myBody)
        {

            using (DBConnect db = new DBConnect())
            {
                string id = db.getDetails().Tables[0].Rows[0]["id"].ToString();
                int user_id = int.Parse(id);
                int client_id = 2;
                string policy_number = "";
                string claim_number = "";
                string member_name = "";
                string member_surname = "";
                string patient_name = "";
                string username = db.getDetails().Tables[0].Rows[0]["username"].ToString();
                string email = db.getDetails().Tables[0].Rows[0]["email"].ToString();
                policy_number = myStaticMethods.Between(myBody, "Policy Number:", "Claim Number:");
                claim_number = myStaticMethods.Between(myBody, "Claim Number:", "Principal Member:");
                member_name = myStaticMethods.Between(myBody, "Principal Member:", "Patient:");
                patient_name = myStaticMethods.Between(myBody, "Patient:", "The above claim has been");
                claim_number = (myStaticMethods.trimM(claim_number));
                policy_number = (myStaticMethods.trimM(policy_number));
                policy_number = myStaticMethods.XmlDecode(policy_number);
                claim_number = myStaticMethods.XmlDecode(claim_number);
                member_name = myStaticMethods.XmlDecode(member_name);
                patient_name = myStaticMethods.XmlDecode(patient_name);
                member_name = (myStaticMethods.trimM(member_name));
                patient_name = (myStaticMethods.trimM(patient_name));
                string xman = "";

                int num = db.getDuplicate1(claim_number,client_id).Tables[0].Rows.Count;

                if (num < 1)
                {

                    int pos = policy_number.IndexOf("MED");
                    int pos2 = policy_number.IndexOf("H");
                    // int value;
                    // || int.TryParse(policy_number, out value)

                    if (pos >= 0 || pos2 == 0)
                    {
                        client_id = 5;
                    }
                    if (member_name.IndexOf(',') < 0)
                    {
                        member_name += ",null";
                    }
                    string[] fullName = (myStaticMethods.trimM(member_name)).Split(',');
                    member_surname = fullName[0].Trim();
                    string name = fullName[1].Trim();

                    string myDoc = claim_number.Replace("/", "-");
                    ftpDocs.startDoc(myDoc);
                    patient_name = (myStaticMethods.trimM(patient_name));
                    //DateTime dtt = Convert.ToDateTime("1111-11-11");

                    int num1 = db.getMember1(policy_number,client_id).Tables[0].Rows.Count;

                    if (num1 < 1)
                    {
                        db.InsertMember(client_id, policy_number, name, member_surname);
                    }

                    Thread.Sleep(3);
                    db.member_id = int.Parse(db.getMember1(policy_number,client_id).Tables[0].Rows[0]["member_id"].ToString());


                    Thread.Sleep(3);

                    db.InsertClaim(db.member_id, claim_number, username);
                    Thread.Sleep(3);
                    db.myClaim_id = int.Parse(db.getDuplicate1(claim_number, client_id).Tables[0].Rows[0]["claim_id"].ToString());
                    Thread.Sleep(3);
                    //MessageBox.Show(db.myClaim_id.ToString());
                    db.InsertPatient(db.myClaim_id, patient_name);
                    Thread.Sleep(3);
                    //db.Insert(user_id, client_id, policy_number, claim_number, name, member_surname, patient_name, username, "", 0, 0, 0, dtt);
                    // db.Insert(user_id, 3, policy, claim, "null", "null", "null", username, "", 0, 0, 0, dtt);
                    db.Update(user_id);
                    myStaticMethods.sendMail(email, claim_number, policy_number);

                    MySftp.testDrive(db.myClaim_id);
                    Thread.Sleep(1000);

                }
                else
                {
                    db.Logs("aaa", claim_number);
                }

            }

        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;
            if (disposing)
            {
                handle.Dispose();
            }
            disposed = true;
        }

    }

    public static class myStaticMethods
    {
        public static string Between(this string value, string a, string b)
        {
            int posA = value.IndexOf(a);
            int posB = value.LastIndexOf(b);
            if (posA == -1)
            {
                return "";
            }
            if (posB == -1)
            {
                return "";
            }
            int adjustedPosA = posA + a.Length;
            if (adjustedPosA >= posB)
            {
                return "";
            }
            return value.Substring(adjustedPosA, posB - adjustedPosA);
        }



        public static string trimM(string str)
        {
            return str.Replace("*", " ").Trim();
        }
        public static string StripHtml(string source)
        {
            string output;

            //get rid of HTML tags
            output = Regex.Replace(source, "<[^>]*>", string.Empty);

            //get rid of multiple blank lines
            output = Regex.Replace(output, @"^\s*$\n", string.Empty, RegexOptions.Multiline);

            return output;
        }
        public static void sendMail(string recepient, string numbr, string numbr2)
        {
            using (DBConnect db = new DBConnect())
            {
                var message = new MailMessage(db.getConfigs().Tables[0].Rows[0]["email"].ToString(), recepient);
                message.Subject = "New Claim loaded - " + numbr + "(" + numbr2 + ")";
                message.Body = "You have received a new claim";
                //MailAddress copy = new MailAddress(db.getConfigs().Tables[0].Rows[0]["cc"].ToString());
                //message.CC.Add(copy);
                //Attachment attachment;               
                Thread.Sleep(20);
                //string ppath = @db.getConfigs().Tables[0].Rows[0]["destination_folder"].ToString() + ftpDocs.ambledownFolder + ftpDocs.getName;
                //attachment = new Attachment(ppath);
                //message.Attachments.Add(attachment);
                using (SmtpClient mailer = new SmtpClient("smtp.gmail.com", 587))
                {
                    mailer.Credentials = new NetworkCredential(db.getConfigs().Tables[0].Rows[0]["email"].ToString(), db.getConfigs().Tables[0].Rows[0]["password"].ToString());
                    mailer.EnableSsl = true;
                    mailer.Send(message);
                }
            }

        }

        public static void generalMailSender(string recepient, string subject, string body)
        {
            using (DBConnect db = new DBConnect())
            {


                var message = new MailMessage(db.getConfigs().Tables[0].Rows[0]["email"].ToString(), recepient);
                message.Subject = subject;
                message.Body = body;

                MailAddress copy = new MailAddress(db.getConfigs().Tables[0].Rows[0]["cc"].ToString());
                message.CC.Add(copy);
                using (SmtpClient mailer = new SmtpClient("smtp.gmail.com", 587))
                {
                    mailer.Credentials = new NetworkCredential(db.getConfigs().Tables[0].Rows[0]["email"].ToString(), db.getConfigs().Tables[0].Rows[0]["password"].ToString());
                    mailer.EnableSsl = true;
                    mailer.Send(message);
                }
            }

        }

        public static void simpleMailSender(string recepient, string subject, string body)
        {
            using (DBConnect db = new DBConnect())
            {


                var message = new MailMessage(db.getConfigs().Tables[0].Rows[0]["email"].ToString(), recepient);
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;
                MailAddress copy = new MailAddress(db.getConfigs().Tables[0].Rows[0]["cc"].ToString());
                message.CC.Add(copy);
                using (SmtpClient mailer = new SmtpClient("smtp.gmail.com", 587))
                {
                    mailer.Credentials = new NetworkCredential(db.getConfigs().Tables[0].Rows[0]["email"].ToString(), db.getConfigs().Tables[0].Rows[0]["password"].ToString());
                    mailer.EnableSsl = true;
                    mailer.Send(message);
                }
            }

        }
        public static void plainMailSender(string recepient, string subject, string body)
        {
            using (DBConnect db = new DBConnect())
            {


                var message = new MailMessage(db.getConfigs().Tables[0].Rows[0]["email"].ToString(), recepient);
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;
                using (SmtpClient mailer = new SmtpClient("smtp.gmail.com", 587))
                {
                    mailer.Credentials = new NetworkCredential(db.getConfigs().Tables[0].Rows[0]["email"].ToString(), db.getConfigs().Tables[0].Rows[0]["password"].ToString());
                    mailer.EnableSsl = true;
                    mailer.Send(message);
                }
            }

        }

        public static void clinicalMail(string recepient, string subject, string body)
        {
            using (DBConnect db = new DBConnect())
            {


                var message = new MailMessage(db.getConfigs().Tables[0].Rows[0]["notification_email"].ToString(), recepient);
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;
                using (SmtpClient mailer = new SmtpClient("smtp.gmail.com", 587))
                {
                    mailer.Credentials = new NetworkCredential(db.getConfigs().Tables[0].Rows[0]["notification_email"].ToString(), db.getConfigs().Tables[0].Rows[0]["notification_password"].ToString());
                    mailer.EnableSsl = true;
                    mailer.Send(message);
                }
            }

        }

        public static void generalMailSenderWithAttachement(string recepient, string subject, string body, string path, string document,string fullpath)
        {
            using (DBConnect db = new DBConnect())
            {
                var message = new MailMessage(db.getConfigs().Tables[0].Rows[0]["email"].ToString(), recepient);
                message.Subject = subject;
                message.Body = body;
                MailAddress copy = new MailAddress(db.getConfigs().Tables[0].Rows[0]["cc"].ToString());
                message.CC.Add(copy);
                Attachment attachment;
                Thread.Sleep(20);
                string ppath = fullpath;
                //MessageBox.Show(ppath);
                attachment = new Attachment(ppath);
                message.Attachments.Add(attachment);
                using (SmtpClient mailer = new SmtpClient("smtp.gmail.com", 587))
                {
                    mailer.Credentials = new NetworkCredential(db.getConfigs().Tables[0].Rows[0]["email"].ToString(), db.getConfigs().Tables[0].Rows[0]["password"].ToString());
                    mailer.EnableSsl = true;
                    mailer.Send(message);
                }
            }

        }
        public static void plainMailSender1(string recepient, string subject, string body)
        {
            using (DBConnect dbConnect = new DBConnect())
            {
                MailMessage message = new MailMessage("info@medclaimassist.co.za", recepient);
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;
                using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtpClient.Credentials = (ICredentialsByHost)new NetworkCredential("info@medclaimassist.co.za", "jFRd7=SX");
                    smtpClient.EnableSsl = true;
                    smtpClient.Send(message);
                }
            }
        }
        public static void copyAccount(string recepient,string recepient1, string subject, string body)
        {
            using (DBConnect db = new DBConnect())
            {
                var message = new MailMessage(db.getConfigs().Tables[0].Rows[0]["email"].ToString(), recepient);
                message.Subject = subject;
                message.Body = body;              
                message.IsBodyHtml = true;
                MailAddress copy = new MailAddress(recepient1);
                message.CC.Add(copy);
               
                using (SmtpClient mailer = new SmtpClient("smtp.gmail.com", 587))
                {
                    mailer.Credentials = new NetworkCredential(db.getConfigs().Tables[0].Rows[0]["email"].ToString(), db.getConfigs().Tables[0].Rows[0]["password"].ToString());
                    mailer.EnableSsl = true;
                    mailer.Send(message);
                }
            }

        }
        public static void copyMultiple(string copies,string subject, string body)
        {
            using (DBConnect db = new DBConnect())
            {                
                string[] cop = copies.Split(',');
               var message = new MailMessage(db.getConfigs().Tables[0].Rows[0]["email"].ToString(), cop[0]);
               
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;
                for(int i = 0; i < cop.Length; i++)
                {
                    MailAddress copy = new MailAddress(cop[i]);
                    message.CC.Add(copy);
                }              

                using (SmtpClient mailer = new SmtpClient("smtp.gmail.com", 587))
                {
                    mailer.Credentials = new NetworkCredential(db.getConfigs().Tables[0].Rows[0]["email"].ToString(), db.getConfigs().Tables[0].Rows[0]["password"].ToString());
                    mailer.EnableSsl = true;
                    mailer.Send(message);
                }
            }

        }

        public static void Configs(TextBox email, TextBox password, TextBox source, TextBox destination, TextBox smtp, TextBox imap, TextBox cc)
        {
            using (DBConnect db = new DBConnect())
            {
                email.Text = db.getConfigs().Tables[0].Rows[0]["email"].ToString();
                password.Text = db.getConfigs().Tables[0].Rows[0]["password"].ToString();
                source.Text = db.getConfigs().Tables[0].Rows[0]["source_folder"].ToString();
                destination.Text = db.getConfigs().Tables[0].Rows[0]["destination_folder"].ToString();
                smtp.Text = db.getConfigs().Tables[0].Rows[0]["smtp_server"].ToString();
                imap.Text = db.getConfigs().Tables[0].Rows[0]["imap_server"].ToString();
                cc.Text = db.getConfigs().Tables[0].Rows[0]["cc"].ToString();
            }
        }

        public static string XmlDecode(string value)
        {

            return Regex.Replace(value, "<.*?>", String.Empty);

        }


        public static bool InternetCon()
        {
            try
            {
                System.Net.NetworkInformation.Ping pingSender = new System.Net.NetworkInformation.Ping();
                System.Net.NetworkInformation.PingReply reply = pingSender.Send("gmail.com");
                if (reply.Status == System.Net.NetworkInformation.IPStatus.Success)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static int CheckOccurrences(string str1, string pattern)
        {
            int count = 0;
            int a = 0;

            while ((a = str1.IndexOf(pattern, a)) != -1)
            {
                a += pattern.Length;
                count++;
            }
            return count;
        }
        public static string Calculatex(string mystadate, string myenddate)
        {
            //01-01","03-22","04-19","04-27","05-01","06-17","08-09","09-24","12-16","12-25","12-26
            List<DateTime> holidays = new List<DateTime>();
            // Manually adding all holiday list.
            holidays.Add(new DateTime(DateTime.Now.Year, 1, 1));
            holidays.Add(new DateTime(DateTime.Now.Year, 03, 22));
            holidays.Add(new DateTime(DateTime.Now.Year, 04, 19));
            holidays.Add(new DateTime(DateTime.Now.Year, 04, 27));
            holidays.Add(new DateTime(DateTime.Now.Year, 05, 01));
            holidays.Add(new DateTime(DateTime.Now.Year, 06, 17));
            holidays.Add(new DateTime(DateTime.Now.Year, 08, 09));
            holidays.Add(new DateTime(DateTime.Now.Year, 09, 24));
            holidays.Add(new DateTime(DateTime.Now.Year, 12, 16));
            holidays.Add(new DateTime(DateTime.Now.Year, 12, 25));
            holidays.Add(new DateTime(DateTime.Now.Year, 12, 26));

            DateTime startDate = Convert.ToDateTime(mystadate);
            DateTime endDate = Convert.ToDateTime(myenddate);
            int days = 0;

            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                if (startDate.DayOfWeek != DayOfWeek.Saturday && startDate.DayOfWeek != DayOfWeek.Sunday && !holidays.Contains(date))
                {
                    days++;
                }
                startDate = startDate.AddDays(1);
            }
            return days.ToString();

        }

    }
}

