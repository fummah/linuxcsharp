using System;
using System.IO;
using aaa_library;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace sendReport
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Starting wait ...");
            myClassv ms = new myClassv();
            ms.ExcecuteSendReport();
            Console.WriteLine("This is done");
            Console.ReadLine();
        }
    }
    public class myClassv
    {
        private string Today = DateTime.Today.ToString("yyyy_MM_dd");


        public void ExcecuteSendReport()
        {
            DBConnect db;

            using (db = new DBConnect())
            {
                var yesterday = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
                DataSet ds = db.fetchClient();
                int num = ds.Tables[0].Rows.Count;
                for (int i = 0; i < num; i++)
                {
                    sendReports(i, db, yesterday, Today,ds);
                }

            }
            db = null;
        }


        private void sendReports(int i, DBConnect db, string yesterday, string todayx,DataSet ds)
        {

            string client_id = ds.Tables[0].Rows[i]["reporting_client_id"].ToString();
            string name = ds.Tables[0].Rows[i]["client_name"].ToString();
            string email = ds.Tables[0].Rows[i]["client_email"].ToString();
            string ftp_server = ds.Tables[0].Rows[i]["ftp_server"].ToString();
            string reporting_method = ds.Tables[0].Rows[i]["reporting_method"].ToString();
            string ftp_username = ds.Tables[0].Rows[i]["ftp_username"].ToString();
            string ftp_password = ds.Tables[0].Rows[i]["ftp_password"].ToString();
            string path = ds.Tables[0].Rows[i]["path"].ToString();
            string fullName = name + "_" + todayx;
            string subject = "Daily Claims Report from Med ClaimAssist-" + todayx;
            string body = "Report Attached.";        
            string ppath = @"files";
            string home = System.Environment.GetEnvironmentVariable("HOME");
            string fullpath = home + @"/files/Reports/" + fullName + ".xlsx";
            Console.WriteLine(email);
            try
            {
                int k = db.excelData(int.Parse(client_id), yesterday).Rows.Count;
                if (k > 0)
                {
                    //dataGridView1.DataSource = db.excelData(int.Parse(client_id), yesterday);

                    Task uploadTask = Task.Factory.StartNew(() =>
                    {
                        if (reporting_method == "ftp")
                        {
                            body = "Your Report has been uploaded onto the FTP server";
                            MySftp.UploadReports(ftp_server, ftp_username, ftp_password, path, fullName, fullpath);
                            myStaticMethods.generalMailSender(email, subject, body);
                            //MessageBox.Show(ftp_server+"====="+email+"====="+client_id+"======="+name);
                        }
                        else
                        {
                            myStaticMethods.generalMailSenderWithAttachement(email, subject, body, ppath, fullName + ".xlsx", fullpath);
                            //MessageBox.Show(ftp_server + "=====" + email + "=====" + client_id + "=======" + name);
                        }

                    }, TaskCreationOptions.LongRunning);
                    uploadTask.Wait();
                    Thread.Sleep(5000);
                    // MessageBox.Show("Excel Created");
                }
                else
                {
                    body = "No report generated.";
                    // MessageBox.Show(body);
                   myStaticMethods.generalMailSender(email, subject, body);
                }

            }
            catch (Exception e)
            {
                myStaticMethods.generalMailSender("tendai@medclaimassist.co.za", "Report Error", name + " Daily Report does not run correctly. Error : " + e.Message);
                //Console.WriteLine("The Error : "+e.ToString());
            }
        }
        public void exe10()
        {

            try
            {
                using (DBConnect db = new DBConnect())
                {
                    int num = db.get10DayClaims().Rows.Count;
                    //Console.WriteLine(num.ToString());
                    foreach (DataRow drow in db.get10DayClaims().Rows)
                    {
                        string value = drow["claim_number"].ToString();
                        string claim_id = drow["claim_id"].ToString();
                        string claim_number = drow["claim_number"].ToString();
                        string policy_number = drow["policy_number"].ToString();
                        string client_name = drow["first_name"].ToString();
                        string client_surname = drow["surname"].ToString();
                        string providers = "";
                        //Console.WriteLine(value);

                        int num1 = db.getClaimDoctors(int.Parse(claim_id)).Rows.Count;

                        foreach (DataRow drow2 in db.getClaimDoctors(int.Parse(claim_id)).Rows)
                        {
                            string practice_number = drow2["practice_number"].ToString();
                            providers += practice_number + ", ";

                        }
                        //Console.WriteLine(providers);
                        string amount = drow["gap"].ToString();
                        string hasDrPaid = drow["hasDrPaid"].ToString();
                        int conDr = int.Parse(hasDrPaid);
                        if (conDr == 1)
                        {
                            hasDrPaid = "YES";
                        }
                        else
                        {
                            hasDrPaid = "NO";
                        }
                        string subject = client_surname + "(" + claim_number + ")";
                        string body = "The claim below has more than 10 days in MedClaim Assist System and is still open.<br><br><table border='1' cellpadding='0' cellspacing='0'><tr><td style='background-color: grey'>Claim Number</td><td>" + claim_number + "</td></tr><tr><td style='background-color: grey'>Client Surname</td><td>" + client_surname + "</td></tr><tr><td style='background-color: grey'>Service provider(s) Number</td><td>" + providers + "</td></tr><tr><td style='background-color: grey'>Amount</td><td>" + amount + "</td></tr><tr><td style='background-color: grey'>Has the client paid the doctor?</td><td>" + hasDrPaid + "</td></tr></table>";
                        myStaticMethods.simpleMailSender("Jennifer.Corin@stratumbenefits.co.za", subject, body);
                        Thread.Sleep(1000);
                        int id = int.Parse(claim_id);
                        // Console.WriteLine(id);

                        db.insert10(id);


                    }


                }


            }
            catch (Exception r)
            {
                Console.WriteLine(r.Message);
                //Console.ReadLine();
            }
            finally
            {

            }
        }

    }

}
