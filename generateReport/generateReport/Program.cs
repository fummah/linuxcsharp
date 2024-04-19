using System;
using GemBox.Spreadsheet;
using System.IO;
using aaa_library;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace generateReport
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Excel Test");

                SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
                Console.WriteLine("Reports are running, please wait.....");
                Thread.Sleep(100);
                myClassv mc = new myClassv();
                mc.OpenCases();
                Thread.Sleep(5000);
                mc.ExcecuteReport();

                Thread.Sleep(50);
                Console.WriteLine("Done");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
            }
            //Console.ReadLine();
        }


    }
    public class myClassv
    {
        private string Today = DateTime.Today.ToString("yyyy_MM_dd");
        private void ExecuteExcel(string client, int id, int len)
        {


            try
            {            
                char[] alpa = this.alphabet();
                var yesterday = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
                string sheetname = "Claims Report";
                string sheetheading = "Daily Claims Report";
                ExcelFile workbook = new ExcelFile();
                ExcelWorksheet worksheet = workbook.Worksheets.Add(sheetname);

                worksheet.Cells["A1"].Value = sheetheading;
                //worksheet.Cells["A1"].Style.Font.Size= 18;
                worksheet.Cells["A1"].Style.Font.Weight = ExcelFont.BoldWeight;

                int rowcount = 2;
                DBConnect db = new DBConnect();
                DataTable dt = db.excelData(id, yesterday, len);
                int colnum = dt.Columns.Count;
                foreach (DataRow datarow in dt.Rows)
                {
                    if (rowcount == 2)
                    {
                        for (int i = 1; i <= colnum; i++)
                        {


                            worksheet.Cells[alpa[i - 1] + "2"].Value = dt.Columns[i - 1].ColumnName;
                            worksheet.Cells[alpa[i - 1] + "2"].Style.Font.Weight = ExcelFont.BoldWeight;

                            Console.WriteLine(db.excelData(id, yesterday, len).Columns[i - 1].ColumnName);
                            Console.WriteLine(i.ToString() + " --- " + colnum.ToString());
                        }

                    }
                    if (rowcount > 2)
                    {
                        string policy_number = datarow["POLICY_NUMBER"].ToString();
                        string claim_number = datarow["CLAIM_NUMBER"].ToString();
                        string intervention_date = datarow["INTERVENTION_DATE"].ToString();
                        string intervention_description = datarow["INTERVENTION_DESCRIPTION"].ToString();
                        string open = datarow["Open"].ToString();
                        string scheme_savings = datarow["SCHEME_SAVINGS"].ToString();
                        string discount_savings = datarow["DISCOUNT_SAVINGS"].ToString();
                        worksheet.Cells["A" + rowcount.ToString()].Value = policy_number;
                        worksheet.Cells["B" + rowcount.ToString()].Value = claim_number;
                        worksheet.Cells["C" + rowcount.ToString()].Value = intervention_date;
                        worksheet.Cells["D" + rowcount.ToString()].Value = intervention_description;
                        worksheet.Cells["E" + rowcount.ToString()].Value = open;
                        worksheet.Cells["F" + rowcount.ToString()].Value = scheme_savings;
                        worksheet.Cells["G" + rowcount.ToString()].Value = discount_savings;                       

                    }
                    rowcount += 1;
                }

                string home = Environment.GetEnvironmentVariable("HOME");
                string path = home + @"/files/Reports/";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                workbook.Save(path + client + ".xlsx");
                Console.WriteLine("DDone");
            }
            catch (Exception ex)
            {

                Console.WriteLine("There is an error on excel here " + ex.Message);
            }
            finally
            {

            }
        }
        public void OpenCases()
        {
            try
            {
                DBConnect db = new DBConnect();
                string email = db.getConfigs().Tables[0].Rows[0]["cc1"].ToString();
                int tot = 0;
                int num = db.OpenCases().Tables[0].Rows.Count;
                string subject = "Individual Open Cases";
                string body = "Below is a list of open cases per individual specialist.<br><br><table border='1' cellpadding='10' cellspacing='0'><tr style='font - weight: bolder'><th>Username</th><th>Number of Open Cases</th></tr>";
                for (int i = 0; i < num; i++)
                {
                    string username = db.OpenCases().Tables[0].Rows[i]["username"].ToString();
                    string open_num = db.OpenCases().Tables[0].Rows[i]["open"].ToString();
                    tot += int.Parse(open_num);
                    body += "<tr><td>" + username + "</td><td>" + open_num + "</td></tr>";

                }
                body += "<tr><th>Total Open Cases</th><th>" + tot.ToString() + "</th></table>";
                myStaticMethods.plainMailSender(email, subject, body);

            }
            catch (Exception e)
            {

            }
        }
        public void ExcecuteReport()
        {
            DBConnect db;

            using (db = new DBConnect())
            {
                var yesterday = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
                int num = db.fetchClient1().Tables[0].Rows.Count;
                for (int i = 0; i < num; i++)
                {

                    clientsExc(i, db, yesterday, Today);
                }

            }
            db = null;
        }


        private void clientsExc(int i, DBConnect db, string yesterday, string todayx)
        {


            string client_id = db.fetchClient1().Tables[0].Rows[i]["reporting_client_id"].ToString();
            string name = db.fetchClient1().Tables[0].Rows[i]["client_name"].ToString();
            string advanced = db.fetchClient1().Tables[0].Rows[i]["advanced"].ToString();


            string fullName = name + "_" + todayx;
            Console.WriteLine(i.ToString() + "-----" + name + "---" + yesterday + "=========");

            try
            {

                int k = db.excelData(int.Parse(client_id), yesterday, int.Parse(advanced)).Rows.Count;
                if (k > 0)
                {

                    Task excelTsk = Task.Factory.StartNew(() =>
                    {

                        ExecuteExcel(fullName, int.Parse(client_id), int.Parse(advanced));
                    }, TaskCreationOptions.LongRunning);
                    excelTsk.Wait();


                }
                Thread.Sleep(2000);

            }
            catch (Exception e)
            {
                myStaticMethods.generalMailSender("tendai@medclaimassist.co.za", "Report Error", name + " Daily Report does not run correctly during creation. Error Code: " + e.Message);
                //textBox1.Text += e.ToString();
            }
        }

        private char[] alphabet()
        {
            char[] arr = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' };
            return arr;
        }

    }
}
