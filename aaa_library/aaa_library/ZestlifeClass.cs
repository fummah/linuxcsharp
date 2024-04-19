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

namespace aaa_library
{
    public class ZestlifeClass
    {
        public string gg1 { get; set; }
        public string gg2 { get; set; }



 public void fetchDetails()
    {
      Console.WriteLine("Dziva vatanga");
      string[] strArray1 = File.ReadAllLines("C:\\xc\\Zestlife\\PMB_Claims_Extract.csv", Encoding.UTF8);
      List<myClaim> source = new List<myClaim>();
      for (int index = 0; index < strArray1.Length; ++index)
      {
        if (strArray1[index] != "")
        {
          string str1 = strArray1[index].Replace("^", ";").Replace("\",", "^");
          string[] strArray2 = str1.Replace("\"", "").Split('^');
          //string str1 = strArray1[index].Replace("^", ";");
          //string[] strArray2 = str1.Split(';');
          if (strArray2.Length >= 25 && str1.IndexOf("Zestlife claim") <= -1)
          {
            string str2 = this.chechInput1(strArray2[0]);
           // Console.WriteLine(str2);
            string str3 = this.chechInput1(strArray2[19]);
            string s1 = this.chechAmount1(strArray2[14]);
            string s2 = this.chechAmount1(strArray2[15]);
            string s3 = this.chechAmount1(strArray2[16]);
            double num1 = double.Parse(s1);
            double num2 = double.Parse(s2);
            double num3 = double.Parse(s1);
            source.Add(new myClaim()
            {
              claim_number = str2,
              practice_number = str3,
              charged_amnt = num1,
              scheme = num2,
              gap = num3
            });
          }
        }
      }
      List<myClaim> list = source.GroupBy(l => new
      {
        claim_number = l.claim_number,
        practice_number = l.practice_number
      }).Select(cl => new myClaim()
      {
        claim_number = cl.First<myClaim>().claim_number,
        practice_number = cl.First<myClaim>().practice_number,
        charged_amnt = cl.First<myClaim>().charged_amnt,
        scheme = cl.First<myClaim>().scheme,
        gap = cl.First<myClaim>().gap
      }).ToList<myClaim>().GroupBy<myClaim, string>((Func<myClaim, string>) (l => l.claim_number)).Select<IGrouping<string, myClaim>, myClaim>((Func<IGrouping<string, myClaim>, myClaim>) (cl => new myClaim()
      {
        claim_number = cl.First<myClaim>().claim_number,
        charged_amnt = cl.Sum<myClaim>((Func<myClaim, double>) (c => c.charged_amnt)),
        scheme = cl.Sum<myClaim>((Func<myClaim, double>) (c => c.scheme)),
        gap = cl.Sum<myClaim>((Func<myClaim, double>) (c => c.gap))
      })).ToList<myClaim>();
      int count1 = list.Count;
      for (int index = 0; index < count1; ++index)
        Console.WriteLine("----------------->" + list[index].claim_number + "----" + count1.ToString());
      using (DBConnect dbConnect = new DBConnect())
      {
        int num1 = 0;
        int num2 = strArray1.Length - 1;
        int num3 = 0;
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
        string str1 = "";
        Console.WriteLine(num2.ToString());
        for (int index1 = 0; index1 < strArray1.Length; ++index1)
        {
          if (strArray1[index1] != "")
          {
            string str2 = strArray1[index1].Replace("^", ";").Replace("\",", "^");
            string[] strArray2 = str2.Replace("\"", "").Split('^');
              //string str2 = strArray1[index1].Replace("^", ";");
              //string[] strArray2 = str2.Split(';');
            if (strArray2.Length >= 25)
            {
              if (str2.IndexOf("Zestlife claim") <= -1)
              {
                try
                {
                  int client_id = 1;
                  string username = dbConnect.getDetails().Tables[0].Rows[0]["username"].ToString();
                  string s1 = dbConnect.getDetails().Tables[0].Rows[0]["id"].ToString();
                  string recepient = dbConnect.getDetails().Tables[0].Rows[0]["email"].ToString();
                  int id = int.Parse(s1);
                  string str3 = this.chechInput1(strArray2[1]);
                  string member_name = this.chechInput1(strArray2[7]);
                  string member_surname = this.chechInput1(strArray2[8]);
                  string cell = this.chechInput1(strArray2[9]);
                  string telephone = this.chechInput1(strArray2[10]);
                  string email = this.chechInput1(strArray2[11]);
                  string product_name = this.chechInput1(strArray2[2]);
                  string product_code = this.chechInput1(strArray2[3]);
                  string benefitiary_number = this.chechInput1(strArray2[6]);
                  string str4 = this.chechInput1(strArray2[12]);
                  string medical_aid = "Unknown";
                  if (str4 == "Discovery Health Medical Scheme")
                    medical_aid = "Discovery Health Medical Scheme";
                  else if (str4 == "Medshield Medical Scheme")
                    medical_aid = "Medshield Medical Scheme";
                  else if (str4 == "Gems")
                    medical_aid = "Government Employees Medical Scheme (GEMS)";
                  else if (str4 == "Fedhealth Medical Scheme")
                    medical_aid = "Fedhealth Medical Scheme";
                  string str5 = this.chechInput1(strArray2[0]);
                  string s2 = this.chechAmount1(strArray2[14]);
                  string s3 = this.chechAmount1(strArray2[15]);
                  string s4 = this.chechAmount1(strArray2[16]);
                  string icd10 = this.chechInput1(strArray2[17]);
                  string icd10_descr = this.chechInput1(strArray2[18]);
                  int count2 = dbConnect.getPMBstatus(icd10).Tables[0].Rows.Count;

                  //Console.WriteLine("Claim Number : " + str5 + " Name : " + member_name + " Surname : " + member_surname + " Email : " + email + " benefitiary_number : " + benefitiary_number + " medical_aid : " + medical_aid + " icd10 : " + icd10 + "");
                  int pmb = 0;
                  if (count2 > 0 && dbConnect.getPMBstatus(icd10).Tables[0].Rows[0]["pmb_code"].ToString().Trim().Length >= 1)
                    pmb = 1;
                  double charged = double.Parse(s2);
                  double scheme1 = double.Parse(s3);
                  double gap1 = double.Parse(s4);
                  double chargedAmnt = double.Parse(s2);
                  double scheme2 = double.Parse(s3);
                  double gap2 = double.Parse(s4);
                  for (int index2 = 0; index2 < count1; ++index2)
                  {
                    if (list[index2].claim_number == str5)
                    {
                      chargedAmnt = list[index2].charged_amnt;
                      scheme2 = list[index2].scheme;
                      gap2 = list[index2].gap;
                    }
                  }
                  string str6 = this.chechInput1(strArray2[4]);
                  string str7 = this.chechInput1(strArray2[5]);
                  string str8 = this.chechInput1(strArray2[19]).PadLeft(7, '0');
                  string str9 = this.chechInput1(strArray2[20]);
                  string treatmentcode = this.chechInput1(strArray2[21]);
                  string treatment_descr = this.chechInput1(strArray2[22]);
                  string str10 = this.chechInput1(strArray2[13]);
                  string serviceDate = this.chechInput1(strArray2[27]);
                  string s5 = this.chechAmount1(strArray2[23]);
                  string s6 = this.chechAmount1(strArray2[24]);
                  str1 = s5 + "---------" + s6 + str5;
                  double charged_amt = double.Parse(s5);
                  double scheme_paid = double.Parse(s6);
                  string treatmentdate = str10.Replace('/', '-');
                  string treatmentdateService = serviceDate.Replace('/', '-');
                  DateTime dateTime = Convert.ToDateTime(treatmentdate);
                  if (dbConnect.getMember1(str3,client_id).Tables[0].Rows.Count < 1)
                  {
                    dbConnect.InsertMember(client_id, str3, member_name, member_surname, telephone, email, medical_aid, cell, product_name, product_code, benefitiary_number);
                    Console.WriteLine("New Member loaded" + str3);
                    ++num3;
                  }
                  else
                  {
                    Console.WriteLine("Duplicate member" + str3);
                    ++num4;
                  }
                  Thread.Sleep(3);
                  dbConnect.member_id = int.Parse(dbConnect.getMember1(str3,client_id).Tables[0].Rows[0]["member_id"].ToString());
                  int count3 = dbConnect.getDuplicate1(str5, client_id).Tables[0].Rows.Count;
                  if (count3 < 1)
                  {
                    dbConnect.InsertClaim(dbConnect.member_id, str7, username, service_date: new DateTime?(dateTime), icd10: icd10, pmb: pmb, amount_charged: chargedAmnt, client_gap: gap2, icd10_descr: icd10_descr, scheme_paid: scheme2);
                    dbConnect.Update(id);
                    Console.WriteLine("New Claim Loaded " + str7);
                    //myStaticMethods.sendMail(recepient, str5, str3);
                  }
                  else
                  {
                    Console.WriteLine("Duplicate Claim " + str5);
                    ++num6;
                  }
                  Thread.Sleep(3);
                  dbConnect.myClaim_id = int.Parse(dbConnect.getDuplicate1(str5, client_id).Tables[0].Rows[0]["claim_id"].ToString());
                  string str11 = dbConnect.getDuplicate1(str5, client_id).Tables[0].Rows[0]["Open"].ToString();
                  if (count3 < 1)
                  {
                    dbConnect.InsertPatient(dbConnect.myClaim_id, str6 + " " + str7);
                    Console.WriteLine("New patient loaded " + str6);
                    ++num7;
                  }
                  else
                  {
                    Console.WriteLine("Duplicate Patient " + str6);
                    ++num8;
                  }
                  Thread.Sleep(3);
                  int count4 = dbConnect.check_localDoctor(dbConnect.myClaim_id, str8).Tables[0].Rows.Count;
                  Console.WriteLine("We are here" + count4.ToString());
                  if (count4 < 1)
                  {
                    int count5 = dbConnect.checkDoctor_Medp(str8).Tables[0].Rows.Count;
                    Console.WriteLine("We are here 11" + count5.ToString());
                    if (count5 < 1)
                      dbConnect.insertDoctorMedpages(str9, str8);
                    dbConnect.insertDoctorLocal(dbConnect.myClaim_id, str8, str9, charged, scheme1, gap1);
                    Console.WriteLine("New Doctor loaded " + str8);
                    ++num9;
                  }
                  else
                  {
                    Console.WriteLine("Duplicate Doctoer " + str8);
                    ++num10;
                  }
                  if (dbConnect.checkClaimline(dbConnect.myClaim_id, str8, treatmentdateService, treatmentcode).Tables[0].Rows.Count < 1)
                  {
                      dbConnect.insertClaimline(dbConnect.myClaim_id, str8, treatmentdateService, treatmentcode, treatment_descr, charged_amt, scheme_paid, icd10, icd10_descr,0,"");
                    Console.WriteLine("New Claim Line loaded ");
                    if (!bool.Parse(str11))
                    {
                        string reopen_date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        dbConnect.openClaim(dbConnect.myClaim_id, 1, reopen_date);
                      Console.WriteLine("The closed");
                    }
                    ++num11;
                  }
                  else
                  {
                    Console.WriteLine("Duplicate Claim Line " + treatmentcode);
                    ++num12;
                  }
                }
                catch (Exception ex)
                {
                  Console.WriteLine(str1 + "----There is an error " + ex.Message);
                  ++num13;
                }
                finally
                {
                  Console.WriteLine("================================================================Ends here vvvv");
                }
              }
            }
          }
        }
        Console.WriteLine("-------------------------------------------------Finish-------------------------------------------------------------------");
        Console.WriteLine("Total Lines : " + num2.ToString());
        Console.WriteLine("Total New Members : " + num3.ToString());
        Console.WriteLine("Total Duplicate Member : " + num4.ToString());
        Console.WriteLine("Total New Claims : " + num5.ToString());
        Console.WriteLine("Total Claim Duplicates : " + num6.ToString());
        Console.WriteLine("Total New Patients : " + num7.ToString());
        Console.WriteLine("Total Patient Duplicates : " + num8.ToString());
        Console.WriteLine("Total New Doctors : " + num9.ToString());
        Console.WriteLine("Total Doctor Duplicates : " + num10.ToString());
        Console.WriteLine("Total New Claim Lines : " + num11.ToString());
        Console.WriteLine("Total Claim Lines Duplicates : " + num12.ToString());
        Console.WriteLine("Total Errors : " + num13.ToString());
        myStaticMethods.simpleMailSender("tendai@medclaimassist.co.za", "Zeslife Claims", string.Format("Total Lines : {0} <br> Total New Members : {1} <br> Total Duplicate Member : {2}<br> Total New Claims : {3} <br> Total Claim Duplicates :{4} <br> Total New Patients : {5} <br> Total Patient Duplicates : {6} <br> Total New Doctors : {7} <br> Total Doctor Duplicates : {8} <br> Total New Claim Lines : {9} <br> Total Claim Lines Duplicates : {10} <br> Total Errors : {11}", (object) num2.ToString(), (object) num3.ToString(), (object) num4.ToString(), (object) num5.ToString(), (object) num6.ToString(), (object) num7.ToString(), (object) num8.ToString(), (object) num9.ToString(), (object) num10.ToString(), (object) num11.ToString(), (object) num12.ToString(), (object) num13.ToString()));
      }
    }
        public void feedbackZest()
        {
            Console.WriteLine("watanga fut dzivarg");
            var csv = new StringBuilder();
            // var filePath = @"C:\xc\Zestlife\MCAFeedback.csv";
            var filePath = @"C:\xc\Zestlife\MCAFeedback.csv";

            File.WriteAllText(filePath, String.Empty);
            using (DBConnect db = new DBConnect())
            {
                int mynum = db.zestFeedbak().Tables[0].Rows.Count;
                Console.WriteLine(mynum.ToString());
                string header = "Zestlife claim number|Policy Number|Product Name|Product code|Patient Name|Patient Surname|Beneficiary Number|Policy holder Name|Policy holder surname|	Policy holder cell phone number|Policy holder telephone number|Policy holder email address|Policy holder medical aid scheme|Treatment date|Claim charge amount|Claim scheme paid amount|Zestlife Gap amount|Icd10 code|Icd 10 description|Practice number|Practice name|Treatment code|Treatment code description|Claimline charge amount|Claimline scheme paid amount|Date sent to MCA|Date Enter by MCA|Intervention Description|Open|Scheme Savings|Discount Saving|Updated";
                csv.AppendLine(header);

                foreach (DataTable table in db.zestFeedbak().Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {

                        try
                        {
                            string claim_id = dr["mca_claim_id"].ToString();
                            string practice_number = dr["practice_number"].ToString();
                            string tariff_code = dr["tariff_code"].ToString();
                            string msg_dscr = dr["mca_claim_id"].ToString();
                            string primaryICDCode = dr["msg_dscr"].ToString();
                            string primaryICDDescr = dr["primaryICDDescr"].ToString();
                            string treatmentDate = dr["treatmentDate"].ToString();
                            string clmnline_charged_amnt = dr["clmnline_charged_amnt"].ToString();
                            string clmline_scheme_paid_amnt = dr["clmline_scheme_paid_amnt"].ToString();
                            string date_entered = dr["date_entered"].ToString();
                            string claim_number = dr["claim_number"].ToString();
                            string policy_number = dr["policy_number"].ToString();
                            string first_name = dr["first_name"].ToString();
                            string surname = dr["surname"].ToString();
                            string cell = dr["cell"].ToString();
                            string telephone = dr["telephone"].ToString();
                            string email = dr["email"].ToString();
                            string medical_scheme = dr["medical_scheme"].ToString();
                            string charged_amnt = dr["charged_amnt"].ToString();
                            string scheme_paid = dr["scheme_paid"].ToString();
                            string gap = dr["gap"].ToString();
                            string icd10 = dr["icd10"].ToString();
                            string icd10_desc = dr["icd10_desc"].ToString();
                            string productName = dr["productName"].ToString();
                            string claimline_id = dr["id"].ToString();
                            string open = dr["Open"].ToString();
                            string intervention = "";
                            string doc_name = "";
                            string product_code = dr["product_code"].ToString();
                            string beficiary_number = dr["beneficiary_number"].ToString();
                            string today = DateTime.Now.ToString("yyyy-MM-dd");
                            string scheme_savings = dr["savings_scheme"].ToString();
                            string discount_savings = dr["savings_discount"].ToString();
                            if (db.checkD(practice_number, int.Parse(claim_id)).Tables[0].Rows.Count > 0)
                            {
                                doc_name = db.checkD(practice_number, int.Parse(claim_id)).Tables[0].Rows[0]["doc_name"].ToString();
                                scheme_savings = db.checkD(practice_number, int.Parse(claim_id)).Tables[0].Rows[0]["savings_scheme"].ToString();
                                discount_savings = db.checkD(practice_number, int.Parse(claim_id)).Tables[0].Rows[0]["savings_discount"].ToString();
                                charged_amnt = db.checkD(practice_number, int.Parse(claim_id)).Tables[0].Rows[0]["doc_charged_amount"].ToString();
                                scheme_paid = db.checkD(practice_number, int.Parse(claim_id)).Tables[0].Rows[0]["doc_scheme_amount"].ToString();
                                gap = db.checkD(practice_number, int.Parse(claim_id)).Tables[0].Rows[0]["doc_gap"].ToString();

                            }
                            if (db.doctoNote(practice_number, int.Parse(claim_id)).Tables[0].Rows.Count > 0)
                            {
                                intervention = db.doctoNote(practice_number, int.Parse(claim_id)).Tables[0].Rows[0]["intervention_desc"].ToString();
                            }

                            char updated = 'Y';
                            string xxx = "Open";
                            if (!bool.Parse(open))
                            {
                                xxx = "Closed";
                                db.updateZestlife(1, int.Parse(claimline_id));

                            }

                            int isnot = db.selectZestlifenotes(int.Parse(claimline_id), intervention).Tables[0].Rows.Count;
                            if (isnot > 0)
                            {
                                updated = 'N';
                            }
                            intervention = intervention.Replace("|", ";");
                            intervention = intervention.Replace("\n", ".");
                            intervention = intervention.Replace("\r", ".");
                            intervention = intervention.Replace("\t", ".");
                            //intervention = Regex.Replace(intervention, @"\t\n\r", ".");

                            db.insertZestlifenotes(int.Parse(claimline_id), intervention);
                            string content = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}|{15}|{16}|{17}|{18}|{19}|{20}|{21}|{22}|{23}|{24}|{25}|{26}|{27}|{28}|{29}|{30}|{31}", claim_number, policy_number, productName, product_code, first_name, surname, beficiary_number, first_name, surname, cell, telephone, email, medical_scheme, treatmentDate, charged_amnt, scheme_paid, gap, icd10, icd10_desc, practice_number, doc_name, tariff_code, msg_dscr, clmnline_charged_amnt, clmline_scheme_paid_amnt, date_entered, date_entered, intervention, open, scheme_savings, discount_savings, updated);

                            // Console.WriteLine(policy_number + "---" + practice_number + "=====" + doc_name + "---" + icd10 + "---" + primaryICDCode + "---" + primaryICDDescr + "---" + treatmentDate + "---" + clmnline_charged_amnt + "---" + clmline_scheme_paid_amnt);
                            csv.AppendLine(content);

                            Console.WriteLine(claim_id + "---" + claimline_id + "====" + xxx + "----" + open + "----" + intervention);

                        }
                        catch (Exception ex)
                        {

                            Console.WriteLine("There is a srs error " + ex.Message);

                        }
                        finally
                        {
                            File.WriteAllText(filePath, csv.ToString());
                            //Console.WriteLine("======================================================Done++++++++++++++++++++++++++++++++++++");
                        }
                    }
                }
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


    }

    class myClaim
    {
        public string claim_number { get; set; }
        public string practice_number { get; set; }
        public double charged_amnt { get; set; }
        public double scheme { get; set; }
        public double gap { get; set; }
    }
}
