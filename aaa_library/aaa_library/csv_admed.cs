using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;
using System.Data;
using ExcelDataReader;
using System.Data.OleDb;




namespace aaa_library
{
    public class csv_admed
    {
        public string gg1 { get; set; }
        public string gg2 { get; set; }

        public void fetchDetails()
        {
            Console.WriteLine("Dziva vatanga");
            string homepath = Environment.GetEnvironmentVariable("HOME");
            homepath = homepath + @"/files/Admed/";
            string path1 =homepath + DateTime.Now.ToString("yyyy_MM_dd") + @"/MCA_ClaimFile_" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
            string path2 = homepath + DateTime.Now.ToString("yyyy_MM_dd") + @"/MCA_ClaimFile_ " + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";
            string str1 = "";
            Console.WriteLine(path1);
            DateTime now;
            if (File.Exists(path1))
            {
                now = DateTime.Now;
                string str2 = now.ToString("yyyy_MM_dd");
                now = DateTime.Now;
                string str3 = now.ToString("yyyyMMdd");
                str1 = str2 + @"/MCA_ClaimFile_" + str3 + ".xlsx";
            }
            else if (File.Exists(path2))
            {
                now = DateTime.Now;
                string str4 = now.ToString("yyyy_MM_dd");
                now = DateTime.Now;
                string str5 = now.ToString("yyyyMMdd");
                str1 = str4 + @"/MCA_ClaimFile_ " + str5 + ".xlsx";
            }
            else
                Environment.Exit(0);
            OleDbConnection oleDbConnection = new OleDbConnection("provider=Microsoft.ACE.OLEDB.12.0;Data Source='"+homepath + str1 + "';Extended Properties='Excel 12.0;HDR=YES;';");
            oleDbConnection.Open();
            OleDbCommand oleDbCommand1 = new OleDbCommand();
            oleDbCommand1.Connection = oleDbConnection;
            oleDbCommand1.CommandText = "SELECT * from [Sheet1$]";
            OleDbDataReader reader = oleDbCommand1.ExecuteReader();
            List<Admed_One> source = new List<Admed_One>();
            Console.WriteLine("Step 1545" + reader.Read().ToString());
            while (reader.Read())
            {
                if (reader[0].ToString() != "" && reader[0].ToString() != "ClaimNumber")
                {
                    string str6 = this.chechInput1(reader[0].ToString());
                    string str7 = this.chechInput1(reader[24].ToString());
                    string s1 = this.chechAmount1(reader[25].ToString());
                    string s2 = this.chechAmount1(reader[26].ToString());
                    string s3 = this.chechAmount1(reader[27].ToString());
                    double num1 = double.Parse(s1);
                    double num2 = double.Parse(s2);
                    double num3 = double.Parse(s3);
                    source.Add(new Admed_One()
                    {
                        claim_number = str6,
                        practice_number = str7,
                        charged_amnt = num1,
                        scheme = num2,
                        gap = num3
                    });
                }
            }
            List<Admed_One> list = source.GroupBy(l => new
            {
                claim_number = l.claim_number,
                practice_number = l.practice_number
            }).Select(cl => new Admed_One()
            {
                claim_number = cl.First<Admed_One>().claim_number,
                practice_number = cl.First<Admed_One>().practice_number,
                charged_amnt = cl.First<Admed_One>().charged_amnt,
                scheme = cl.First<Admed_One>().scheme,
                gap = cl.First<Admed_One>().gap
            }).ToList<Admed_One>().GroupBy<Admed_One, string>((Func<Admed_One, string>)(l => l.claim_number)).Select<IGrouping<string, Admed_One>, Admed_One>((Func<IGrouping<string, Admed_One>, Admed_One>)(cl => new Admed_One()
            {
                claim_number = cl.First<Admed_One>().claim_number,
                charged_amnt = cl.Sum<Admed_One>((Func<Admed_One, double>)(c => c.charged_amnt)),
                scheme = cl.Sum<Admed_One>((Func<Admed_One, double>)(c => c.scheme)),
                gap = cl.Sum<Admed_One>((Func<Admed_One, double>)(c => c.gap))
            })).ToList<Admed_One>();
            int count1 = list.Count;
            for (int index = 0; index < count1; ++index)
                Console.WriteLine("----------------->" + list[index].claim_number + "----" + count1.ToString());
            using (DBConnect dbConnect = new DBConnect())
            {
                int num4 = 0;
                int num5 = 0;
                int num6 = 0;
                int num7 = 0;
                int num8 = 0;
                int num9 = 0;
                int num10 = 0;
                int num11 = 0;
                int num12 = 0;
                int num13 = 0;
                int num14 = 0;
                int num15 = 0;
                int num16 = 0;
                string str8 = "";

                OleDbCommand select1 = new OleDbCommand();
                select1.Connection = oleDbConnection;
                select1.CommandText = "SELECT * from [Sheet1$]";               
                reader = select1.ExecuteReader();
                //DateTime?xdate = null;
                while (reader.Read())
                {
                    //Console.WriteLine("Step 2");
                    if (reader[0].ToString() != "")
                    {
                        //Console.WriteLine("Step 3");
                        if (reader[0].ToString() != "ClaimNumber")
                        {
                            //Console.WriteLine("Step 4");
                            try
                            {
                                int client_id = 6;
                                ///Console.WriteLine("Step 11 "+ reader[21]);
                                string username = this.chechInput1(reader[21].ToString()).Split(',')[1].Replace(" ", "");
                                string xp = dbConnect.getDetails().Tables[0].Rows.Count.ToString();
                                //Console.WriteLine("Step 14 - "+xp);
                                string s4 = dbConnect.getDetails().Tables[0].Rows[0]["id"].ToString();
                                //Console.WriteLine("Step 344");
                                string recepient = dbConnect.getDetails().Tables[0].Rows[0]["email"].ToString();
                                //Console.WriteLine("Step Dziva "+ recepient);
                                int id1 = int.Parse(s4);
                                string numbr2 = this.chechInput1(reader[0].ToString());
                                string policy_number = this.chechInput1(reader[1].ToString());
                                string member_name = this.chechInput1(reader[9].ToString());
                                string member_surname = this.chechInput1(reader[10].ToString());
                                string cell = this.chechInput1(reader[11].ToString());
                                string telephone = this.chechInput1(reader[12].ToString());
                                string email = this.chechInput1(reader[14].ToString());
                                string product_name = this.chechInput1(reader[2].ToString());
                                string product_code = this.chechInput1(reader[3].ToString());
                                string benefitiary_number = this.chechInput1(reader[7].ToString());
                                string str9 = this.chechInput1(reader[15].ToString());
                                string medical_aid = "Unknown";
                                if (str9 == "Discovery Health")
                                    medical_aid = "Discovery Health Medical Scheme";
                                else if (str9 == "LA Health")
                                    medical_aid = "LA-Health Medical Scheme";
                                else if (str9 == "Momentum Health")
                                    medical_aid = "Momentum Health";
                                else if (str9 == "BestMed Medical Scheme")
                                    medical_aid = "Bestmed Medical Scheme";
                                string scheme_option = this.chechInput1(reader[16].ToString());
                                string scheme_number = this.chechInput1(reader[17].ToString());
                                string str10 = this.chechInput1(reader[0].ToString());
                                string s5 = this.chechAmount1(reader[18].ToString());
                                string s6 = this.chechAmount1(reader[19].ToString());
                                string s7 = this.chechAmount1(reader[20].ToString());
                                string icd10 = this.chechInput1(reader[29].ToString());
                                string icd10_descr = this.chechInput1(reader[30].ToString());
                                int count2 = dbConnect.getPMBstatus(icd10).Tables[0].Rows.Count;
                                Console.WriteLine("Step Fuma " + count2.ToString());
                                int pmb = 0;
                                if (count2 > 0 && dbConnect.getPMBstatus(icd10).Tables[0].Rows[0]["pmb_code"].ToString().Trim().Length >= 1)
                                    pmb = 1;
                                double amount_charged = double.Parse(s5);
                                double scheme_paid = double.Parse(s6);
                                double.Parse(s7);
                                double.Parse(s5);
                                double.Parse(s6);
                                double gap1 = double.Parse(s7);
                                for (int index = 0; index < count1; ++index)
                                {
                                    if (list[index].claim_number == str10)
                                    {
                                        double chargedAmnt = list[index].charged_amnt;
                                        double scheme = list[index].scheme;
                                        gap1 = list[index].gap;
                                    }
                                }
                                string[] strArray1 = this.chechInput1(reader[4].ToString()).Split('/');
                                string[] strArray2 = this.chechInput1(reader[8].ToString()).Split('/');
                                string patient_number = strArray1[1];
                                string id_number = strArray2[1];
                                string patient_name = this.chechInput1(reader[5].ToString());
                                string patient_surname = this.chechInput1(reader[6].ToString());
                                string str13 = this.chechInput1(reader[24].ToString()).PadLeft(7, '0');
                                string str14 = this.chechInput1(reader[22].ToString()) + " " + this.chechInput1(reader[23].ToString());
                                string str15 = this.chechInput1(reader[32].ToString());
                                string treatment_descr = this.chechInput1(reader[33].ToString());
                                string str16 = this.chechInput1(reader[31].ToString());
                                string str17 = this.chechInput1(reader[31].ToString());
                                string s8 = this.chechAmount1(reader[34].ToString());
                                string s9 = this.chechAmount1(reader[35].ToString());
                                string s10 = this.chechAmount1(reader[36].ToString());
                                string s11 = this.chechAmount1(reader[25].ToString());
                                string s12 = this.chechAmount1(reader[26].ToString());
                                string s13 = this.chechAmount1(reader[27].ToString());
                                string modifier1 = this.chechInput1(reader[37].ToString());
                                string str18 = this.chechInput1(reader[38].ToString());
                                string modifier_charged = this.chechInput1(reader[39].ToString());
                                string modifier_claimable = this.chechInput1(reader[40].ToString());
                                str8 = s8 + "---------" + s9 + str10;
                                double num17 = double.Parse(s8);
                                double num18 = double.Parse(s9);
                                double gap2 = double.Parse(s10);
                                double charged = double.Parse(s11);
                                double scheme1 = double.Parse(s12);
                                double gap3 = double.Parse(s13);
                                string treatmentdate1 = str16.Replace('/', '-').Replace("00:00:00", "").Replace(" ", "");
                                string treatmentdate2 = str17.Replace('/', '-').Replace("00:00:00", "").Replace(" ", "");
                                DateTime dateTime1 = Convert.ToDateTime(treatmentdate1);
                                int reopened = 1;
                                if (dbConnect.getMember1(policy_number, client_id).Tables[0].Rows.Count < 1)
                                {
                                    dbConnect.InsertMember(client_id, policy_number, member_name, member_surname, telephone, email, medical_aid, cell, product_name, product_code, benefitiary_number, scheme_option, scheme_number, id_number);
                                    Console.WriteLine("New Member loaded" + policy_number);
                                    ++num6;
                                }
                                else
                                {
                                    Console.WriteLine("Duplicate member" + policy_number);
                                    ++num7;
                                }
                                Thread.Sleep(3);
                                
                                dbConnect.member_id = int.Parse(dbConnect.getMember1(policy_number, client_id).Tables[0].Rows[0]["member_id"].ToString());
                                
                                int count3 = dbConnect.getDuplicate1(str10, client_id).Tables[0].Rows.Count;
                                
                                if (count3 < 1)
                                {
                                    ++num4;
                                    dbConnect.InsertClaim(dbConnect.member_id, str10, username, service_date: new DateTime?(dateTime1), icd10: icd10, pmb: pmb, amount_charged: amount_charged, client_gap: gap1, icd10_descr: icd10_descr, scheme_paid: scheme_paid, patient_number: patient_number);
                                    dbConnect.Update(id1);
                                    Console.WriteLine("New Claim Loaded " + str10);
                                    ++num8;
                                    myStaticMethods.sendMail(recepient, str10, numbr2);
                                }
                                else
                                {
                                                                        
                                    string cv=dbConnect.getDuplicate1(str10, client_id).Tables[0].Rows[0]["Open"].ToString();                                    
                                   reopened = cv=="False"?0:1;
                                    Console.WriteLine(cv + " -X-X- " + reopened.ToString());
                                    string iccd = dbConnect.getDuplicate1(str10, client_id).Tables[0].Rows[0]["icd10"].ToString();
                                    Console.WriteLine(iccd+" --ICD10 Length- " + iccd.Length.ToString());
                                    if (dbConnect.getDuplicate1(str10, client_id).Tables[0].Rows[0]["icd10"].ToString().Length < 1)
                                    {
                                        //Console.WriteLine("Xtest --- " + dbConnect.member_id.ToString());
                                        dbConnect.myClaim_id = int.Parse(dbConnect.getDuplicate1(str10, client_id).Tables[0].Rows[0]["claim_id"].ToString());
                                        now = DateTime.Now;
                                        //Console.WriteLine("Killer " + count3.ToString());
                                        DateTime dateTime2 = Convert.ToDateTime(now.ToString("yyyy-MM-dd HH:mm:ss"));
                                        dbConnect.updateClaim(dbConnect.myClaim_id, new DateTime?(dateTime2), service_date: new DateTime?(dateTime1), icd10: icd10, pmb: pmb, amount_charged: amount_charged, icd10_descr: icd10_descr, scheme_paid: scheme_paid, gap: gap1, patient_number: patient_number);
                                        dbConnect.updatePatient(dbConnect.myClaim_id, patient_name+" "+patient_surname);
                                        //myStaticMethods.sendMail(recepient, str10, numbr2);
                                    }
                                    Console.WriteLine("Duplicate Claim " + str10);
                                    ++num9;
                                }
                                string str19 = dbConnect.getDuplicate1(str10, client_id).Tables[0].Rows[0]["policy_number"].ToString();
                                string last_date_closed = dbConnect.getDuplicate1(str10, client_id).Tables[0].Rows[0]["date_closed"].ToString();
                                double last_scheme_savings = double.Parse(dbConnect.getDuplicate1(str10, client_id).Tables[0].Rows[0]["savings_scheme"].ToString());
                                double last_discount_savings = double.Parse(dbConnect.getDuplicate1(str10, client_id).Tables[0].Rows[0]["savings_discount"].ToString());
                                Console.WriteLine(last_date_closed+"-----------> " + last_scheme_savings+" --- "+ last_discount_savings);
                                if (str19.Length < 2)
                                {
                                    //Console.WriteLine("Policy Lenth --- > is below 2 ---> " + policy_number);
                                    dbConnect.member_id = int.Parse(dbConnect.getDuplicate1(str10, client_id).Tables[0].Rows[0]["member_id"].ToString());
                                    dbConnect.updateMember(dbConnect.member_id, policy_number, member_name, member_surname, telephone, email, medical_aid, cell, product_name, product_code, benefitiary_number, scheme_option, scheme_number, id_number);
                                }
                                Thread.Sleep(3);
                                dbConnect.myClaim_id = int.Parse(dbConnect.getDuplicate1(str10, client_id).Tables[0].Rows[0]["claim_id"].ToString());
                                //dbConnect.getDuplicate1(str10, client_id).Tables[0].Rows[0]["Open"].ToString();
                                if (count3 < 1)
                                {
                                    dbConnect.InsertPatient(dbConnect.myClaim_id, patient_name + " " + patient_surname);
                                    Console.WriteLine("New patient loaded " + patient_name);
                                    ++num10;
                                }
                                else
                                {
                                    Console.WriteLine("Duplicate Patient " + patient_name);
                                    ++num11;
                                }
                                Thread.Sleep(3);
                                int count4 = dbConnect.check_localDoctor(dbConnect.myClaim_id, str13).Tables[0].Rows.Count;
                                //Console.WriteLine("We are here" + count4.ToString());
                                if (count4 < 1)
                                {
                                    int count5 = dbConnect.checkDoctor_Medp(str13).Tables[0].Rows.Count;
                                    Console.WriteLine("We are here 11" + count5.ToString());
                                    if (count5 < 1)
                                        dbConnect.insertDoctorMedpages(str14, str13);
                                    dbConnect.insertDoctorLocal(dbConnect.myClaim_id, str13, str14, charged, scheme1, gap3);
                                    Console.WriteLine("New Doctor loaded " + str13);
                                    ++num12;
                                }
                                else
                                {
                                    Console.WriteLine("Duplicate Doctor " + str13);
                                    ++num13;
                                }
                                string modifier_name1 = "((Modifier : " + modifier1 + " => Name : " + str18 + " => Charged Amount : " + modifier_charged + " => Claimable Amount : " + modifier_claimable + ")) ";
                                if (dbConnect.checkClaimlineAdmed(dbConnect.myClaim_id, str13, treatmentdate1, str15, num17, num18, gap2).Tables[0].Rows.Count < 1)
                                {
                                    Console.WriteLine("New Claim Line loaded ");
                                    string msg_code = "";
                                    now = DateTime.Now;
                                    string date_reopened = now.ToString("yyyy-MM-dd HH:mm:ss");
                                    if ((num17 < 1.0 || num18 < 1.0) && dbConnect.admedReasonCodes(str15).Tables[0].Rows.Count > 0)
                                    {
                                        msg_code = dbConnect.admedReasonCodes(str15).Tables[0].Rows[0]["BENEFIT"].ToString();
                                    }                                        
                                    dbConnect.insertClaimline(dbConnect.myClaim_id, str13, treatmentdate2, str15, treatment_descr, num17, num18, icd10, icd10_descr, gap2, modifier1, modifier_name1, modifier_charged, modifier_claimable, msg_code);
                                    dbConnect.openClaim(dbConnect.myClaim_id, 1, date_reopened, new DateTime?(dateTime1), dateTime1.ToString());
                                    Console.WriteLine("New Claim Line loaded ");
                                    if (reopened == 0)
                                    {
                                        dbConnect.insertReopenedClaims(dbConnect.myClaim_id, "New Claim Line", "System", last_date_closed, last_scheme_savings, last_discount_savings);
                                    }
                                    
                                    Console.WriteLine("The closed");
                                    ++num14;
                                }
                                else
                                {
                                    if (modifier1.Length > 0)
                                    {
                                        string[] array = dbConnect.checkClaimlineAdmed(dbConnect.myClaim_id, str13, treatmentdate1, str15, num17, num18, gap2).Tables[0].Rows[0]["modifier"].ToString().Split(',');
                                        if (Array.IndexOf<string>(array, modifier1) < 0)
                                        {
                                            string modifier2 = string.Join(",", array) + "," + modifier1;
                                            string modifier_name2 = dbConnect.checkClaimlineAdmed(dbConnect.myClaim_id, str13, treatmentdate1, str15, num17, num18, gap2).Tables[0].Rows[0]["modifier_name"].ToString() + modifier_name1;
                                            int id2 = int.Parse(dbConnect.checkClaimlineAdmed(dbConnect.myClaim_id, str13, treatmentdate1, str15, num17, num18, gap2).Tables[0].Rows[0]["id"].ToString());
                                            dbConnect.updateModifierdetails(id2, modifier2, modifier_name2);
                                        }
                                    }
                                    Console.WriteLine("Duplicate Claim Line " + str15);
                                    ++num15;
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(str8 + "----There is an error " + ex.Message);
                                ++num16;
                            }
                            finally
                            {
                                Console.WriteLine("================================================================Ends here Load");
                            }
                        }
                    }
                }
                Console.WriteLine("-------------------------------------------------Finish-------------------------------------------------------------------");
                Console.WriteLine("Total Lines : " + num5.ToString());
                Console.WriteLine("Total New Members : " + num6.ToString());
                Console.WriteLine("Total Duplicate Member : " + num7.ToString());
                Console.WriteLine("Total New Claims : " + num8.ToString());
                Console.WriteLine("Total Claim Duplicates : " + num9.ToString());
                Console.WriteLine("Total New Patients : " + num10.ToString());
                Console.WriteLine("Total Patient Duplicates : " + num11.ToString());
                Console.WriteLine("Total New Doctors : " + num12.ToString());
                Console.WriteLine("Total Doctor Duplicates : " + num13.ToString());
                Console.WriteLine("Total New Claim Lines : " + num14.ToString());
                Console.WriteLine("Total Claim Lines Duplicates : " + num15.ToString());
                Console.WriteLine("Total Errors : " + num16.ToString());
                myStaticMethods.simpleMailSender("tendai@medclaimassist.co.za", "Admed Claims", string.Format("Total Lines : {0} <br> Total New Members : {1} <br> Total Duplicate Member : {2}<br> Total New Claims : {3} <br> Total Claim Duplicates :{4} <br> Total New Patients : {5} <br> Total Patient Duplicates : {6} <br> Total New Doctors : {7} <br> Total Doctor Duplicates : {8} <br> Total New Claim Lines : {9} <br> Total Claim Lines Duplicates : {10} <br> Total Errors : {11}", (object)num5.ToString(), (object)num6.ToString(), (object)num7.ToString(), (object)num8.ToString(), (object)num9.ToString(), (object)num10.ToString(), (object)num11.ToString(), (object)num12.ToString(), (object)num13.ToString(), (object)num14.ToString(), (object)num15.ToString(), (object)num16.ToString()));
            }
        }

        public string chechInput(int i, Excel.Range xlRange, int j)
        {
            string str = "";
            if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)
            {
                str = xlRange.Cells[i, j].Value2.ToString();
            }

            return str;
        }
        public string chechInput1(string str)
        {

            if (str == "")
            {
                str = "";
            }

            return str;
        }
        private string chechAmount(int i, Excel.Range xlRange, int j)
        {
            string str = "0,0";
            if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)
            {
                str = xlRange.Cells[i, j].Value2.ToString();
                str = str.Replace(',', ',');
            }

            return str;
        }
        public string chechAmount1(string str)
        {

            if (string.IsNullOrEmpty(str))
            {
                str = "0,0";
            }
            else
            {
                str = str.Replace('.', ',');
            }
            return str;
        }

        public void downloadDoc()
        {
            Console.WriteLine("We are sterting now....");
            try
            {
                string tes = "";
                ftpDocs.startDoc2(tes);
            }
            catch (Exception ex)
            {
                Console.WriteLine("There is an error " + ex);
            }
            finally
            {
                Console.WriteLine("pppeduuuu");
            }
        }

        public void cv(string homepath)
        {
   

            string con =
  @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source="+homepath+"MCA_ClaimFile_20220428_v2.xlsx;" +
  @"Extended Properties='Excel 8.0;HDR=Yes;'";
            using (OleDbConnection connection = new OleDbConnection(con))
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand("select * from [Sheet1$]", connection);
                using (OleDbDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        var row1Col0 = dr[0];
                        Console.WriteLine(row1Col0);
                    }
                }
            }
        }
    }

    class Admed_One
    {
        public string claim_number { get; set; }
        public string practice_number { get; set; }
        public double charged_amnt { get; set; }
        public double scheme { get; set; }
        public double gap { get; set; }
    }

    public class Classx
    {
        private DataTable GetDataTable(string sql, string connectionString)
        {
            DataTable dt = null;

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                {
                    using (OleDbDataReader rdr = cmd.ExecuteReader())
                    {
                        dt.Load(rdr);
                        return dt;
                    }
                }
            }
        }

        public void GetExcel(string homepath)
        {
         

            System.Data.OleDb.OleDbConnection MyConnection;        
            MyConnection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.ACE.OLEDB.12.0;Data Source='"+homepath+"MCA_ClaimFile_20220428_v2.xlsx';Extended Properties='Excel 12.0;HDR=YES;';");
            MyConnection.Open();
                   
            OleDbCommand select = new OleDbCommand();
            select.Connection = MyConnection;
            select.CommandText = "SELECT * from [Sheet1$]";
            OleDbDataReader reader = select.ExecuteReader();
           
            while (reader.Read())
            {
                Console.WriteLine(reader[0].ToString() + "," + reader[1].ToString());
            }
         
            MyConnection.Close();
        }
    }
    
}