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
    public class trade_bridge : amountsGrouper
    {
        public void fetchDetails(string path)
        {
            //CHF_PolicyProductName|CHF_PolicyNumber|CHF_PolicyStartDate|CHF_Threshold|Claim_MemName|Claim_MemSurname|Claim_MemNum|Claim_MemMAPlan|Claim_PatName|Claim_PatSurname|Claim_PatGender|Claim_PatDOB|Claim_PatID|Claim_PatDependantCode|Claim_PraNum|Claim_PraName|Claim_TrtDrPraNum|Claim_TrtDrName|Claim_TranNum|Claim_AuthNum|Claim_Discount|Claim_TotTarAmount|Claim_TotModAmount|ClaimLine_LineNum|ClaimLine_DOS|ClaimLine_LineType|ClaimLine_AuthNum|ClaimLine_NappiCode|ClaimLine_ChargeCode|ClaimLine_ChargeDesc|ClaimLine_ChargeStart|ClaimLine_ChargeEnd|ClaimLine_Amount|ClaimLine_Discount|ClaimLine_HospInd|ClaimLine_Diagnoses|RA_RANum|RA_EFTNum|RA_RADate|RA_Funder|RAClaim_TotClaimAmount|RAClaim_TotPaymentAmount|RAClaim_CalcPatLiableAmount|RAClaimLine_ChargeCode|RAClaimLine_NappiCode|RAClaimLine_Amount|RAClaimLine_TariffAmount|RAClaimLine_PaymentAmount|RAClaimLine_PatPaymentAmount|RAClaimLine_PatOwedAmount|RAClaimLine_Reasons

            Console.WriteLine("Starting...");
            

            if (File.Exists(path))
            {
              
                string[] strArray = File.ReadAllLines(path, Encoding.UTF8);
                //Console.WriteLine("testing ...");
                //Console.ReadLine();
                List<myGroup> source = new List<myGroup>();
                for (int index = 0; index < strArray.Length; ++index)
                {
                    
                    if (strArray[index] != "")
                    {
                        string str = strArray[index];
                        string[] strArray2 = str.Split('|');
                        if (strArray2.Length >= 25 && str.IndexOf("CHF_PolicyProductName") <= -1)
                        {
                            string claim_number_1 = this.chechInput1(strArray2[17]);
                            // Console.WriteLine(str2);
                            string practice_number_1 = this.chechInput1(strArray2[15]);
                            double charged_amnt_1 = double.Parse(this.chechAmount1(strArray2[20]).ToString());
                            double scheme_1 = double.Parse(this.chechAmount1(strArray2[21]).ToString());
                            double gap_1 = charged_amnt_1 - scheme_1;
                            Console.WriteLine("=="+charged_amnt_1.ToString());
                            source.Add(new myGroup()
                            {
                                claim_number = claim_number_1,
                                practice_number = practice_number_1,
                                charged_amnt = charged_amnt_1,
                                scheme = scheme_1,
                                gap = gap_1
                            });
                        }
                    }
                }
              
                List<myGroup> result = this.processGroup(source);
                int count1 = result.Count;
                for (int index = 0; index < count1; ++index)
                {
                    Console.WriteLine("----------------->" + result[index].claim_number + "----" + count1.ToString());
                }
                using (DBConnect dbConnect = new DBConnect("seamless"))
                {
                   
                    int num2 = strArray.Length - 1;
                    int num3 = 0;
                    int num4 = 0;                   
                    int num6 = 0;
                    int num7 = 0;
                    int num8 = 0;
                    int num9 = 0;
                    int num10 = 0;
                    int num11 = 0;
                    int num12 = 0;
                    int num13 = 0;
                    string str1 = "";
                    Console.WriteLine("Test 3");
                    for (int index1 = 0; index1 < strArray.Length; ++index1)
                    {
                        if (strArray[index1] != "")
                        {
                            string str2 = strArray[index1];
                            string[] strArray2 = str2.Split('|');
                            if (strArray2.Length >= 25)
                            {
                                if (str2.IndexOf("CHF_PolicyProductName") <= -1)
                                {
                                    try
                                    {
                                        int client_id = 3;                                        
                                        string username = "Faghry";
                                        string owner_id = dbConnect.getDetails().Tables[0].Rows[0]["id"].ToString();
                                        string recepient = dbConnect.getDetails().Tables[0].Rows[0]["email"].ToString();
                                        int id = int.Parse(owner_id);
                                        string claim_number_2 = this.chechInput1(strArray2[17]);
                                        string member_name = this.chechInput1(strArray2[4]);
                                        string member_surname = this.chechInput1(strArray2[5]);
                                        string member_id = this.chechInput1(strArray2[6]);
                                        string patient_name = this.chechInput1(strArray2[7]);
                                        string patient_surname = this.chechInput1(strArray2[8]);
                                        string patient_id = this.chechInput1(strArray2[11]);
                                        string patient_gender = this.chechInput1(strArray2[9]);
                                        string patient_dob = this.chechInput1(strArray2[10]);
                                        string cell = "";
                                        string telephone = "";
                                        string email = "";
                                        string product_name = this.chechInput1(strArray2[0]);
                                        string product_code = "";
                                        string benefitiary_number = "";
                                        string policy_number = this.chechInput1(strArray2[1]);
                                        string medical_scheme = "Unknown";
                                        if(product_name.IndexOf("Sanlam")>-1)
                                        {
                                            client_id = 15;
                                        }
                                        else if (product_name.IndexOf("Western") > -1)
                                        {
                                            client_id = 16;
                                        }
                                            string[] icd10_arr = this.chechInput1(strArray2[34]).Split(',');
                                        string icd10 = icd10_arr[0];
                                        string icd10_descr = "";
                                        string sec_icd10 = icd10_arr.Length > 1 ? this.chechInput1(strArray2[34]) : "";
                                        string medical_aid = "Unknown";
                                        if (medical_scheme == "Discovery Health Medical Scheme")
                                            medical_aid = "Discovery Health Medical Scheme";
                                        else if (medical_scheme == "Medshield Medical Scheme")
                                            medical_aid = "Medshield Medical Scheme";
                                        else if (medical_scheme == "Gems")
                                            medical_aid = "Government Employees Medical Scheme (GEMS)";
                                        else if (medical_scheme == "Fedhealth Medical Scheme")
                                            medical_aid = "Fedhealth Medical Scheme";
                                        double charged_amnt_1 = double.Parse(this.chechAmount1(strArray2[20]).ToString());
                                        double scheme_1 = double.Parse(this.chechAmount1(strArray2[21]).ToString());
                                        double scheme_rate = double.Parse(this.chechAmount1(strArray2[20]).ToString());
                                        double gap_1 = charged_amnt_1 - scheme_1;
                                        Console.WriteLine(claim_number_2+"--->"+charged_amnt_1.ToString());
                                        //double m = 9.0;
                                       
                                        int count2 = dbConnect.getPMBstatus(icd10).Tables[0].Rows.Count;
                                        string claim_line_start = this.chechInput1(strArray2[29]);
                                        string claim_line_end = this.chechInput1(strArray2[30]);
                                        string claim_authnum = this.chechInput1(strArray2[25]);
                                        string inhospital = this.chechInput1(strArray2[33]);

                                        int pmb = 0;
                                        if (count2 > 0 && dbConnect.getPMBstatus(icd10).Tables[0].Rows[0]["pmb_code"].ToString().Trim().Length >= 1)
                                            pmb = 1;

                                        double chargedAmnt_line = double.Parse(this.chechAmount1(strArray2[44]).ToString());
                                        double scheme_line = double.Parse(this.chechAmount1(strArray2[45]).ToString());
                                        double gap_line = chargedAmnt_line - scheme_line;
                                        

                                        string pract_name = this.chechInput1(strArray2[14]);
                                        string treating_pract_name = this.chechInput1(strArray2[16]);
                                        string practice_number_2 = this.chechInput1(strArray2[13]).PadLeft(7, '0');
                                        string treating_practice_number = this.chechInput1(strArray2[15]).PadLeft(7, '0');

                                        string treatmentcode = this.chechInput1(strArray2[27]);
                                        string treatment_descr = this.chechInput1(strArray2[28]);
                                        string str10 = this.chechInput1(strArray2[23]);
                                        string serviceDate = this.chechInput1(strArray2[37]);
                                        string nappi_code = this.chechInput1(strArray2[26]);
                                        string claim_line_number = this.chechInput1(strArray2[22]);
                                        string treatment_type = this.chechInput1(strArray2[24]);
                                        string reason = this.chechInput1(strArray2[49]);
                                        string[] reason_arr=reason.Split(':');
                                        string reason_code=reason_arr[0];
                                        double xchargedAmnt = 0;
                                        double xscheme = 0;
                                        double xgap1 = 0;
                                        for (int index = 0; index < count1; ++index)
                                        {
                                            if (result[index].claim_number == claim_number_2 && result[index].practice_number==practice_number_2)
                                            {
                                                xchargedAmnt += chargedAmnt_line;
                                                xscheme += scheme_line;
                                                xgap1 += xchargedAmnt- xscheme;
                                            }
                                        }
                                        //xchargedAmnt = (double)(xchargedAmnt/100);
                                        //xscheme = (double)(xscheme / 100);
                                        //xgap1 = (double)(xgap1 / 100);


                                        string treatmentdate = str10.Replace('/', '-');
                                        string treatmentdateService = serviceDate.Replace('/', '-');
                                        DateTime dateTime = Convert.ToDateTime(treatmentdate);
                                        if (dbConnect.getMember1(policy_number, client_id).Tables[0].Rows.Count < 1)
                                        {
                                            dbConnect.InsertMember(client_id, policy_number, member_name, member_surname, telephone, email, medical_aid, cell, product_name, product_code, benefitiary_number);
                                            Console.WriteLine("New Member loaded" + policy_number);
                                            ++num3;
                                        }
                                        else
                                        {
                                            Console.WriteLine("Duplicate member" + policy_number);
                                            ++num4;
                                        }
                                        Thread.Sleep(3);
                                        Console.WriteLine("Policy Number : " + policy_number +" --- Client id : "+ client_id.ToString());
                                        dbConnect.member_id = int.Parse(dbConnect.getMember1(policy_number, client_id).Tables[0].Rows[0]["member_id"].ToString());
                                        int count3 = dbConnect.getDuplicate1(claim_number_2, client_id).Tables[0].Rows.Count;
                                        if (count3 < 1)
                                        {
                                            dbConnect.InsertClaim(dbConnect.member_id, claim_number_2, username, service_date: new DateTime?(dateTime), icd10: icd10, pmb: pmb, amount_charged: charged_amnt_1, client_gap: gap_1, icd10_descr: icd10_descr, scheme_paid: scheme_1,open:2,patient_number:patient_id,patient_dob:patient_dob,patient_gender:patient_gender, claim_authnum:claim_authnum, tariff_amnt:scheme_rate, senderId:"11");
                                            
                                            Console.WriteLine("New Claim Loaded " + claim_number_2);
                                            //myStaticMethods.sendMail(recepient, str5, str3);
                                        }
                                        else
                                        {
                                            Console.WriteLine("Duplicate Claim " + claim_number_2);
                                            ++num6;
                                        }
                                        //break;
                                        Thread.Sleep(3);
                                        //Console.WriteLine("Zimbabwe XXXX" + claim_number_2+"---"+ client_id.ToString());
                                        dbConnect.myClaim_id = int.Parse(dbConnect.getDuplicate1(claim_number_2, client_id).Tables[0].Rows[0]["claim_id"].ToString());
                                       
                                        Console.WriteLine("Try here Again");
                                        if (count3 < 1)
                                        {
                                            dbConnect.InsertPatient(dbConnect.myClaim_id, patient_name + " " + patient_surname);
                                            Console.WriteLine("New patient loaded " + patient_name);
                                            ++num7;
                                        }
                                        else
                                        {
                                            Console.WriteLine("Duplicate Patient " + patient_name);
                                            ++num8;
                                        }
                                        Thread.Sleep(3);
                                        int count4 = dbConnect.check_localDoctor(dbConnect.myClaim_id, practice_number_2).Tables[0].Rows.Count;
                                        Console.WriteLine("We are here" + count4.ToString());
                                        if (count4 < 1)
                                        {
                                            int count5 = dbConnect.checkDoctor_Medp(practice_number_2).Tables[0].Rows.Count;
                                            Console.WriteLine("We are here 11" + count5.ToString());
                                            if (count5 < 1)
                                                dbConnect.insertDoctorMedpages(pract_name, practice_number_2);
                                            dbConnect.insertDoctorLocal(dbConnect.myClaim_id, practice_number_2, pract_name, xchargedAmnt, xscheme, xgap1, treating_practice_number, treating_pract_name,claim_number_2);
                                            Console.WriteLine("New Doctor loaded " + pract_name);
                                            ++num9;
                                        }
                                        else
                                        {
                                            Console.WriteLine("Duplicate Doctoer " + pract_name);
                                            ++num10;
                                        }
                                        if (dbConnect.checkClaimline(dbConnect.myClaim_id, practice_number_2, treatmentdateService, treatmentcode).Tables[0].Rows.Count < 1)
                                        {
                                            icd10_descr = dbConnect.getPMBstatus(icd10).Tables[0].Rows[0]["shortdesc"].ToString();
                                            Console.WriteLine("Claim Id "+ dbConnect.myClaim_id.ToString());
                                            Console.WriteLine("practice_number_2 " + practice_number_2);
                                            Console.WriteLine("treatmentdateService " + treatmentdateService);
                                            Console.WriteLine("treatmentcode " + treatmentcode);
                                            Console.WriteLine("treatment_descr " + treatment_descr);
                                            Console.WriteLine("chargedAmnt_line " + chargedAmnt_line);
                                            Console.WriteLine("scheme_line " + scheme_line);
                                            dbConnect.insertClaimline(dbConnect.myClaim_id, practice_number_2, treatmentdateService, treatmentcode, treatment_descr, chargedAmnt_line, scheme_line, icd10, icd10_descr, 0, "","","","",reason_code,nappi_code,treatment_type,sec_icd10, reason_code,reason, claim_line_start, claim_line_end, inhospital, claim_line_number);
                                            Console.WriteLine("New Claim Line loaded ");                                            
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
                }

            }

        }
         
            }
        }
