using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Data;

namespace aaa_library
{
    public class CHFClass : amountsGrouper
    {
        public void subscribers(string providername)
        {
            string basehome = Environment.GetEnvironmentVariable("HOME");
            string fileName = basehome+@"/files/" + providername + @".txt";

            if (!File.Exists(fileName))
            {
                var filecreated = File.Create(fileName);
                filecreated.Close();
            }


            Thread.Sleep(100);

            File.WriteAllText(fileName, String.Empty);
            using (System.IO.StreamWriter file = new StreamWriter(fileName, true))
            {
                try
                {
                    using (DBConnect db = new DBConnect())
                    {
                        DataSet ds = db.getSubscribers();
                        int tot = ds.Tables[0].Rows.Count;
                        Console.WriteLine(tot.ToString());
                        for (int x = 0; x < tot; x++)
                        {
                            string name, surname, id_number, dob, email, medical_scheme, scheme_option, medical_aid_number, role, initials;
                            name = ds.Tables[0].Rows[x]["name"].ToString();
                            surname = ds.Tables[0].Rows[x]["surname"].ToString();
                            id_number = ds.Tables[0].Rows[x]["id_number"].ToString();
                            dob = ds.Tables[0].Rows[x]["dob"].ToString();
                            email = ds.Tables[0].Rows[x]["email"].ToString();
                            medical_scheme = ds.Tables[0].Rows[x]["medical_scheme"].ToString();
                            scheme_option = ds.Tables[0].Rows[x]["scheme_option"].ToString();
                            medical_aid_number = ds.Tables[0].Rows[x]["medical_aid_number"].ToString();
                            medical_aid_number = medical_aid_number == "Zestlife" ? "" : medical_aid_number;
                            role = ds.Tables[0].Rows[x]["role"].ToString();
                            initials = name.Substring(0, 1);
                            dob = dob.Replace("-", "/");
                            string str = "";
                            if (providername == "Healthbridge")
                            {
                                str = String.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}|{15}", "", name, initials, surname, dob, "", id_number, "", "", medical_scheme, scheme_option, medical_aid_number, "", "", role, "2500");

                            }
                            else if (providername == "cgm")
                            {
                                str = String.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\",\"{9}\",\"{10}\",\"{11}\",\"{12}\",\"{13}\",\"{14}\",\"{15}\"", "", "client", name, surname, initials, id_number, dob, "", "", "", medical_scheme, scheme_option, medical_aid_number, "", email, "");


                            }
                            else
                            {
                                str = String.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}", "", "", name, initials, surname, dob, "", id_number, "", "", medical_scheme, scheme_option, medical_aid_number, "", "client");
                            }
                            file.WriteLine(str);
                            //Console.WriteLine(str);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("there is an error " + ex.Message);
                }
            }
        }

        public void mergeFiles(string path1, string path2)
        {
            string newFilePath = path1;
            string allText = System.IO.File.ReadAllText(path1);
            //allText += "\r\n";
            allText += System.IO.File.ReadAllText(path2);
            //Console.WriteLine(allText);
            File.WriteAllText(path1, String.Empty);
            System.IO.File.WriteAllText(newFilePath, allText);
        }


        public void KS(string filepath, string filename, string cv)
        {
            int totallines = 0;
            int total_loaded = 0;
            int total_actclaims = 0;

            string entered_by = "System";
            List<string> skippedclaims = new List<string>();
            DBConnect db = new DBConnect();


            if (File.Exists(filepath))
            {
                string[] lines = File.ReadAllLines(filepath, Encoding.UTF8);
                using (System.IO.StreamWriter file = new StreamWriter(filepath, true))
                {

                    List<splitGroup> source = new List<splitGroup>();

                    for (int i = 0; i < lines.Length; i++)
                    {
                        string line = lines[i];

                        if (line != "")
                        {


                            if (line.IndexOf("claims records extracted") > -1)
                            {

                                continue;
                            }
                            if (line.IndexOf("No gap claims records to extract") > -1)
                            {

                                continue;
                            }
                            else if (line.IndexOf("Loyalty_number") > -1)
                            {
                                continue;
                            }
                           
                            string[] content = line.Split('|');
                            string loyalty_number = this.chechInput1(content[0]);
                              double charged_amount = double.Parse(this.chechAmount1(content[30]));
                           
                           double scheme_tariff_amount = double.Parse(this.chechAmount1(content[31]));
                           double scheme_paid_amount = double.Parse(this.chechAmount1(content[32]));

                           int copay_amount;
                           string numberStrcopay = this.chechInput1(content[44]);
                           bool isParsable = Int32.TryParse(numberStrcopay, out copay_amount);
                           if (!isParsable)
                           {
                               copay_amount = 0;
                           }

                          source.Add(new splitGroup()
                          {
                              loyalty_number = loyalty_number,
                              charged_amount = charged_amount,
                              scheme_amount = scheme_paid_amount,
                              copayment = copay_amount
                          });

                        }
                    }

                    List<splitGroup> result = this.processGroupSplit(source);
                    int count1 = result.Count;
                    for (int index = 0; index < count1; ++index)
                    {
                        string ca = result[index].charged_amount.ToString();
                        string sa = result[index].scheme_amount.ToString();
                        int cp = result[index].copayment;
                        if (ca == sa && cp < 1)
                        {
                            skippedclaims.Add(result[index].loyalty_number);
                        }
                        Console.WriteLine("----------------->" + result[index].loyalty_number + "==Charged : " + result[index].charged_amount.ToString() + "== Scheme : " + result[index].scheme_amount.ToString());
                    }
                    //End of first line

                    string[] arrskip = skippedclaims.ToArray();

                    for (int i = 0; i < lines.Length; i++)
                    {
                        string line = lines[i];
                        try
                        {


                            if (line != "")
                            {
                                if (line.IndexOf("claims records extracted") > -1)
                                {
                                    totallines = int.Parse(line.Split(' ')[0]);
                                    continue;
                                }
                                if (line.IndexOf("No gap claims records to extract") > -1)
                                {
                                    totallines = 0;
                                    continue;
                                }
                                else if (line.IndexOf("Loyalty_number") > -1)
                                {
                                    string[] isQuantity = line.Split('|');
                                    if (Array.IndexOf(isQuantity, "Quantity") < 0)
                                    {
                                        myStaticMethods.generalMailSender("tendai@medclaimassist.co.za", "SFTP Quantity error", "There is an error on Quantity");
                                        break;
                                    }
                                    else
                                    {
                                        continue;
                                    }

                                }

                                string[] content = line.Split('|');

                                string loyalty_number = this.chechInput1(content[0]);
                                double charged_amount = double.Parse(this.chechAmount1(content[30]));
                                double scheme_tariff_amount = double.Parse(this.chechAmount1(content[31]));
                                double scheme_paid_amount = double.Parse(this.chechAmount1(content[32]));


                                int copay_amount;
                                string numberStrcopay = this.chechInput1(content[44]);

                                bool isParsable = Int32.TryParse(numberStrcopay, out copay_amount);
                                if (!isParsable)
                                {
                                    copay_amount = 0;
                                }

                                if (arrskip.Contains(loyalty_number))
                                {
                                    Console.WriteLine("Skipped claim");
                                    continue;
                                }



                                string membership_number = this.chechInput1(content[1]);
                                string per_internal_number = this.chechInput1(content[2]);
                                string benefit_option = this.chechInput1(content[3]);
                                string benefit_start_date = this.chechInput1(content[4]);
                                string relationship_type = this.chechInput1(content[5]);
                                string beneficiary_name = this.chechInput1(content[6]);
                                string beneficiary_id_number = this.chechInput1(content[7]);
                                string beneficiary_date_of_birth = this.chechInput1(content[8]);
                                string beneficiary_scheme_join_date = this.chechInput1(content[9]);
                                string scheme_id = this.chechInput1(content[10]);
                                string dependant_code = this.chechInput1(content[11]);
                                string hospitalised_y_n = this.chechInput1(content[12]);
                                string procedure_date = this.chechInput1(content[13]);
                                string admission_date = this.chechInput1(content[14]);
                                string discharge_date = this.chechInput1(content[15]);
                                string no_of_days = this.chechInput1(content[16]);
                                string hospital_name = this.chechInput1(content[17]);
                                string pmb_y_n = this.chechInput1(content[18]);
                                string icd10_code = this.chechInput1(content[19]);
                                string tariff_code = this.chechInput1(content[20]);
                                string tariff_description = this.chechInput1(content[21]);
                                string bra_code = this.chechInput1(content[22]);
                                string original_claim_number = this.chechInput1(content[23]);
                                string reprocessed_claim = this.chechInput1(content[24]);
                                string claim_line_internal_number = this.chechInput1(content[25]);
                                string spe_speciality_code = this.chechInput1(content[26]);
                                string hcp = this.chechInput1(content[27]);
                                hcp = hcp.PadLeft(7, '0');
                                string hcp_name = this.chechInput1(content[28]);
                                string paid_amount_vendor = this.chechInput1(content[33]);
                                string paid_amount_member = this.chechInput1(content[34]);
                                string header_status = this.chechInput1(content[35]);
                                string line_status = this.chechInput1(content[36]);
                                string final_status_date = this.chechInput1(content[37]);
                                string date_sent_to_scheme_finance = this.chechInput1(content[38]);
                                string plan_benefit_group = this.chechInput1(content[39]);
                                string plan_benefit = this.chechInput1(content[40]);
                                string limit_codes = this.chechInput1(content[41]);
                                string limit_amount = this.chechInput1(content[42]);
                                string savings_amount = this.chechInput1(content[43]);
                                string new_borns_date_of_birth = this.chechInput1(content[45]);
                                string biological_drugs = this.chechInput1(content[46]);
                                string modifier = this.chechInput1(content[47]);
                                string header_error = this.chechInput1(content[48]);
                                string header_errors = this.chechInput1(content[49]);
                                string line_error = this.chechInput1(content[50]);
                                string line_errors = this.chechInput1(content[51]);
                                Console.WriteLine(membership_number + "- Tatanga 4 - " + beneficiary_id_number);
                                if (db.getSplitMembers(membership_number, beneficiary_id_number, filename).Tables[0].Rows.Count < 1)
                                {
                                    Console.WriteLine("New member loaded");
                                    db.insertSplitMember(membership_number, beneficiary_name, beneficiary_scheme_join_date, beneficiary_id_number, beneficiary_date_of_birth, entered_by, filename);

                                }
                                else
                                {
                                    Console.WriteLine("Duplicate Member");
                                }
                                Console.WriteLine(membership_number.ToString() + "---" + beneficiary_id_number + "--" + filename);
                                int split_member_id = int.Parse(db.getSplitMembers(membership_number, beneficiary_id_number, filename).Tables[0].Rows[0]["id"].ToString());
                                Console.WriteLine("Member ID : " + split_member_id.ToString());
                                if (db.getSplitClaim(split_member_id, loyalty_number).Tables[0].Rows.Count < 1)
                                {
                                    Console.WriteLine("New claim loaded");
                                    db.insertSplitClaim(split_member_id, loyalty_number, procedure_date, admission_date, discharge_date, copay_amount.ToString(), entered_by, filename);
                                    total_actclaims++;
                                }
                                else
                                {
                                    Console.WriteLine("Duplicate Claim");
                                }
                                int split_claim_id = int.Parse(db.getSplitClaim(split_member_id, loyalty_number).Tables[0].Rows[0]["id"].ToString());
                                Console.WriteLine("Claim ID : " + split_claim_id.ToString());

                                if (db.getSplitDoctor(split_claim_id, hospital_name).Tables[0].Rows.Count < 1)
                                {
                                    Console.WriteLine("Doctor Inserted");
                                    db.insertSplitDoctor(split_claim_id, hcp, hcp_name, hospital_name, entered_by, filename);
                                }
                                //if (db.getSplitClaimLine(split_claim_id,hospital_name, procedure_date,icd10_code, tariff_code, charged_amount,scheme_paid_amount).Tables[0].Rows.Count < 1)
                                //{
                                db.insertSplitClaimLine(split_claim_id, hcp, "", procedure_date, icd10_code, "", tariff_code, tariff_description, "", "", "", "", "", "", charged_amount, scheme_tariff_amount, scheme_paid_amount, line_errors, "", "", "", entered_by, filename, hospital_name, copay_amount);
                                Console.WriteLine("Claim line Inserted");
                                total_loaded++;
                                //}
                                db.updateTempSplit(split_claim_id, hospital_name, procedure_date, icd10_code, tariff_code, charged_amount, scheme_paid_amount, copay_amount);
                                Console.WriteLine("===========================================================");

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("This line number : " + i++.ToString());
                            //Console.WriteLine(line);
                            Console.WriteLine("There is an error : " + ex.Message);
                            //Console.ReadLine();
                            continue;

                        }
                    }
                  
                    file.Close();
                    Thread.Sleep(10);
                    Console.WriteLine("Finish 1");

                    db.insertSplitFiles(filename, totallines, total_loaded, entered_by, "loaded", total_actclaims);
                    var copies = db.getConfigs().Tables[0].Rows[0]["kaeloemails"].ToString();
                    myStaticMethods.copyMultiple(copies, filename + " loaded", string.Format("Good Day, <br><br>File Name :<b> {0} </b><br>Total Lines :<b> {1} </b><br> Total Lines Loaded : <b>{2} </b><br> Total Claims Loaded : <b> {3} </b><br><br>Regards,<br>MCA Team", filename, totallines, total_loaded, total_actclaims));


                    Console.WriteLine("Total Lines ===" + totallines.ToString());
                }
            }
            else
            {
                Console.WriteLine("The file does not exist");
            }

        }
        public void runCinagi(string sourcepath, string filename, string providername)
        {
            string[] lines = File.ReadAllLines(sourcepath, Encoding.UTF8);

            if (!File.Exists(filename))
            {
                File.WriteAllText(filename, String.Empty);
                using (System.IO.StreamWriter file = new StreamWriter(filename, true))
                {
                    //string strx = @"Policy_number|First name|Initials|Last name|DOB|Beneficiary number|ID number|Policy start date|Policy end date|Medical aid name|Medical aid option name|Medical aid number|Date Joined Medical Aid|Policy/product name|Role|Threshold";
                    string strx = "";
                    if (providername == "Medswitch")
                    {
                        strx = @"Product Id|Policy number|First name|Initials|Last name|DOB|Beneficiary number|ID number|Policy start date|Policy end date|Medical aid name|Medical aid option name|Medical aid number|Date Joined Medical Aid|Policy/product name";
                    }
                    else if (providername == "cgm")
                    {
                        //strx = String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15}", "\"Policy_number\"", "\"Policy / product name\"", "\"First name\"", "\"Last name\"", "\"Initials\"", "\"ID number\"", "\"DOB\"", "\"Beneficiary number\"", "\"Policy start date\"", "\"Policy end date\"", "\"Medical aid name\"", "\"Medical aid option name\"", "\"Medical aid number\"", "\"Cell phone number\"", "\"Email address\"", "\"Landline number\"");

                    }
                    if (strx.Length > 2)
                    {
                        file.WriteLine(strx);
                    }
                    Console.WriteLine(strx);

                    for (int i = 0; i < lines.Length; i++)
                    {
                        try
                        {
                            if (lines[i] != "")
                            {
                                string contentx = lines[i];
                                // Policy Number,Product Name,FirstName,Surname,ID Number,DOB,Policy Start Date,Policy End Date,Medcial Aid Name,Medcial Aid Option Name,Medcial Aid Number
                                string[] content = contentx.Split(',');
                                if (content.Length < 6 || contentx.IndexOf("Policy") > -1)
                                {
                                    continue;
                                }
                                string product_id = "";
                                string date_joined = "";
                                string role = "member";
                                string threshold = "2500";
                                string dob = content[5].Replace("-", "/");
                                string benefiary_number = "";
                                string start_date = content[6].Replace("-", "/");
                                string end_date = content[7].Replace("-", "/");
                                string id_number = content[4];
                                if (contentx.Contains("Policy"))
                                {
                                    product_id = "Product Id";
                                    date_joined = "Date Joined Medical Aid";
                                    role = "Role";
                                    threshold = "Threshold";
                                }
                                else
                                {

                                    dob = dob.Replace(" ", "");
                                    start_date = start_date.Replace(" ", "");
                                    end_date = end_date.Replace(" ", "");
                                    dob = dob.Length == 10 ? dob : "";
                                    start_date = start_date.Length == 10 ? start_date : "";
                                    if (end_date.Length == 10)
                                    {
                                        DateTime dateTime = DateTime.Now;
                                        DateTime otherDateTime = dateTime.AddDays(-90);
                                        string iDate = end_date.Replace("-", "/") + " 00:00:00";
                                        DateTime oDate = Convert.ToDateTime(iDate);
                                        if (otherDateTime > oDate)
                                        {
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        end_date = "";
                                    }
                                    if (id_number.Length > 14 || id_number.Length < 12)
                                    {
                                        id_number = "";
                                    }
                                }
                                string policy_number = content[0];
                                string product_name = content[1];
                                string first_name = content[2];
                                string last_name = content[3];
                                string initials = first_name.Substring(0, 1);

                                string medical_scheme = content[8];
                                string option_name = content[9];
                                string scheme_number = content[10];
                                string cellnumber = "";
                                string email = "";
                                if (medical_scheme == "To be confirmed")
                                {
                                    medical_scheme = "";
                                }
                                string str = "";
                                if (providername == "Healthbridge")
                                {
                                    str = String.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}|{15}", policy_number, first_name, initials, last_name, dob, benefiary_number, id_number, start_date, end_date, medical_scheme, option_name, scheme_number, date_joined, product_name, role, threshold);
                                    //First name|Initials|Last name|DOB|Beneficiary number|ID number|Policy start date|Policy end date|Medical aid name|Medical aid option name|Medical aid number|Date Joined Medical Aid|Policy/product name|Role|Threshold

                                }
                                else if (providername == "cgm")
                                {

                                    str = String.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\",\"{9}\",\"{10}\",\"{11}\",\"{12}\",\"{13}\",\"{14}\",\"{15}\"", policy_number, product_name, first_name, last_name, initials, id_number, dob, benefiary_number, start_date, end_date, medical_scheme, option_name, scheme_number, cellnumber, email, "");

                                }
                                else
                                {
                                    str = String.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}", product_id, policy_number, first_name, initials, last_name, dob, benefiary_number, id_number, start_date, end_date, medical_scheme, option_name, scheme_number, date_joined, product_name);
                                }
                                if (str.Length > 2)
                                {
                                    file.WriteLine(str);
                                }

                                Console.WriteLine(str);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                    }
                }
            }

        }
        public void runText(string sourcepath, string filename, string providername)
        {
            string[] lines = File.ReadAllLines(sourcepath, Encoding.UTF8);

            if (!File.Exists(filename))
            {


                File.WriteAllText(filename, String.Empty);
                using (System.IO.StreamWriter file = new StreamWriter(filename, true))
                {
                    string strx = @"Policy_number|First name|Initials|Last name|DOB|Beneficiary number|ID number|Policy start date|Policy end date|Medical aid name|Medical aid option name|Medical aid number|Date Joined Medical Aid|Policy/product name|Role|Threshold
";
                    if (providername == "Medswitch")
                    {
                        strx = @"Product Id|Policy number|First name|Initials|Last name|DOB|Beneficiary number|ID number|Policy start date|Policy end date|Medical aid name|Medical aid option name|Medical aid number|Date Joined Medical Aid|Policy/product name";
                    }
                    else if (providername == "cgm")
                    {
                        strx = String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15}", "\"Policy_number\"", "\"Policy / product name\"", "\"First name\"", "\"Last name\"", "\"Initials\"", "\"ID number\"", "\"DOB\"", "\"Beneficiary number\"", "\"Policy start date\"", "\"Policy end date\"", "\"Medical aid name\"", "\"Medical aid option name\"", "\"Medical aid number\"", "\"Cell phone number\"", "\"Email address\"", "\"Landline number\"");

                    }
                    if (strx.Length > 2)
                    {
                        file.WriteLine(strx);
                    }
                    Console.WriteLine(strx);

                    for (int i = 0; i < lines.Length; i++)
                    {
                        try
                        {

                            //PolicyNumber|ProductName|FirstName|Surname|client_idnumber|client_dateofbirth|policy_inceptiondate|policy_cancellationdate|client_lifemedicalscheme|client_lifemedicalschemeoption|client_lifemedicalschemepolicynumber|client_cellnumber|client_emailaddress    
                            if (lines[i] != "")
                            {
                                string contentx = lines[i].Replace(";", "");
                                string content0 = contentx.Replace("^", ";");
                                string content1 = content0.Replace("\",", "^");
                                string[] content = content1.Replace("\"", "").Split('|');
                                if (content.Length < 12 || content1.IndexOf("Policy") > -1)
                                {
                                    continue;
                                }
                                string product_id = "";
                                string date_joined = "";
                                string role = "member";
                                string threshold = "2500";
                                string dob = content[5].Replace("-", "/");
                                string benefiary_number = "";
                                string start_date = content[6].Replace("-", "/");
                                string end_date = content[7].Replace("-", "/");
                                string id_number = content[4];
                                if (content1.Contains("Policy"))
                                {
                                    product_id = "Product Id";
                                    date_joined = "Date Joined Medical Aid";
                                    role = "Role";
                                    threshold = "Threshold";
                                }
                                else
                                {

                                    dob = dob.Replace(" ", "");
                                    start_date = start_date.Replace(" ", "");
                                    end_date = end_date.Replace(" ", "");
                                    dob = dob.Length == 10 ? dob : "";
                                    start_date = start_date.Length == 10 ? start_date : "";
                                    if (end_date.Length == 10)
                                    {
                                        DateTime dateTime = DateTime.Now;
                                        DateTime otherDateTime = dateTime.AddDays(-90);
                                        string iDate = end_date.Replace("-", "/") + " 00:00:00";
                                        DateTime oDate = Convert.ToDateTime(iDate);
                                        if (otherDateTime > oDate)
                                        {
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        end_date = "";
                                    }
                                    if (id_number.Length > 14 || id_number.Length < 12)
                                    {
                                        id_number = "";
                                    }
                                }
                                // //PolicyNumber|ProductName|FirstName|Surname|client_idnumber|client_dateofbirth|policy_inceptiondate|policy_cancellationdate|client_lifemedicalscheme|client_lifemedicalschemeoption|client_lifemedicalschemepolicynumber|client_cellnumber|client_emailaddress    

                                string policy_number = content[0];
                                string product_name = content[1];
                                string first_name = content[2];
                                string last_name = content[3];
                                string initials = first_name.Substring(0, 1);

                                string medical_scheme = content[8];
                                string option_name = content[9];
                                string scheme_number = content[10];
                                string cellnumber = content[11];
                                string email = content[12];



                                if (medical_scheme == "To be confirmed")
                                {
                                    medical_scheme = "";
                                }
                                string str = "";
                                if (providername == "Healthbridge")
                                {
                                    str = String.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}|{15}", policy_number, first_name, initials, last_name, dob, benefiary_number, id_number, start_date, end_date, medical_scheme, option_name, scheme_number, date_joined, product_name, role, threshold);
                                    //First name|Initials|Last name|DOB|Beneficiary number|ID number|Policy start date|Policy end date|Medical aid name|Medical aid option name|Medical aid number|Date Joined Medical Aid|Policy/product name|Role|Threshold

                                }
                                else if (providername == "cgm")
                                {
                                    str = String.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\",\"{9}\",\"{10}\",\"{11}\",\"{12}\",\"{13}\",\"{14}\",\"{15}\"", policy_number, product_name, first_name, last_name, initials, id_number, dob, benefiary_number, start_date, end_date, medical_scheme, option_name, scheme_number, cellnumber, email, "");

                                }
                                else
                                {
                                    str = String.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}", product_id, policy_number, first_name, initials, last_name, dob, benefiary_number, id_number, start_date, end_date, medical_scheme, option_name, scheme_number, date_joined, product_name);
                                }
                                if (str.Length > 2)
                                {
                                    file.WriteLine(str);
                                }
                                Console.WriteLine(str);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                    }
                }
            }

        }
    }
}
