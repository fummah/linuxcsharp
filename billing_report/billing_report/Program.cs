using System;
using System.Data;
using System.IO;
using aaa_library;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Text;


namespace billing_report
{
    class Program    {
        static void Main(string[] args)
        {
            try
            {
                //copyMultiple(string copies,string subject, string body)
                Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                Console.WriteLine("start creating documents ...");
                using (DBConnect db = new DBConnect())
                {
                    DataSet ds = db.getAllDetails();
                    int num = ds.Tables[0].Rows.Count;
                    string copies = db.getConfigs().Tables[0].Rows[0]["admin_emails"].ToString();
                    Program pm = new Program();
                    DateTime dateTime2 = DateTime.Today;
                    dateTime2 = dateTime2.AddMonths(-1);
                    string datc = dateTime2.ToString("yyyy-MM");
                    pm.generateBillingReport(db, datc, copies);
                    /*
                    for (int i=0; i<num; i++)
                    {
                        string username= ds.Tables[0].Rows[i]["username"].ToString();
                        Console.WriteLine(username);
                        //htmltopdf.GeneratePDF(username);

                    }
                   */
                    int snapnum = db.getSnapClaims(datc).Tables[0].Rows.Count;
                    if (snapnum > 0)
                    {
                        Console.WriteLine("Claims already loaded");
                    }
                    else
                    {
                        Console.WriteLine("Claims loaded");
                        db.insertClosedCasesSnap(datc);
                        myStaticMethods.copyMultiple(copies, datc + " Closed Claims Snap Generated", "Hi<br>Closed claims snap successfully generated for " + datc + ". <br><br>MCA Team");
                    }

                }
                Console.WriteLine("Done");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("There is an error on reports : " + ex.Message);
                myStaticMethods.copyMultiple("tendai@medclaimassist.co.za", "Error on Monthly Billing Report", "There is an error on reports : " + ex.Message+". <br><br>MCA Team");

            }
            //Console.ReadLine();
        }

        void generateBillingReport(DBConnect db, string datc, string copies)
        {
            DateTime dateTime1 = DateTime.Today;
            dateTime1 = dateTime1.AddMonths(-1);
            string month = dateTime1.ToString("MMMM");
            string year = dateTime1.ToString("yyyy");



            string fulldate = month + " " + year;
            string subject = fulldate + " MCA Billing Report";

            string indwe = db.getBrokerTotals(1).Tables[0].Rows[0]["totals"].ToString();
            string xpandit = db.getBrokerTotals(718).Tables[0].Rows[0]["totals"].ToString();
            string angela = db.getBrokerTotals(697).Tables[0].Rows[0]["totals"].ToString();
            string cgm = db.getBrokerTotals(770).Tables[0].Rows[0]["totals"].ToString();
            string rfadvice = db.getBrokerTotals(985).Tables[0].Rows[0]["totals"].ToString();
            int tot = int.Parse(db.getClientSeamless("Kaelo").Tables[0].Rows[0]["totals"].ToString()) + int.Parse(db.getClientSeamless("Western").Tables[0].Rows[0]["totals"].ToString()) + int.Parse(db.getClientSeamless("Sanlam").Tables[0].Rows[0]["totals"].ToString());
            string kaelo_switch = formatNumber(tot);
            string sanlam_switch = formatNumber(int.Parse(db.getClientSeamless("Sanlam").Tables[0].Rows[0]["totals"].ToString()));
            string kaelo_savings = formatAmounts(reopenedCases(db, "Kaelo"));
            string western_savings = formatAmounts(reopenedCases(db, "Western"));
            string sanlam_savings = formatAmounts(reopenedCases(db, "Sanlam"));
            string turnberry_savings = formatAmounts(reopenedCases(db, "Turnberry", 1));
            string gaprisk_savings = formatAmounts(reopenedCases(db, "Gaprisk_administrators", 1));
            string cinagi_claims = formatNumber(int.Parse(db.getCinigeClaims("Cinagi", datc, "Clinical Review").Tables[0].Rows[0]["claims"].ToString()));
            string cinagi_claims1 = formatNumber(int.Parse(db.getCinigeClaims("Cinagi", datc, "PMB Negotiation").Tables[0].Rows[0]["claims"].ToString()));
            string totalrisk_claims = formatNumber(int.Parse(db.getTotalClaims("Total_risk_administrators", datc).Tables[0].Rows[0]["claims"].ToString()));

            string body = "Hi <br><br>Variable invoicing for MCA for " + fulldate + " is as follows:";
            body += "<br><ul>";
            body += "<li>Indwe - <b>" + indwe + " members</b></li>";
            body += "<li>XpandIT - <b>" + xpandit + " staff members</b></li>";
            body += "<li> Angela van Breda brokers - <b>" + angela + " clients</b></li>";
            body += "<li> RF Advice - <b>" + rfadvice + " members</b></li>";
            body += "<li>CGM - <b>" + cgm + " staff members</b></li>";
            body += "<li>Western savings - <b>" + western_savings + " (ex VAT)</b></li>";
            body += "<li>Kaelo savings - <b>" + kaelo_savings + " (ex VAT)</b></li>";
            body += "<li>Sanlam savings - <b>" + sanlam_savings + " (ex VAT)</b></li>";
            body += "<li>Turnberry savings - <b>" + turnberry_savings + "</b></li>";
            body += "<li>Gap Risk Administrators savings - <b>" + gaprisk_savings + "</b></li>";
            body += "<li>Kaelo(for Switch Assist claims) - <b>" + kaelo_switch + "</b> policyholders as per the " + fulldate + " cardholder file</li>";
            body += "<li>Total Risk Administrators - <b>" + totalrisk_claims + " cases</b></li>";
            body += "<li>Cinagi - <b>" + cinagi_claims + "</b> Clinical Advisory Service at <b>R610.00 (ex VAT)</b></li>";
            body += "<li>Cinagi - <b>" + cinagi_claims1 + "</b> PMB Negotiation at <b>R490.00 (ex VAT)</b></li>";
            body += "<li>Kaelo / Sanlam sFTP process for Seamless Claims - <b>" + sanlam_switch + "</b> Sanlam members on " + fulldate + " cardholder file</li>";
            body += "</ul><br>Regards,<br>MCA Team";
            myStaticMethods.copyMultiple(copies, subject, body);

            Console.WriteLine(body);
            /*
            Console.WriteLine("Indwe - " + indwe + " members");
            Console.WriteLine("XpandIT - " + xpandit + " staff members");
            Console.WriteLine(" Angela van Breda brokers - " + angela + " clients");
            Console.WriteLine("CGM - " + cgm + " staff members");
            Console.WriteLine("Western savings - " + western_savings + " (ex VAT)");
            Console.WriteLine("Kaelo savings - " + kaelo_savings + " (ex VAT)");
            Console.WriteLine("Sanlam savings - " + sanlam_savings + " (ex VAT)");
            Console.WriteLine("Turnberry savings - " + turnberry_savings + "");
            Console.WriteLine("Gap Risk Administrators savings - " + gaprisk_savings + "");
            Console.WriteLine("Kaelo(for Switch Assist claims) - " + kaelo_switch + " policyholders as per the " + fulldate + " cardholder file");
            Console.WriteLine("Total Risk Administrators - " + totalrisk_claims + " cases");
            Console.WriteLine("Cinagi - " + cinagi_claims + " Clinical Advisory Service at R610.00 (ex VAT)");
            Console.WriteLine("Cinagi - " + cinagi_claims1 + " PMB Negotiation at at R490.00 (ex VAT)");

            Console.WriteLine("Kaelo / Sanlam sFTP process for Seamless Claims - " + sanlam_switch + " Sanlam members on " + fulldate + " cardholder file");
      */
        }


        double reopenedCases(DBConnect db, string client_name, int role = 0)
        {
            DateTime dateTime1 = DateTime.Today;
            dateTime1 = dateTime1.AddMonths(-1);
            string dat = dateTime1.ToString("yyyy-MM");
            DataSet arr = db.getReopenedClaimsPerClient(client_name, dat);
            double total1 = 0.00;
            double total2 = 0.00;
            int numm = arr.Tables[0].Rows.Count;
            for (int i = 0; i < numm; i++)
            {
                //string x1 = arr.Tables[0].Rows[0]["first_savings"].ToString();
                //string x2 = arr.Tables[0].Rows[0]["final_savings"].ToString();
                string s1 = arr.Tables[0].Rows[i]["first_savings"].ToString();
                string s2 = arr.Tables[0].Rows[i]["final_savings"].ToString();
                double firstsavings;
                double lastsavings;
                bool isParsable = double.TryParse(s1, out firstsavings);
                bool isParsable1 = double.TryParse(s2, out lastsavings);
                if (!isParsable)
                {
                    firstsavings = 0.00;
                }
                else
                {
                    firstsavings = double.Parse(s1);
                }
                if (!isParsable1)
                {
                    lastsavings = 0.00;
                }
                else
                {
                    lastsavings = double.Parse(s2);
                }
                // Console.WriteLine(firstsavings.ToString() + "====" + lastsavings.ToString()+"-->"+ numm.ToString()+"....."+ dat);
                total1 += firstsavings;
                total2 += lastsavings;
            }
            //Console.WriteLine(total1.ToString() + "====" + total2.ToString()+"-->"+ client_name);

            double vari = total2 - total1;
            double savings = double.Parse(db.getClientsWithSavings(client_name, dat).Tables[0].Rows[0]["savings"].ToString());

            double actualsavings = savings - total1;
            //Console.WriteLine(client_name+"---"+savings.ToString() + "----" + actualsavings.ToString());

            double vat = 100.0 / 115.0;
            if (role == 0)
            {
                double vatexcl = Math.Round(actualsavings * vat, 2);
                return vatexcl;
            }
            else
            {
                return Math.Round(actualsavings, 2);
            }

        }
        string formatAmounts(double num)
        {
            string nn = ($" {num:C}");
            nn = nn.Replace("$", "R");
            return nn;
        }
        string formatNumber(int num)
        {
            string nn = ($" {num:C0}");
            nn = nn.Replace("$", string.Empty);
            return nn;
        }
    }
}

