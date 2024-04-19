using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using aaa_library;
using System.Data;

namespace quality_assurance

{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Stsrting ....");
            try
            {
                using (DBConnect db = new DBConnect())
                {
                    string currentDate = DateTime.Now.ToString("yyyy-MM-dd") + " 21";
                    //string currentDate = DateTime.Now.ToString("yyyy-MM-dd");

                    int num = db.getAllDetails().Tables[0].Rows.Count;
                    if (num > 0)
                    {

                        for (int i = 0; i < num; i++)
                        {

                            string username = db.getAllDetails().Tables[0].Rows[i]["username"].ToString();
                            int fg = db.getUserClaims(username).Tables[0].Rows.Count;
                            Console.WriteLine(fg.ToString() + "------" + username);
                            for (int j = 0; j < fg; j++)
                            {
                                string claim_id = db.getUserClaims(username).Tables[0].Rows[j]["claim_id"].ToString();
                                string claim_number = db.getUserClaims(username).Tables[0].Rows[j]["claim_number"].ToString();
                                db.updateQuality(int.Parse(claim_id));
                                Console.WriteLine(claim_number + "====" + claim_id);
                            }
                            Console.WriteLine("-=-=-=-");

                        }

                    }


                    for (int i = 0; i < db.getAdmedNightclaims(currentDate).Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            string claimN = db.getAdmedNightclaims(currentDate).Tables[0].Rows[i]["claim_id"].ToString();
                            int claim_idS = int.Parse(claimN);
                            string yuser = claim_idS % 2 == 0 ? "Keasha" : "Keasha";
                            db.updateNewAdmed(claim_idS, yuser);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
            //Console.ReadLine();
        }
    }
}
