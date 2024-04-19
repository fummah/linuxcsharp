// Decompiled with JetBrains decompiler
// Type: aaa_library.DBConnect
// Assembly: aaa_library, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5E795FC6-0075-4108-BBFD-3BF70EE3AAFE
// Assembly location: C:\Users\fumma\OneDrive\Desktop\admed_csv\admed_csv\bin\Debug\aaa_library.dll

using Microsoft.Win32.SafeHandles;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace aaa_library
{
    public class DBConnect : IDisposable
    {
        private MySqlConnection connection;
        private MySqlConnection connection1;
        private MySqlConnection connection2;
        private MySqlConnection connection_seamless;
        private string server;
        private string database;
        private string uid;
        private string password;
        private string serverC;
        private string databaseC;
        private string uidC;
        private string passwordC;
        private string serverD;
        private string databaseD;
        private string uidD;
        private string passwordD;
        private string server_seamless;
        private string database_seamless;
        private string uid_seamless;
        private string password_seamless;
        public int member_id { get; set; }
        public int myClaim_id { get; set; }
        bool disposed = false;
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
        //Constructor
        public DBConnect(string db = "production")
        {
            myConfig(db);
        }

        //Initialize values
        private void myConfig(string db)
        {
            string basehome = Environment.GetEnvironmentVariable("HOME");
            string prod = basehome + @"/files/xc/xpx2.txt";
            string seamless = basehome + @"/files/xc/xpx2_seamless.txt";

            string[] lines = db == "production" ? File.ReadAllLines(prod, Encoding.UTF8) : File.ReadAllLines(seamless, Encoding.UTF8); ;
            string[] lines_seamless = File.ReadAllLines(seamless, Encoding.UTF8);
            //Main DB
                   
            server = details(lines[1])[0];
            database = details(lines[1])[1];
            uid = details(lines[1])[2];
            password = details(lines[1])[3];

            //Seamless DB
            server_seamless = details(lines_seamless[1])[0];
            database_seamless = details(lines_seamless[1])[1];
            uid_seamless = details(lines_seamless[1])[2];
            password_seamless = details(lines_seamless[1])[3];

            // Coding databse connection
            serverC = details(lines[2])[0];
            databaseC = details(lines[2])[1];
            uidC = details(lines[2])[2];
            passwordC = details(lines[2])[3];
            // doctors databse connection
            serverD = details(lines[3])[0];
            databaseD = details(lines[3])[1];
            uidD = details(lines[3])[2];
            passwordD = details(lines[3])[3];

            string connectionString;
            string connectionString1;
            string connectionString2;
            string connectionString_seamless;
            connectionString = @"SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PWD=" + password + ";";
            connectionString1 = @"SERVER=" + serverC + ";" + "DATABASE=" + databaseC + ";" + "UID=" + uidC + ";" + "PWD=" + passwordC + ";";
            connectionString2 = @"SERVER=" + serverD + ";" + "DATABASE=" + databaseD + ";" + "UID=" + uidD + ";" + "PWD=" + passwordD + ";";
            connectionString_seamless = @"SERVER=" + server_seamless + ";" + "DATABASE=" + database_seamless + ";" + "UID=" + uid_seamless + ";" + "PWD=" + password_seamless + ";";
            connection = new MySqlConnection(connectionString);
            connection1 = new MySqlConnection(connectionString1);
            connection2 = new MySqlConnection(connectionString2);
            connection_seamless = new MySqlConnection(connectionString_seamless);
         
        }
        private static string[] details(string txt)
        {
            string[] det = txt.Split(new[] { "[]" }, StringSplitOptions.None);
            return det;
        }

        private bool OpenConnection()
        {
            try
            {
                this.connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Testing --- "+ex.Message);
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine(ex.Message);
                        break;
                    case 1045:
                        Console.WriteLine("Invalid username/password, please try again" + ex.Message);
                        break;
                }
                return false;
            }
        }

        private bool CloseConnection()
        {
            try
            {
                this.connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        private bool OpenConnection1()
        {
            try
            {
                this.connection1.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine("ndapererwa hangu" + ex.Message);
                        break;
                    case 1045:
                        Console.WriteLine("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        private bool CloseConnection1()
        {
            try
            {
                this.connection1.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        private bool OpenConnection2()
        {
            try
            {
                this.connection2.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine(ex.Message);
                        break;
                    case 1045:
                        Console.WriteLine("Invalid username/password, please try again" + ex.Message);
                        break;
                }
                return false;
            }
        }

        private bool CloseConnection2()
        {
            try
            {
                this.connection2.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        private bool OpenConnection_seamless()
        {
            try
            {
                this.connection_seamless.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine(ex.Message);
                        break;
                    case 1045:
                        Console.WriteLine("Invalid username/password, please try again" + ex.Message);
                        break;
                }
                return false;
            }
        }
        private bool CloseConnection_seamless()
        {
            try
            {
                this.connection_seamless.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public void InsertMember(
          int client_id,
          string policy_number = "",
          string member_name = "",
          string member_surname = "",
          string telephone = "",
          string email = "",
          string medical_aid = "Unknown",
          string cell = "",
          string product_name = "",
          string product_code = "",
          string benefitiary_number = "",
          string scheme_option = "",
          string scheme_number = "",
          string id_number = "")
        {
            string cmdText = "INSERT INTO member (client_id,policy_number,first_name,surname,productName,telephone,cell,email,medical_scheme,product_code,beneficiary_number,scheme_option,scheme_number,id_number) VALUES(@client_id,@policy_number,@member_name,@member_surname,@productName,@telephone,@cell,@email,@medical_scheme,@product_code,@benefitiary_number,@scheme_option,@scheme_number,@id_number)";
            using (this.connection)
            {
                if (!this.OpenConnection())
                    return;
                MySqlCommand mySqlCommand = new MySqlCommand(cmdText, this.connection);
                mySqlCommand.Parameters.AddWithValue("@client_id", (object)client_id);
                mySqlCommand.Parameters.AddWithValue("@policy_number", (object)policy_number);
                mySqlCommand.Parameters.AddWithValue("@member_name", (object)member_name);
                mySqlCommand.Parameters.AddWithValue("@member_surname", (object)member_surname);
                mySqlCommand.Parameters.AddWithValue("@productName", (object)product_name);
                mySqlCommand.Parameters.AddWithValue("@telephone", (object)telephone);
                mySqlCommand.Parameters.AddWithValue("@cell", (object)cell);
                mySqlCommand.Parameters.AddWithValue("@email", (object)email);
                mySqlCommand.Parameters.AddWithValue("@medical_scheme", (object)medical_aid);
                mySqlCommand.Parameters.AddWithValue("@product_code", (object)product_code);
                mySqlCommand.Parameters.AddWithValue("@benefitiary_number", (object)benefitiary_number);
                mySqlCommand.Parameters.AddWithValue("@scheme_option", (object)scheme_option);
                mySqlCommand.Parameters.AddWithValue("@scheme_number", (object)scheme_number);
                mySqlCommand.Parameters.AddWithValue("@id_number", (object)id_number);
                mySqlCommand.ExecuteNonQuery();
                this.CloseConnection();
                Thread.Sleep(1000);
            }
        }

        public void InsertClaim(
          int member_id,
          string claim_number,
          string username,
          string folder = "",
          DateTime? service_date = null,
          string icd10 = "",
          int pmb = 0,
          double amount_charged = 0.0,
          double client_gap = 0.0,
          string icd10_descr = "",
          double scheme_paid = 0.0,
          int open = 1,
          string preassessor = "",
          string patient_number = "",
          string patient_dob = "",
          string patient_gender = "",
          string claim_authnum="",
          double tariff_amnt=0.0,
          string senderId=""
          )
        {
            string cmdText = "INSERT INTO claim (member_id,claim_number,username,icd10,pmb,charged_amnt,Service_Date,my_folder,icd10_desc,scheme_paid,client_gap,Open,preassessor,patient_number,patient_dob,patient_gender,claim_authnum,tariff_amnt,senderId) VALUES(@member_id,@claim_number,@username,@icd10,@pmb,@charged_amnt,@Service_Date,@my_folder,@icd10_desc,@scheme_paid,@client_gap,@open,@preassessor,@patient_number,@patient_dob,@patient_gender,@claim_authnum,@tariff_amnt,@senderId)";
            using (this.connection)
            {
                if (!this.OpenConnection())
                    return;
                MySqlCommand mySqlCommand = new MySqlCommand(cmdText, this.connection);
                mySqlCommand.Parameters.AddWithValue("@member_id", (object)member_id);
                mySqlCommand.Parameters.AddWithValue("@claim_number", (object)claim_number);
                mySqlCommand.Parameters.AddWithValue("@username", (object)username);
                mySqlCommand.Parameters.AddWithValue("@Service_Date", (object)service_date);
                mySqlCommand.Parameters.AddWithValue("@icd10", (object)icd10);
                mySqlCommand.Parameters.AddWithValue("@pmb", (object)pmb);
                mySqlCommand.Parameters.AddWithValue("@charged_amnt", (object)amount_charged);
                mySqlCommand.Parameters.AddWithValue("@my_folder", (object)folder);
                mySqlCommand.Parameters.AddWithValue("@icd10_desc", (object)icd10_descr);
                mySqlCommand.Parameters.AddWithValue("@scheme_paid", (object)scheme_paid);
                mySqlCommand.Parameters.AddWithValue("@client_gap", (object)client_gap);
                mySqlCommand.Parameters.AddWithValue("@open", (object)open);
                mySqlCommand.Parameters.AddWithValue("@preassessor", (object)preassessor);
                mySqlCommand.Parameters.AddWithValue("@patient_number", (object)patient_number);
                mySqlCommand.Parameters.AddWithValue("@patient_dob", (object)patient_dob);
                mySqlCommand.Parameters.AddWithValue("@patient_gender", (object)patient_gender);
                mySqlCommand.Parameters.AddWithValue("@claim_authnum", (object)claim_authnum);
                mySqlCommand.Parameters.AddWithValue("@tariff_amnt", (object)tariff_amnt);
                mySqlCommand.Parameters.AddWithValue("@senderId", (object)senderId);
                mySqlCommand.ExecuteNonQuery();
                this.CloseConnection();
                Thread.Sleep(1000);
            }
        }

        public void Insert(
          int user_id,
          int client_id,
          string policy_number,
          string claim_number,
          string member_name,
          string member_surname,
          string patient_name,
          string username,
          string icd10,
          int pmb,
          int amount_charged,
          int gap,
          DateTime service_date)
        {
            string cmdText = "INSERT INTO claim (user_id, client_id,policy_number,claim_number,member_name,member_surname,patient_name,username,medical_scheme,scheme_number,id_number,doc_name_1,doc_name_2,doc_name_3,doc_name_4,doc_name_5,prac_num_1,prac_num_2,prac_num_3,prac_num_4,prac_num_5,icd10,icd10_desc,pmb,memb_telephone,memb_cell,memb_email,scheme_option,savings_scheme,savings_discount,charged_amnt,scheme_paid,gap,Open,date_entered,Service_Date) VALUES(@user_id,@client_id,@policy_number,@claim_number,@member_name,@member_surname,@patient_name,@username,@medical_scheme,@scheme_number,@id_number,@doc_name_1,@doc_name_2,@doc_name_3,@doc_name_4,@doc_name_5,@prac_num_1,@prac_num_2,@prac_num_3,@prac_num_4,@prac_num_5,@icd10,@icd10_desc,@pmb,@memb_telephone,@memb_cell,@memb_email,@scheme_option,@scheme_savings,@discount_savings,@charged_amnt,@scheme_paid,@gap,@Open,@date_entered,@Service_Date)";
            using (this.connection)
            {
                if (!this.OpenConnection())
                    return;
                MySqlCommand mySqlCommand = new MySqlCommand(cmdText, this.connection);
                mySqlCommand.Parameters.AddWithValue("@user_id", (object)user_id);
                mySqlCommand.Parameters.AddWithValue("@client_id", (object)client_id);
                mySqlCommand.Parameters.AddWithValue("@policy_number", (object)policy_number);
                mySqlCommand.Parameters.AddWithValue("@claim_number", (object)claim_number);
                mySqlCommand.Parameters.AddWithValue("@member_name", (object)member_name);
                mySqlCommand.Parameters.AddWithValue("@member_surname", (object)member_surname);
                mySqlCommand.Parameters.AddWithValue("@patient_name", (object)patient_name);
                mySqlCommand.Parameters.AddWithValue("@username", (object)username);
                mySqlCommand.Parameters.AddWithValue("@medical_scheme", (object)"");
                mySqlCommand.Parameters.AddWithValue("@scheme_number", (object)"");
                mySqlCommand.Parameters.AddWithValue("@id_number", (object)"");
                mySqlCommand.Parameters.AddWithValue("@Service_Date", (object)service_date);
                mySqlCommand.Parameters.AddWithValue("@doc_name_1", (object)"");
                mySqlCommand.Parameters.AddWithValue("@doc_name_2", (object)"");
                mySqlCommand.Parameters.AddWithValue("@doc_name_3", (object)"");
                mySqlCommand.Parameters.AddWithValue("@doc_name_4", (object)"");
                mySqlCommand.Parameters.AddWithValue("@doc_name_5", (object)"");
                mySqlCommand.Parameters.AddWithValue("@prac_num_1", (object)0);
                mySqlCommand.Parameters.AddWithValue("@prac_num_2", (object)0);
                mySqlCommand.Parameters.AddWithValue("@prac_num_3", (object)0);
                mySqlCommand.Parameters.AddWithValue("@prac_num_4", (object)0);
                mySqlCommand.Parameters.AddWithValue("@prac_num_5", (object)0);
                mySqlCommand.Parameters.AddWithValue("@icd10", (object)icd10);
                mySqlCommand.Parameters.AddWithValue("@icd10_desc", (object)"");
                mySqlCommand.Parameters.AddWithValue("@pmb", (object)pmb);
                mySqlCommand.Parameters.AddWithValue("@memb_telephone", (object)"");
                mySqlCommand.Parameters.AddWithValue("@memb_cell", (object)"");
                mySqlCommand.Parameters.AddWithValue("@memb_email", (object)"");
                mySqlCommand.Parameters.AddWithValue("@scheme_option", (object)"");
                mySqlCommand.Parameters.AddWithValue("@Open", (object)1);
                mySqlCommand.Parameters.AddWithValue("@date_entered", (object)DateTime.Now);
                mySqlCommand.Parameters.AddWithValue("@scheme_savings", (object)0);
                mySqlCommand.Parameters.AddWithValue("@discount_savings", (object)0);
                mySqlCommand.Parameters.AddWithValue("@charged_amnt", (object)amount_charged);
                mySqlCommand.Parameters.AddWithValue("@scheme_paid", (object)0);
                mySqlCommand.Parameters.AddWithValue("@gap", (object)gap);
                mySqlCommand.ExecuteNonQuery();
                this.CloseConnection();
                Thread.Sleep(1000);
            }
        }

        public void InsertPatient(int claim_id, string patient_name)
        {
            string cmdText = "INSERT INTO patient (claim_id, patient_name) VALUES(@claim_id,@patient_name)";
            using (this.connection)
            {
                if (!this.OpenConnection())
                    return;
                MySqlCommand mySqlCommand = new MySqlCommand(cmdText, this.connection);
                mySqlCommand.Parameters.AddWithValue("@claim_id", (object)claim_id);
                mySqlCommand.Parameters.AddWithValue("@patient_name", (object)patient_name);
                mySqlCommand.ExecuteNonQuery();
                this.CloseConnection();
                Thread.Sleep(1000);
            }
        }

        public void insertDocument(int claim_id, string desc, int email_id = 0)
        {
            string cmdText = "INSERT INTO documents (claim_id,doc_description, doc_type,doc_size,date,randomNum) VALUES(@id,@desc,@type,@size,@date,@random)";
            using (this.connection)
            {
                this.OpenConnection();
                MySqlCommand mySqlCommand = new MySqlCommand(cmdText, this.connection);
                mySqlCommand.Parameters.AddWithValue("@id", (object)claim_id);
                mySqlCommand.Parameters.AddWithValue("@desc", (object)desc);
                mySqlCommand.Parameters.AddWithValue("@type", (object)"pdf");
                mySqlCommand.Parameters.AddWithValue("@size", (object)1000);
                mySqlCommand.Parameters.AddWithValue("@date", (object)DateTime.Now);
                mySqlCommand.Parameters.AddWithValue("@random", (object)MySftp.rr);
                mySqlCommand.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        public void Update(int id)
        {
            DateTime now = DateTime.Now;
            string cmdText = "UPDATE users_information SET datetime=@time WHERE id=@id AND status=@status";
            using (this.connection)
            {
                if (!this.OpenConnection())
                    return;
                MySqlCommand mySqlCommand = new MySqlCommand(cmdText, this.connection);
                mySqlCommand.Parameters.AddWithValue("@time", (object)now);
                mySqlCommand.Parameters.AddWithValue("@id", (object)id);
                mySqlCommand.Parameters.AddWithValue("@status", (object)1);
                mySqlCommand.ExecuteNonQuery();
                this.CloseConnection();
                Thread.Sleep(1000);
            }
        }
        public void updatePatient(int claim_id,string patient_name)
        {
            DateTime now = DateTime.Now;
            string cmdText = "UPDATE patient SET patient_name=@patient_name WHERE claim_id=@claim_id";
            using (this.connection)
            {
                if (!this.OpenConnection())
                    return;
                MySqlCommand mySqlCommand = new MySqlCommand(cmdText, this.connection);
                mySqlCommand.Parameters.AddWithValue("@claim_id", (object)claim_id);
                mySqlCommand.Parameters.AddWithValue("@patient_name", (object)patient_name);              
                mySqlCommand.ExecuteNonQuery();
                this.CloseConnection();
                Thread.Sleep(1000);
            }
        }

        public void UpdateFolder(int id)
        {
            DateTime now = DateTime.Now;
            string cmdText = "UPDATE claim SET my_folder='' WHERE claim_id=@id";
            using (this.connection)
            {
                if (!this.OpenConnection())
                    return;
                MySqlCommand mySqlCommand = new MySqlCommand(cmdText, this.connection);
                mySqlCommand.Parameters.AddWithValue("@id", (object)id);
                mySqlCommand.ExecuteNonQuery();
                this.CloseConnection();
                Thread.Sleep(1000);
            }
        }

        public void updateConfig(
          string email,
          string password,
          string source,
          string destination,
          string smtp,
          string imap,
          string cc)
        {
            string cmdText = "UPDATE email_configs SET email=@email,password=@password,source_folder=@source,destination_folder=@destination,smtp_server=@smtp,imap_server=@imap,cc=@cc";
            using (this.connection)
            {
                if (!this.OpenConnection())
                    return;
                MySqlCommand mySqlCommand = new MySqlCommand(cmdText, this.connection);
                mySqlCommand.Parameters.AddWithValue("@email", (object)email);
                mySqlCommand.Parameters.AddWithValue("@password", (object)password);
                mySqlCommand.Parameters.AddWithValue("@source", (object)source);
                mySqlCommand.Parameters.AddWithValue("@destination", (object)destination);
                mySqlCommand.Parameters.AddWithValue("@smtp", (object)smtp);
                mySqlCommand.Parameters.AddWithValue("@imap", (object)imap);
                mySqlCommand.Parameters.AddWithValue("@cc", (object)cc);
                if (mySqlCommand.ExecuteNonQuery() == 1)
                {
                    int num1 = (int)MessageBox.Show("Configurations Updated");
                }
                else
                {
                    int num2 = (int)MessageBox.Show("Update Failed");
                }
                this.CloseConnection();
            }
        }

        public DataSet getMember(string policy_number = "")
        {
            DataSet dataSet = new DataSet();
            string cmdText = "SELECT MAX(member_id) as member_id FROM member WHERE policy_number=@pol";
            using (this.connection)
            {
                if (this.OpenConnection())
                {
                    MySqlCommand selectCommand = new MySqlCommand(cmdText, this.connection);
                    selectCommand.Parameters.AddWithValue("@pol", (object)policy_number);
                    new MySqlDataAdapter(selectCommand).Fill(dataSet);
                    this.CloseConnection();
                }
            }
            return dataSet;
        }

        public DataSet getMember1(string policy_number, int client_id)
        {
            DataSet dataSet = new DataSet();
            string cmdText = "SELECT member_id,policy_number FROM member WHERE policy_number=@pol AND client_id=@client_id";
            using (this.connection)
            {
                if (this.OpenConnection())
                {
                    MySqlCommand selectCommand = new MySqlCommand(cmdText, this.connection);
                    selectCommand.Parameters.AddWithValue("@pol", (object)policy_number);
                    selectCommand.Parameters.AddWithValue("@client_id", (object)client_id);
                    new MySqlDataAdapter(selectCommand).Fill(dataSet);
                    this.CloseConnection();
                }
            }
            return dataSet;
        }

        public DataSet getUser(string policy_number)
        {
            DataSet dataSet = new DataSet();
            string cmdText = "SELECT a.username FROM claim as a inner join member as b on a.member_id=b.member_id WHERE b.policy_number=@pol";
            using (this.connection)
            {
                if (this.OpenConnection())
                {
                    MySqlCommand selectCommand = new MySqlCommand(cmdText, this.connection);
                    selectCommand.Parameters.AddWithValue("@pol", (object)policy_number);
                    new MySqlDataAdapter(selectCommand).Fill(dataSet);
                    this.CloseConnection();
                }
            }
            return dataSet;
        }

        public DataSet getConfigs()
        {
            DataSet dataSet = new DataSet();
            string cmdText = "SELECT *FROM email_configs LIMIT 1";
            using (this.connection)
            {
                if (this.OpenConnection())
                {
                    new MySqlDataAdapter(new MySqlCommand(cmdText, this.connection)).Fill(dataSet);
                    this.CloseConnection();
                }
            }
            return dataSet;
        }

        public DataSet getPMBstatus(string icd10)
        {
            DataSet dataSet = new DataSet();
            string cmdText = "SELECT pmb_code,shortdesc FROM Diagnosis WHERE diag_code=@icd10 LIMIT 1";
            using (this.connection1)
            {
                if (this.OpenConnection1())
                {
                    MySqlCommand selectCommand = new MySqlCommand(cmdText, this.connection1);
                    selectCommand.Parameters.AddWithValue("@icd10", (object)icd10);
                    new MySqlDataAdapter(selectCommand).Fill(dataSet);
                    this.CloseConnection1();
                }
            }
            return dataSet;
        }

        public DataSet getMissingDoc(string claim, int client_id)
        {
            DataSet dataSet = new DataSet();
            string cmdText = "SELECT *FROM documents WHERE claim_id=@num";
            int num = int.Parse(this.getDuplicate1(claim, client_id).Tables[0].Rows[0]["claim_id"].ToString());
            using (this.connection)
            {
                if (this.OpenConnection())
                {
                    MySqlCommand selectCommand = new MySqlCommand(cmdText, this.connection);
                    selectCommand.Parameters.AddWithValue("@num", (object)num);
                    new MySqlDataAdapter(selectCommand).Fill(dataSet);
                    this.CloseConnection();
                }
            }
            return dataSet;
        }

        public DataSet getDetails()
        {
            DataSet ds = new DataSet();            
            try
            {
                string query = "SELECT *FROM users_information ORDER BY datetime ASC LIMIT 1";
                using (connection)
                {
                    //Open connection
                 
                    if (this.OpenConnection() == true)
                    {                        
                        //Create Command
                        MySqlCommand cmd = new MySqlCommand(query, connection);
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);

                        da.Fill(ds);

                        //close Connection
                        this.CloseConnection();

                        //return list to be displayed

                    }
                }
            }
            catch(Exception e)

            {
                Console.WriteLine("Myyy err "+e.Message);
            }
            return ds;

        }
        public DataSet getEmail(string username)
        {
            DataSet dataSet = new DataSet();
            string cmdText = "SELECT email FROM users_information WHERE username=@username";
            using (this.connection)
            {
                if (this.OpenConnection())
                {
                    MySqlCommand selectCommand = new MySqlCommand(cmdText, this.connection);
                    selectCommand.Parameters.AddWithValue("@username", (object)username);
                    new MySqlDataAdapter(selectCommand).Fill(dataSet);
                    this.CloseConnection();
                }
            }
            return dataSet;
        }

        public DataSet getDuplicate(string num, int client)
        {
            num = num.Trim();
            num = num.Replace(" ", "");
            DataSet dataSet = new DataSet();
            string cmdText = "SELECT MAX(a.claim_id) as claim_id, a.claim_number FROM claim as a INNER JOIN member as b ON a.member_id=b.member_id WHERE a.claim_number=@num AND b.client_id=@client";
            using (this.connection)
            {
                if (this.OpenConnection())
                {
                    MySqlCommand selectCommand = new MySqlCommand(cmdText, this.connection);
                    selectCommand.Parameters.AddWithValue("@num", (object)num);
                    selectCommand.Parameters.AddWithValue("@client", (object)client);
                    new MySqlDataAdapter(selectCommand).Fill(dataSet);
                    this.CloseConnection();
                }
            }
            return dataSet;
        }

        public DataSet getDuplicate2(string num, int client)
        {
            num = num.Trim();
            num = num.Replace(" ", "");
            DataSet dataSet = new DataSet();
            string cmdText = "SELECT a.claim_number FROM claim as a INNER JOIN member as b ON a.member_id=b.member_id WHERE a.claim_number=@num AND b.client_id=@client";
            using (this.connection)
            {
                if (this.OpenConnection())
                {
                    MySqlCommand selectCommand = new MySqlCommand(cmdText, this.connection);
                    selectCommand.Parameters.AddWithValue("@num", (object)num);
                    selectCommand.Parameters.AddWithValue("@client", (object)client);
                    new MySqlDataAdapter(selectCommand).Fill(dataSet);
                    this.CloseConnection();
                }
            }
            return dataSet;
        }

        public DataSet getXXXX(string num)
        {
            DataSet dataSet = new DataSet();
            string cmdText = "SELECT claim_id FROM claim WHERE claim_number=@num";
            using (this.connection)
            {
                if (this.OpenConnection())
                {
                    MySqlCommand selectCommand = new MySqlCommand(cmdText, this.connection);
                    selectCommand.Parameters.AddWithValue("@num", (object)num);
                    new MySqlDataAdapter(selectCommand).Fill(dataSet);
                    this.CloseConnection();
                }
            }
            return dataSet;
        }

        public DataSet getDuplicate1(string num, int client_id)
        {
            DataSet dataSet = new DataSet();
            string cmdText = "SELECT a.claim_id,a.Open,a.icd10,b.policy_number,a.member_id,a.date_closed,a.savings_scheme,a.savings_discount FROM claim as a INNER JOIN member as b ON a.member_id=b.member_id WHERE a.claim_number=@num AND b.client_id=@client_id";
            using (this.connection)
            {
                if (this.OpenConnection())
                {
                    MySqlCommand selectCommand = new MySqlCommand(cmdText, this.connection);
                    selectCommand.Parameters.AddWithValue("@num", (object)num);
                    selectCommand.Parameters.AddWithValue("@client_id", (object)client_id);
                    new MySqlDataAdapter(selectCommand).Fill(dataSet);
                    this.CloseConnection();
                }
            }
            return dataSet;
        }

        public DataSet getToFinal()
        {
            DataSet dataSet = new DataSet();
            string cmdText = "SELECT my_folder,claim_id FROM claim  WHERE my_folder<>'' AND Open=0";
            using (this.connection)
            {
                if (this.OpenConnection())
                {
                    new MySqlDataAdapter(new MySqlCommand(cmdText, this.connection)).Fill(dataSet);
                    this.CloseConnection();
                }
            }
            return dataSet;
        }

        public void Logs(string owner, string desc)
        {
            string cmdText = "INSERT INTO logs (owner, description) VALUES(@owner,@desc)";
            using (this.connection)
            {
                if (!this.OpenConnection())
                    return;
                MySqlCommand mySqlCommand = new MySqlCommand(cmdText, this.connection);
                mySqlCommand.Parameters.AddWithValue("@owner", (object)owner);
                mySqlCommand.Parameters.AddWithValue("@desc", (object)desc);
                mySqlCommand.ExecuteNonQuery();
                this.CloseConnection();
                Thread.Sleep(1000);
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object)this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
                return;
            if (disposing)
                this.handle.Dispose();
            this.disposed = true;
        }

        public DataTable excelData(int client_id, string yesterday, int len = 8)
        {
            DayOfWeek dayOfWeek = DateTime.Now.DayOfWeek;
            DateTime dateTime1 = DateTime.Today;
            dateTime1 = dateTime1.AddDays(-2.0);
            string str1 = dateTime1.ToString("yyyy-MM-dd");
            DateTime dateTime2 = DateTime.Today;
            dateTime2 = dateTime2.AddDays(-1.0);
            string str2 = dateTime2.ToString("yyyy-MM-dd");
            string cmdText = "SELECT c.policy_number as POLICY_NUMBER,a.claim_number as CLAIM_NUMBER,b.date_entered as INTERVENTION_DATE,b.intervention_desc as INTERVENTION_DESCRIPTION,a.Open,a.savings_scheme as SCHEME_SAVINGS,a.savings_discount as DISCOUNT_SAVINGS FROM claim as a INNER JOIN member as c ON a.member_id=c.member_id  INNER JOIN intervention as b ON a.claim_id=b.claim_id WHERE b.date_entered LIKE @date AND c.client_id=@id";
            if (len > 8)
                cmdText = "SELECT c.policy_number as POLICY_NUMBER,a.claim_number as CLAIM_NUMBER,b.date_entered as INTERVENTION_DATE,b.intervention_desc as INTERVENTION_DESCRIPTION,a.Open,d.savings_scheme as SCHEME_SAVINGS,d.savings_discount as DISCOUNT_SAVINGS,d.pay_doctor as PAY_DOCTOR,b.practice_number as PRACTICE_NUMBER,b.doc_name as PROVIDER_NAME FROM intervention as b INNER JOIN doctors as d ON b.practice_number=d.practice_number AND b.claim_id=d.claim_id INNER JOIN claim as a ON b.claim_id=a.claim_id INNER JOIN member as c ON a.member_id=c.member_id WHERE b.date_entered LIKE @date AND c.client_id=@id";
            if (dayOfWeek.ToString() == "Monday")
            {
                yesterday = DateTime.Today.AddDays(-3.0).ToString("yyyy-MM-dd");
                cmdText = "SELECT c.policy_number as POLICY_NUMBER,a.claim_number as CLAIM_NUMBER,b.date_entered as INTERVENTION_DATE,b.intervention_desc as INTERVENTION_DESCRIPTION,a.Open,a.savings_scheme as SCHEME_SAVINGS,a.savings_discount as DISCOUNT_SAVINGS FROM claim as a INNER JOIN member as c ON a.member_id=c.member_id INNER JOIN intervention as b ON a.claim_id=b.claim_id WHERE (b.date_entered LIKE @date OR b.date_entered LIKE @date1 OR b.date_entered LIKE @date2) AND c.client_id=@id";
                if (len > 8)
                    cmdText = "SELECT c.policy_number as POLICY_NUMBER,a.claim_number as CLAIM_NUMBER,b.date_entered as INTERVENTION_DATE,b.intervention_desc as INTERVENTION_DESCRIPTION,a.Open,d.savings_scheme as SCHEME_SAVINGS,d.savings_discount as DISCOUNT_SAVINGS,d.pay_doctor as PAY_DOCTOR,b.practice_number as PRACTICE_NUMBER,b.doc_name as PROVIDER_NAME FROM intervention as b INNER JOIN doctors as d ON b.practice_number=d.practice_number AND b.claim_id=d.claim_id INNER JOIN claim as a ON b.claim_id=a.claim_id INNER JOIN member as c ON a.member_id=c.member_id WHERE (b.date_entered LIKE @date OR b.date_entered LIKE @date1 OR b.date_entered LIKE @date2) AND c.client_id=@id";
            }
            DataTable dataTable = new DataTable();
            using (this.connection)
            {
                if (this.OpenConnection())
                {
                    MySqlCommand selectCommand = new MySqlCommand(cmdText, this.connection);
                    selectCommand.Parameters.AddWithValue("@date", (object)(yesterday + "%"));
                    selectCommand.Parameters.AddWithValue("@date1", (object)(str1 + "%"));
                    selectCommand.Parameters.AddWithValue("@date2", (object)(str2 + "%"));
                    selectCommand.Parameters.AddWithValue("@id", (object)client_id);
                    new MySqlDataAdapter(selectCommand).Fill(dataTable);
                    this.CloseConnection();
                }
            }
            return dataTable;
        }

        public DataSet fetchClient1()
        {
            DataSet dataSet = new DataSet();
            string cmdText = "SELECT DISTINCT reporting_client_id, client_name,advanced FROM clients WHERE reporting_status=@num";
            using (this.connection)
            {
                if (this.OpenConnection())
                {
                    MySqlCommand selectCommand = new MySqlCommand(cmdText, this.connection);
                    selectCommand.Parameters.AddWithValue("@num", (object)1);
                    new MySqlDataAdapter(selectCommand).Fill(dataSet);
                    this.CloseConnection();
                }
            }
            return dataSet;
        }

        public DataSet fetchClient()
        {
            DataSet dataSet = new DataSet();
            string cmdText = "SELECT *FROM clients WHERE reporting_status=@num";
            using (this.connection)
            {
                if (this.OpenConnection())
                {
                    MySqlCommand selectCommand = new MySqlCommand(cmdText, this.connection);
                    selectCommand.Parameters.AddWithValue("@num", (object)1);
                    new MySqlDataAdapter(selectCommand).Fill(dataSet);
                    this.CloseConnection();
                }
            }
            return dataSet;
        }

        public DataTable get10DayClaims()
        {
            DataTable dataTable = new DataTable();
            DateTime dateTime = DateTime.Today;
            dateTime = dateTime.AddDays(-10.0);
            string str = dateTime.ToString("yyyy-MM-dd");
            string cmdText = "SELECT a.claim_id,a.claim_number,b.first_name,b.surname,b.medical_scheme, b.policy_number,a.gap,a.hasDrPaid FROM claim as a inner join member as b ON a.member_id=b.member_id WHERE claim_id NOT IN(SELECT claim_id FROM 10daysclaims) AND a.date_entered <= @dat AND b.client_id=7 AND a.Open=1 ORDER BY a.claim_id DESC LIMIT 20";
            using (this.connection)
            {
                if (this.OpenConnection())
                {
                    MySqlCommand selectCommand = new MySqlCommand(cmdText, this.connection);
                    selectCommand.Parameters.AddWithValue("@dat", (object)str);
                    new MySqlDataAdapter(selectCommand).Fill(dataTable);
                    this.CloseConnection();
                }
            }
            return dataTable;
        }

        public DataTable getClaimDoctors(int claim_id)
        {
            DataTable dataTable = new DataTable();
            string cmdText = "SELECT practice_number FROM doctors WHERE claim_id=@id";
            using (this.connection)
            {
                if (this.OpenConnection())
                {
                    MySqlCommand selectCommand = new MySqlCommand(cmdText, this.connection);
                    selectCommand.Parameters.AddWithValue("@id", (object)claim_id);
                    new MySqlDataAdapter(selectCommand).Fill(dataTable);
                    this.CloseConnection();
                }
            }
            return dataTable;
        }

        public void hhxx()
        {
            int num = (int)MessageBox.Show(DateTime.Today.ToString("yyyy-MM-dd"));
        }

        public void insert10(int id)
        {
            string cmdText = "INSERT INTO 10daysclaims(claim_id) VALUES(@claim)";
            using (this.connection)
            {
                if (!this.OpenConnection())
                    return;
                MySqlCommand mySqlCommand = new MySqlCommand(cmdText, this.connection);
                mySqlCommand.Parameters.AddWithValue("@claim", (object)id);
                mySqlCommand.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        public DataSet OpenCases()
        {
            DataSet dataSet = new DataSet();
            string cmdText = "SELECT username, COUNT(Open) as open FROM `claim` WHERE Open=1 GROUP BY username";
            using (this.connection)
            {
                if (this.OpenConnection())
                {
                    new MySqlDataAdapter(new MySqlCommand(cmdText, this.connection)).Fill(dataSet);
                    this.CloseConnection();
                }
            }
            return dataSet;
        }

        public DataSet checkDoctor(string code)
        {
            DataSet dataSet = new DataSet();
            string cmdText = "SELECT rsoncode FROM `person` WHERE rsoncode=@code";
            using (this.connection2)
            {
                if (this.OpenConnection2())
                {
                    MySqlCommand selectCommand = new MySqlCommand(cmdText, this.connection2);
                    selectCommand.Parameters.AddWithValue("@code", (object)code);
                    new MySqlDataAdapter(selectCommand).Fill(dataSet);
                    this.CloseConnection();
                }
            }
            return dataSet;
        }

        public DataSet checkDoctor1(string code)
        {
            DataSet dataSet = new DataSet();
            string cmdText = "SELECT orgcode FROM `organisation` WHERE orgcode=@code";
            using (this.connection2)
            {
                if (this.OpenConnection2())
                {
                    MySqlCommand selectCommand = new MySqlCommand(cmdText, this.connection2);
                    selectCommand.Parameters.AddWithValue("@code", (object)code);
                    new MySqlDataAdapter(selectCommand).Fill(dataSet);
                    this.CloseConnection();
                }
            }
            return dataSet;
        }

        public void InsertDoctors(
          int rsoncode,
          string persontypename,
          string title,
          string initials,
          string firstname,
          string lastname,
          string gender,
          string language,
          string idnumber,
          string dob,
          string position,
          string dispense,
          string privacy,
          string councilnumber1,
          string addresscode,
          string lastupdate,
          string councilnumber2,
          string councilnumber3,
          string practicetypename,
          string personstatusname,
          string recordstatusname,
          string physad1,
          string physad2,
          string physsuburb,
          string physcode,
          string postalad1,
          string postalad2,
          string postalsuburb,
          string postalcode,
          string tel1code,
          string tel1,
          string tel2code,
          string tel2,
          string tel3code,
          string tel3,
          string tel4code,
          string tel4,
          string em1code,
          string em1,
          string em2code,
          string em2,
          string fax1code,
          string fax1,
          string fax2code,
          string fax2,
          string email,
          string url,
          string subregionname,
          string regionname,
          string countrycode,
          string country_description,
          string service,
          string practiceno,
          string disciplinecode,
          string subdisciplinecode,
          string reportingblockcode,
          string reportingblock,
          string bricknumber,
          string brickdescription,
          string bi_suburb,
          string town,
          string lat,
          string longx,
          string updatetype,
          string gives,
          int chck,
          string orgcode = "",
          string orgname = "",
          string orgtypename = "",
          string mainorg = "",
          string orgstatusname = "",
          string recordstatus = "")
        {
            try
            {
                string cmdText = "INSERT INTO person(rsoncode,persontypename,title,initials,firstname,lastname,gender,language,idnumber,dob,position,dispense,privacy,councilnumber1,addresscode,lastupdate,councilnumber2,councilnumber3,practicetypename,personstatusname,recordstatusname,physad1,physad2,physsuburb,physcode,postalad1,postalad2,postalsuburb,postalcode,tel1code,tel1,tel2code,tel2,tel3code,tel3,tel4code,tel4,em1code,em1,em2code,em2,fax1code,fax1,fax2code,fax2,email,url,subregionname,regionname,countrycode,country_description,service,practiceno,disciplinecode,subdisciplinecode,reportingblockcode,reportingblock,bricknumber,brickdescription,bi_suburb,town,lat,lon,updatetype,gives_discount) VALUES(@rsoncode,@persontypename,@title,@initials,@firstname,@lastname,@gender,@language,@idnumber,@dob,@position,@dispense,@privacy,@councilnumber1,@addresscode,@lastupdate,@councilnumber2,@councilnumber3,@practicetypename,@personstatusname,@recordstatusname,@physad1,@physad2,@physsuburb,@physcode,@postalad1,@postalad2,@postalsuburb,@postalcode,@tel1code,@tel1,@tel2code,@tel2,@tel3code,@tel3,@tel4code,@tel4,@em1code,@em1,@em2code,@em2,@fax1code,@fax1,@fax2code,@fax2,@email,@url,@subregionname,@regionname,@countrycode,@country_description,@service,@practiceno,@disciplinecode,@subdisciplinecode,@reportingblockcode,@reportingblock,@bricknumber,@brickdescription,@bi_suburb,@town,@lat,@long,@updatetype,@gives)";
                switch (chck)
                {
                    case 2:
                        cmdText = "UPDATE person SET rsoncode=@rsoncode,persontypename=@persontypename,title=@title,initials=@initials,firstname=@firstname,lastname=@lastname,gender=@gender,language=@language,idnumber=@idnumber,dob=@dob,position=@position,dispense=@dispense,privacy=@privacy,councilnumber1=@councilnumber1,addresscode=@addresscode,lastupdate=@lastupdate,councilnumber2=@councilnumber2,councilnumber3=@councilnumber3,practicetypename=@practicetypename,personstatusname=@personstatusname,recordstatusname=@recordstatusname,physad1=@physad1,physad2=@physad2,physsuburb=@physsuburb,physcode=@physcode,postalad1=@postalad1,postalad2=@postalad2,postalsuburb=@postalsuburb,postalcode=@postalcode,tel1code=@tel1code,tel1=@tel1,tel2code=@tel2code,tel2=@tel2,tel3code=@tel3code,tel3=@tel3,tel4code=@tel4code,tel4=@tel4,em1code=@em1code,em1=@em1,em2code=@em2code,em2=@em2,fax1code=@fax1code,fax1=@fax1,fax2code=@fax2code,fax2=@fax2,email=@email,url=@url,subregionname=@subregionname,regionname=@regionname,countrycode=@countrycode,country_description=@country_description,service=@service,practiceno=@practiceno,disciplinecode=@disciplinecode,subdisciplinecode=@subdisciplinecode,reportingblockcode=@reportingblockcode,reportingblock=@reportingblock,bricknumber=@bricknumber,brickdescription=@brickdescription,bi_suburb=@bi_suburb,town=@town,lat=@lat,lon=@long,updatetype=@updatetype,gives_discount=@gives WHERE rsoncode=@rsoncode";
                        break;
                    case 3:
                        cmdText = "INSERT INTO organisation(orgcode, orgname, orgtypename, dispense, privacy, mainorg, addresscode, orgstatusname, recordstatus, lastupdate, councilnumber1, councilnumber2, physad1, physad2, physsuburb, physcode, postalad1, postalad2, postalsuburb, postalcode, tel1code, tel1, tel2code, tel2, tel3code, tel3, tel4code, tel4, em1code, em1, em2code, em2, fax1code, fax1, fax2code, fax2, email, url, subregionname, regionname, countrycode, country_description, service, disciplinecode, subdisciplinecode, practiceno, reportingblockcode, reportingblock, bricknumber, brickdescription, bi_suburb, town, lat, lon, updatetype, gives_discount) VALUES(orgcode,@orgname,@orgtypename,@dispense,@privacy,@mainorg,@addresscode,@orgstatusname,@recordstatus,@lastupdate,@councilnumber1,@councilnumber2,@physad1,@physad2,@physsuburb,@physcode,@postalad1,@postalad2,@postalsuburb,@postalcode,@tel1code,@tel1,@tel2code,@tel2,@tel3code,@tel3,@tel4code,@tel4,@em1code,@em1,@em2code,@em2,@fax1code,@fax1,@fax2code,@fax2,@email,@url,@subregionname,@regionname,@countrycode,@country_description,@service,@disciplinecode,@subdisciplinecode,@practiceno,@reportingblockcode,@reportingblock,@bricknumber,@brickdescription,@bi_suburb,@town,@lat,@long,@updatetype,@gives)";
                        break;
                    case 4:
                        cmdText = "UPDATE organisation SET orgcode=@orgcode,orgtypename=@orgtypename,orgname=@orgname,orgstatusname=@orgstatusname,mainorg=@mainorg,dispense=@dispense,privacy=@privacy,councilnumber1=@councilnumber1,addresscode=@addresscode,lastupdate=@lastupdate,councilnumber2=@councilnumber2,physad1=@physad1,physad2=@physad2,physsuburb=@physsuburb,physcode=@physcode,postalad1=@postalad1,postalad2=@postalad2,postalsuburb=@postalsuburb,postalcode=@postalcode,tel1code=@tel1code,tel1=@tel1,tel2code=@tel2code,tel2=@tel2,tel3code=@tel3code,tel3=@tel3,tel4code=@tel4code,tel4=@tel4,em1code=@em1code,em1=@em1,em2code=@em2code,em2=@em2,fax1code=@fax1code,fax1=@fax1,fax2code=@fax2code,fax2=@fax2,email=@email,url=@url,subregionname=@subregionname,regionname=@regionname,countrycode=@countrycode,country_description=@country_description,service=@service,practiceno=@practiceno,disciplinecode=@disciplinecode,subdisciplinecode=@subdisciplinecode,reportingblockcode=@reportingblockcode,reportingblock=@reportingblock,bricknumber=@bricknumber,brickdescription=@brickdescription,bi_suburb=@bi_suburb,town=@town,lat=@lat,lon=@long,updatetype=@updatetype,gives_discount=@gives WHERE orgcode=@orgcode";
                        break;
                }
                using (this.connection2)
                {
                    if (!this.OpenConnection2())
                        return;
                    MySqlCommand mySqlCommand = new MySqlCommand(cmdText, this.connection2);
                    mySqlCommand.Parameters.AddWithValue("@rsoncode", (object)rsoncode);
                    mySqlCommand.Parameters.AddWithValue("@persontypename", (object)persontypename);
                    mySqlCommand.Parameters.AddWithValue("@title", (object)title);
                    mySqlCommand.Parameters.AddWithValue("@initials", (object)initials);
                    mySqlCommand.Parameters.AddWithValue("@firstname", (object)firstname);
                    mySqlCommand.Parameters.AddWithValue("@lastname", (object)lastname);
                    mySqlCommand.Parameters.AddWithValue("@gender", (object)gender);
                    mySqlCommand.Parameters.AddWithValue("@language", (object)language);
                    mySqlCommand.Parameters.AddWithValue("@idnumber", (object)idnumber);
                    mySqlCommand.Parameters.AddWithValue("@dob", (object)dob);
                    mySqlCommand.Parameters.AddWithValue("@position", (object)position);
                    mySqlCommand.Parameters.AddWithValue("@dispense", (object)dispense);
                    mySqlCommand.Parameters.AddWithValue("@privacy", (object)privacy);
                    mySqlCommand.Parameters.AddWithValue("@councilnumber1", (object)councilnumber1);
                    mySqlCommand.Parameters.AddWithValue("@addresscode", (object)addresscode);
                    mySqlCommand.Parameters.AddWithValue("@lastupdate", (object)lastupdate);
                    mySqlCommand.Parameters.AddWithValue("@councilnumber2", (object)councilnumber2);
                    mySqlCommand.Parameters.AddWithValue("@councilnumber3", (object)councilnumber3);
                    mySqlCommand.Parameters.AddWithValue("@practicetypename", (object)practicetypename);
                    mySqlCommand.Parameters.AddWithValue("@personstatusname", (object)personstatusname);
                    mySqlCommand.Parameters.AddWithValue("@recordstatusname", (object)recordstatusname);
                    mySqlCommand.Parameters.AddWithValue("@physad1", (object)physad1);
                    mySqlCommand.Parameters.AddWithValue("@physad2", (object)physad2);
                    mySqlCommand.Parameters.AddWithValue("@physsuburb", (object)physsuburb);
                    mySqlCommand.Parameters.AddWithValue("@physcode", (object)physcode);
                    mySqlCommand.Parameters.AddWithValue("@postalad1", (object)postalad1);
                    mySqlCommand.Parameters.AddWithValue("@postalad2", (object)postalad2);
                    mySqlCommand.Parameters.AddWithValue("@postalsuburb", (object)postalsuburb);
                    mySqlCommand.Parameters.AddWithValue("@postalcode", (object)postalcode);
                    mySqlCommand.Parameters.AddWithValue("@tel1code", (object)tel1code);
                    mySqlCommand.Parameters.AddWithValue("@tel1", (object)tel1);
                    mySqlCommand.Parameters.AddWithValue("@tel2code", (object)tel2code);
                    mySqlCommand.Parameters.AddWithValue("@tel2", (object)tel2);
                    mySqlCommand.Parameters.AddWithValue("@tel3", (object)tel3);
                    mySqlCommand.Parameters.AddWithValue("@tel3code", (object)tel3code);
                    mySqlCommand.Parameters.AddWithValue("@tel4code", (object)tel4code);
                    mySqlCommand.Parameters.AddWithValue("@em1code", (object)em1code);
                    mySqlCommand.Parameters.AddWithValue("@tel4", (object)tel4);
                    mySqlCommand.Parameters.AddWithValue("@em1", (object)em1);
                    mySqlCommand.Parameters.AddWithValue("@em2code", (object)em2code);
                    mySqlCommand.Parameters.AddWithValue("@em2", (object)em2);
                    mySqlCommand.Parameters.AddWithValue("@fax1", (object)fax1);
                    mySqlCommand.Parameters.AddWithValue("@fax2", (object)fax2);
                    mySqlCommand.Parameters.AddWithValue("@fax2code", (object)fax2code);
                    mySqlCommand.Parameters.AddWithValue("@fax1code", (object)fax1code);
                    mySqlCommand.Parameters.AddWithValue("@email", (object)email);
                    mySqlCommand.Parameters.AddWithValue("@url", (object)url);
                    mySqlCommand.Parameters.AddWithValue("@subregionname", (object)subregionname);
                    mySqlCommand.Parameters.AddWithValue("@regionname", (object)regionname);
                    mySqlCommand.Parameters.AddWithValue("@countrycode", (object)countrycode);
                    mySqlCommand.Parameters.AddWithValue("@country_description", (object)country_description);
                    mySqlCommand.Parameters.AddWithValue("@service", (object)service);
                    mySqlCommand.Parameters.AddWithValue("@practiceno", (object)practiceno);
                    mySqlCommand.Parameters.AddWithValue("@disciplinecode", (object)disciplinecode);
                    mySqlCommand.Parameters.AddWithValue("@subdisciplinecode", (object)subdisciplinecode);
                    mySqlCommand.Parameters.AddWithValue("@reportingblockcode", (object)reportingblockcode);
                    mySqlCommand.Parameters.AddWithValue("@reportingblock", (object)reportingblock);
                    mySqlCommand.Parameters.AddWithValue("@bricknumber", (object)bricknumber);
                    mySqlCommand.Parameters.AddWithValue("@brickdescription", (object)brickdescription);
                    mySqlCommand.Parameters.AddWithValue("@bi_suburb", (object)bi_suburb);
                    mySqlCommand.Parameters.AddWithValue("@town", (object)town);
                    mySqlCommand.Parameters.AddWithValue("@lat", (object)lat);
                    mySqlCommand.Parameters.AddWithValue("@long", (object)longx);
                    mySqlCommand.Parameters.AddWithValue("@updatetype", (object)updatetype);
                    mySqlCommand.Parameters.AddWithValue("@gives", (object)gives);
                    mySqlCommand.Parameters.AddWithValue("@orgcode", (object)orgcode);
                    mySqlCommand.Parameters.AddWithValue("@orgname", (object)orgname);
                    mySqlCommand.Parameters.AddWithValue("@orgtypename", (object)orgtypename);
                    mySqlCommand.Parameters.AddWithValue("@mainorg", (object)mainorg);
                    mySqlCommand.Parameters.AddWithValue("@orgstatusname", (object)orgstatusname);
                    mySqlCommand.Parameters.AddWithValue("@recordstatus", (object)recordstatus);
                    mySqlCommand.ExecuteNonQuery();
                    this.CloseConnection2();
                }
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message);
            }
        }

        public DataTable getDocumentstodelete()
        {
            DataTable dataTable = new DataTable();
            string cmdText = "SELECT a.doc_id,a.claim_id,a.date,a.randomNum,a.doc_description,d.client_name,c.first_name,b.username FROM documents as a INNER JOIN claim as b ON a.claim_id=b.claim_id INNER JOIN member as c ON b.member_id=c.member_id INNER JOIN clients as d ON c.client_id=d.client_id WHERE Open=0 AND b.date_closed<@dat AND deleted is null ORDER BY a.doc_id DESC";
            using (this.connection)
            {
                if (this.OpenConnection())
                {
                    string str = DateTime.Today.AddMonths(-1).ToString("yyyy-MM-dd");
                    MySqlCommand selectCommand = new MySqlCommand(cmdText, this.connection);
                    selectCommand.Parameters.AddWithValue("@dat", (object)str);
                    new MySqlDataAdapter(selectCommand).Fill(dataTable);
                    this.CloseConnection();
                }
            }
            return dataTable;
        }

        public void updateDocument(int doc_id, string h = "")
        {
            string cmdText = "UPDATE documents SET deleted=1 WHERE doc_id=@doc_id";
            using (this.connection)
            {
                if (!this.OpenConnection())
                    return;
                MySqlCommand mySqlCommand = new MySqlCommand(cmdText, this.connection);
                mySqlCommand.Parameters.AddWithValue("@doc_id", (object)doc_id);
                mySqlCommand.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        public DataSet checkDoctor_Medp(string practice_number)
        {
            DataSet dataSet = new DataSet();
            string cmdText = "SELECT practice_number FROM `doctor_details` WHERE practice_number=@practiceno";
            using (this.connection)
            {
                if (this.OpenConnection())
                {
                    MySqlCommand selectCommand = new MySqlCommand(cmdText, this.connection);
                    selectCommand.Parameters.AddWithValue("@practiceno", (object)practice_number);
                    new MySqlDataAdapter(selectCommand).Fill(dataSet);
                    this.CloseConnection();
                }
            }
            return dataSet;
        }

        public DataSet check_localDoctor(int claim_id, string practice_number)
        {
            DataSet dataSet = new DataSet();
            string cmdText = "SELECT practice_number FROM doctors WHERE practice_number=@practice_number AND claim_id=@claim_id";
            using (this.connection)
            {
                if (this.OpenConnection())
                {
                    MySqlCommand selectCommand = new MySqlCommand(cmdText, this.connection);
                    selectCommand.Parameters.AddWithValue("@practice_number", (object)practice_number);
                    selectCommand.Parameters.AddWithValue("@claim_id", (object)claim_id);
                    new MySqlDataAdapter(selectCommand).Fill(dataSet);
                    this.CloseConnection();
                }
            }
            return dataSet;
        }

        public void insertDoctorMedpages(string fullname, string practicenumber)
        {
            string cmdText = "INSERT INTO doctor_details(name_initials,practice_number) VALUES (@firstname,@practiceno)";
            using (this.connection)
            {
                if (!this.OpenConnection())
                    return;
                MySqlCommand mySqlCommand = new MySqlCommand(cmdText, this.connection);
                mySqlCommand.Parameters.AddWithValue("@firstname", (object)fullname);
                mySqlCommand.Parameters.AddWithValue("@practiceno", (object)practicenumber);
                mySqlCommand.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        public void insertDoctorLocal(
          int claim_id,
          string practice_number,
          string doc_name,
          double charged = 0.0,
          double scheme = 0.0,
          double gap = 0.0,
          string treating_drnumber="", 
          string treatingdr_name="",
          string provider_invoicenumber="")
        {
            string cmdText = "INSERT INTO doctors(claim_id,practice_number,doc_name,doc_charged_amount,doc_gap,doc_scheme_amount,treating_drnumber,treatingdr_name,provider_invoicenumber) VALUES (@claim_id,@practice_number,@doc_name,@doc_charged_amount,@doc_gap,@doc_scheme_amount,@treating_drnumber,@treatingdr_name,@provider_invoicenumber)";
            using (this.connection)
            {
                if (!this.OpenConnection())
                    return;
                MySqlCommand mySqlCommand = new MySqlCommand(cmdText, this.connection);
                mySqlCommand.Parameters.AddWithValue("@claim_id", (object)claim_id);
                mySqlCommand.Parameters.AddWithValue("@practice_number", (object)practice_number);
                mySqlCommand.Parameters.AddWithValue("@doc_name", (object)doc_name);
                mySqlCommand.Parameters.AddWithValue("@doc_charged_amount", (object)charged);
                mySqlCommand.Parameters.AddWithValue("@doc_gap", (object)gap);
                mySqlCommand.Parameters.AddWithValue("@doc_scheme_amount", (object)scheme);
                mySqlCommand.Parameters.AddWithValue("@treating_drnumber", (object)treating_drnumber);
                mySqlCommand.Parameters.AddWithValue("@treatingdr_name", (object)treatingdr_name);
                mySqlCommand.Parameters.AddWithValue("@provider_invoicenumber", (object)provider_invoicenumber);
                mySqlCommand.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        public DataSet checkClaimline(
          int claim_id,
          string practice_number,
          string treatmentdate,
          string treatmentcode)
        {
            DataSet dataSet = new DataSet();
            string cmdText = "SELECT id FROM `claim_line` WHERE practice_number=@practice_number AND mca_claim_id=@claim_id AND treatmentDate=@dat AND tariff_code=@code";
            using (this.connection)
            {
                if (this.OpenConnection())
                {
                    MySqlCommand selectCommand = new MySqlCommand(cmdText, this.connection);
                    selectCommand.Parameters.AddWithValue("@practice_number", (object)practice_number);
                    selectCommand.Parameters.AddWithValue("@claim_id", (object)claim_id);
                    selectCommand.Parameters.AddWithValue("@dat", (object)treatmentdate);
                    selectCommand.Parameters.AddWithValue("@code", (object)treatmentcode);
                    new MySqlDataAdapter(selectCommand).Fill(dataSet);
                    this.CloseConnection();
                }
            }
            return dataSet;
        }

        public DataSet checkClaimlineAdmed(
          int claim_id,
          string practice_number,
          string treatmentdate,
          string treatmentcode,
          double clmnline_charged_amnt,
          double clmline_scheme_paid_amnt,
          double gap)
        {
            treatmentdate = treatmentdate.Replace("00:00:00", "");
            treatmentdate = treatmentdate.Replace(" ", "");
            treatmentdate = "%" + treatmentdate + "%";
            DataSet dataSet = new DataSet();
            string cmdText = "SELECT id,modifier,modifier_name FROM `claim_line` WHERE practice_number=@practice_number AND mca_claim_id=@claim_id AND treatmentDate like @dat AND tariff_code=@code AND clmnline_charged_amnt=@clmnline_charged_amnt AND clmline_scheme_paid_amnt=@clmline_scheme_paid_amnt AND gap=@gap";
            using (this.connection)
            {
                if (this.OpenConnection())
                {
                    MySqlCommand selectCommand = new MySqlCommand(cmdText, this.connection);
                    selectCommand.Parameters.AddWithValue("@practice_number", (object)practice_number);
                    selectCommand.Parameters.AddWithValue("@claim_id", (object)claim_id);
                    selectCommand.Parameters.AddWithValue("@dat", (object)treatmentdate);
                    selectCommand.Parameters.AddWithValue("@code", (object)treatmentcode);
                    selectCommand.Parameters.AddWithValue("@clmnline_charged_amnt", (object)clmnline_charged_amnt);
                    selectCommand.Parameters.AddWithValue("@clmline_scheme_paid_amnt", (object)clmline_scheme_paid_amnt);
                    selectCommand.Parameters.AddWithValue("@gap", (object)gap);
                    new MySqlDataAdapter(selectCommand).Fill(dataSet);
                    this.CloseConnection();
                }
            }
            return dataSet;
        }

        public void insertClaimline(
            int claim_id,
            string practice_number, 
            string treatmentdate,
            string treatmentcode,
            string treatment_descr,         
            double charged_amt,
          double scheme_paid,
          string icd10,
          string icd10_descr,
          double gap = 0.0,
          string modifier = "",
          string modifier_name = "",
          string modifier_charged = "0",
          string modifier_claimable = "",
          string msg_code = "",
          string nappi_code = "",
          string treatment_code_type = "",
          string secontsry_icd10 = "",
          string reason_code = "",
          string reason_description = "",
          string event_date_from="",
          string event_date_to="",
          string inhospital="",
          string jv_claim_line_number=""
          )
        {
            treatmentdate = treatmentdate.Replace("00:00:00", "");
            treatmentdate = treatmentdate.Replace(" ", "");
            string cmdText = "INSERT INTO claim_line(practice_number,mca_claim_id,treatmentDate,tariff_code,msg_dscr,clmnline_charged_amnt,clmline_scheme_paid_amnt,primaryICDCode,primaryICDDescr,gap,modifier, modifier_name,modifier_charged, modifier_claimable,gap_aamount_line,msg_code,nappi_code,treatmentType,secondaryICDCode,reason_code,reason_description,event_date_from,event_date_to,inhospital,jv_claim_line_number) VALUES (@practice_number,@mca_claim_id,@treatmentDate,@tariff_code,@msg_dscr,@clmnline_charged_amnt,@clmline_scheme_paid_amnt,@primaryICDCode,@primaryICDDescr,@gap,@modifier, @modifier_name,@modifier_charged, @modifier_claimable,@gap_aamount_line,@msg_code,@nappi_code,@treatmentType,@secondaryICDCode,@reason_code,@reason_description,@event_date_from,@event_date_to,@inhospital,@jv_claim_line_number)";
            using (this.connection)
            {
                if (!this.OpenConnection())
                    return;
                MySqlCommand mySqlCommand = new MySqlCommand(cmdText, this.connection);
                mySqlCommand.Parameters.AddWithValue("@practice_number", (object)practice_number);
                mySqlCommand.Parameters.AddWithValue("@mca_claim_id", (object)claim_id);
                mySqlCommand.Parameters.AddWithValue("@treatmentDate", (object)treatmentdate);
                mySqlCommand.Parameters.AddWithValue("@tariff_code", (object)treatmentcode);
                mySqlCommand.Parameters.AddWithValue("@msg_dscr", (object)treatment_descr);
                mySqlCommand.Parameters.AddWithValue("@clmnline_charged_amnt", (object)charged_amt);
                mySqlCommand.Parameters.AddWithValue("@clmline_scheme_paid_amnt", (object)scheme_paid);
                mySqlCommand.Parameters.AddWithValue("@primaryICDCode", (object)icd10);
                mySqlCommand.Parameters.AddWithValue("@primaryICDDescr", (object)icd10_descr);
                mySqlCommand.Parameters.AddWithValue("@gap", (object)gap);
                mySqlCommand.Parameters.AddWithValue("@modifier", (object)modifier);
                mySqlCommand.Parameters.AddWithValue("@modifier_name", (object)modifier_name);
                mySqlCommand.Parameters.AddWithValue("@modifier_charged", (object)modifier_charged);
                mySqlCommand.Parameters.AddWithValue("@modifier_claimable", (object)modifier_claimable);
                mySqlCommand.Parameters.AddWithValue("@gap_aamount_line", (object)gap);
                mySqlCommand.Parameters.AddWithValue("@msg_code", (object)msg_code);
                mySqlCommand.Parameters.AddWithValue("@nappi_code", (object)nappi_code);
                mySqlCommand.Parameters.AddWithValue("@treatmentType", (object)treatment_code_type);                
                mySqlCommand.Parameters.AddWithValue("@secondaryICDCode", (object)secontsry_icd10);
                mySqlCommand.Parameters.AddWithValue("@reason_code", (object)reason_code);
                mySqlCommand.Parameters.AddWithValue("@reason_description", (object)reason_description);
                mySqlCommand.Parameters.AddWithValue("@event_date_from", (object)event_date_from);
                mySqlCommand.Parameters.AddWithValue("@event_date_to", (object)event_date_to);
                mySqlCommand.Parameters.AddWithValue("@inhospital", (object)inhospital);
                mySqlCommand.Parameters.AddWithValue("@jv_claim_line_number", (object)jv_claim_line_number);
                mySqlCommand.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        public DataSet checkD(string practice_number, int claim_id)
        {
            DataSet dataSet = new DataSet();
            string cmdText = "SELECT doc_name,savings_scheme,savings_discount,doc_charged_amount,doc_scheme_amount,doc_gap FROM `doctors` WHERE practice_number=@practice_number AND claim_id=@claim";
            using (this.connection)
            {
                if (this.OpenConnection())
                {
                    MySqlCommand selectCommand = new MySqlCommand(cmdText, this.connection);
                    selectCommand.Parameters.AddWithValue("@practice_number", (object)practice_number);
                    selectCommand.Parameters.AddWithValue("@claim", (object)claim_id);
                    new MySqlDataAdapter(selectCommand).Fill(dataSet);
                    this.CloseConnection();
                }
            }
            return dataSet;
        }

        public DataSet doctoNote(string practice_number, int claim_id)
        {
            DataSet dataSet = new DataSet();
            string cmdText = "SELECT intervention_desc FROM `intervention` WHERE practice_number=@practice_number AND claim_id=@claim ORDER BY intervention_id DESC LIMIT 1";
            using (this.connection)
            {
                if (this.OpenConnection())
                {
                    MySqlCommand selectCommand = new MySqlCommand(cmdText, this.connection);
                    selectCommand.Parameters.AddWithValue("@practice_number", (object)practice_number);
                    selectCommand.Parameters.AddWithValue("@claim", (object)claim_id);
                    new MySqlDataAdapter(selectCommand).Fill(dataSet);
                    this.CloseConnection();
                }
            }
            return dataSet;
        }

        public DataSet zestFeedbak()
        {
            DataSet dataSet = new DataSet();
            string cmdText = "SELECT mca_claim_id,practice_number,tariff_code,msg_dscr,primaryICDCode,primaryICDDescr,treatmentDate,clmnline_charged_amnt,clmline_scheme_paid_amnt,a.date_entered,b.claim_number,c.policy_number,c.first_name,c.surname,c.cell,c.telephone,c.email,c.medical_scheme,b.charged_amnt,b.scheme_paid,b.gap,b.icd10,b.icd10_desc,b.savings_scheme,b.savings_discount,c.productName,c.product_code,c.beneficiary_number,a.id,b.Open FROM `claim_line` as a inner join claim as b on a.mca_claim_id=b.claim_id inner join member as c on b.member_id=c.member_id WHERE c.client_id=1 AND a.confirm_report is null AND b.claim_number like @gg";
            using (this.connection)
            {
                if (this.OpenConnection())
                {
                    MySqlCommand selectCommand = new MySqlCommand(cmdText, this.connection);
                    selectCommand.Parameters.AddWithValue("@gg", (object)"GAP%");
                    new MySqlDataAdapter(selectCommand).Fill(dataSet);
                    this.CloseConnection();
                }
            }
            return dataSet;
        }

        public void updateZestlife(int i, int id)
        {
            try
            {
                string cmdText = "UPDATE claim_line SET confirm_report=@i WHERE id=@id";
                using (this.connection)
                {
                    if (!this.OpenConnection())
                        return;
                    MySqlCommand mySqlCommand = new MySqlCommand(cmdText, this.connection);
                    mySqlCommand.Parameters.AddWithValue("@i", (object)i);
                    mySqlCommand.Parameters.AddWithValue("@id", (object)id);
                    Console.WriteLine("tiripane===" + mySqlCommand.ExecuteNonQuery().ToString());
                    this.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Paresveke +ex." + ex.Message);
            }
        }

        public void insertZestlifenotes(int claimline_id, string note)
        {
            string cmdText = "INSERT INTO zestlifenotes(claimline_id,note) VALUES(@claimline_id,@note)";
            using (this.connection)
            {
                if (!this.OpenConnection())
                    return;
                MySqlCommand mySqlCommand = new MySqlCommand(cmdText, this.connection);
                mySqlCommand.Parameters.AddWithValue("@claimline_id", (object)claimline_id);
                mySqlCommand.Parameters.AddWithValue("@note", (object)note);
                mySqlCommand.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        public DataSet selectZestlifenotes(int claimline_id, string note)
        {
            DataSet dataSet = new DataSet();
            string cmdText = "SELECT \tclaimline_id FROM zestlifenotes WHERE claimline_id=@id AND note=@note ORDER BY claimline_id DESC LIMIT 1";
            using (this.connection)
            {
                if (this.OpenConnection())
                {
                    MySqlCommand selectCommand = new MySqlCommand(cmdText, this.connection);
                    selectCommand.Parameters.AddWithValue("@id", (object)claimline_id);
                    selectCommand.Parameters.AddWithValue("@note", (object)note);
                    new MySqlDataAdapter(selectCommand).Fill(dataSet);
                    this.CloseConnection();
                }
            }
            return dataSet;
        }

        public void openClaim(int claim_id,int open, string date_reopened = "",DateTime? start_date = null, string end_date = "")
        {
            try
            {
                string cmdText = "UPDATE claim SET Open=@open,date_reopened=@date_reopened,Service_Date=@start_date,end_date=@end_date WHERE claim_id=@id";
                using (this.connection)
                {
                    if (this.OpenConnection() == true)
                    {
                        MySqlCommand mySqlCommand = new MySqlCommand(cmdText, this.connection);
                        mySqlCommand.Parameters.AddWithValue("@id", (object)claim_id);
                        mySqlCommand.Parameters.AddWithValue("@open", (object)open);
                        mySqlCommand.Parameters.AddWithValue("@date_reopened", (object)date_reopened);
                        mySqlCommand.Parameters.AddWithValue("@start_date", (object)start_date);
                        mySqlCommand.Parameters.AddWithValue("@end_date", (object)end_date);
                        Console.WriteLine("Update Open Status : " + mySqlCommand.ExecuteNonQuery().ToString());
                        this.CloseConnection();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Updating Open status "+ex.Message);
            }
        }

        public DataSet loadedClaims(int client_id, string dat)
        {
            DataSet dataSet = new DataSet();
            string cmdText = "SELECT  FROM claim as a inner join member as c on a.member_id=b.member_id WHERE b.client_id=@cid AND a.date_entered like @dat";
            using (this.connection)
            {
                if (this.OpenConnection())
                {
                    MySqlCommand selectCommand = new MySqlCommand(cmdText, this.connection);
                    selectCommand.Parameters.AddWithValue("@cid", (object)client_id.ToString());
                    selectCommand.Parameters.AddWithValue("@dat", (object)(dat + "%"));
                    new MySqlDataAdapter(selectCommand).Fill(dataSet);
                    this.CloseConnection();
                }
            }
            return dataSet;
        }

        public DataSet checkZestAmount(int claim_id, string practice_number)
        {
            DataSet dataSet = new DataSet();
            string cmdText = "SELECT SUM(total) as tot FROM (SELECT gap_aamount_line as total  FROM `claim_line` WHERE `mca_claim_id` = @claim_id AND practice_number=@practice_number GROUP BY gap_aamount_line) as a";
            using (this.connection)
            {
                if (this.OpenConnection())
                {
                    MySqlCommand selectCommand = new MySqlCommand(cmdText, this.connection);
                    selectCommand.Parameters.AddWithValue("@claim_id", (object)claim_id);
                    selectCommand.Parameters.AddWithValue("@practice_number", (object)practice_number);
                    new MySqlDataAdapter(selectCommand).Fill(dataSet);
                    this.CloseConnection();
                }
            }
            return dataSet;
        }

        public void updateDoctorZestlife(int claim_id, string practice_number)
        {
            try
            {
                double num = double.Parse(this.checkZestAmount(claim_id, practice_number).Tables[0].Rows[0]["tot"].ToString());
                string cmdText = "UPDATE doctors SET doc_gap=@doc_gap WHERE claim_id=@id AND practice_number=@practice_number";
                using (this.connection)
                {
                    if (!this.OpenConnection())
                        return;
                    MySqlCommand mySqlCommand = new MySqlCommand(cmdText, this.connection);
                    mySqlCommand.Parameters.AddWithValue("@id", (object)claim_id);
                    mySqlCommand.Parameters.AddWithValue("@practice_number", (object)practice_number);
                    mySqlCommand.Parameters.AddWithValue("@doc_gap", (object)num);
                    Console.WriteLine("Dzive " + mySqlCommand.ExecuteNonQuery().ToString());
                    this.CloseConnection();
                }
            }
            catch (Exception ex)
            {
            }
        }

        public DataSet getEmailMembers(string subject)
        {
            DataSet dataSet = new DataSet();
            string cmdText = "SELECT DISTINCT email FROM member where email NOT IN(SELECT email FROM bulk_emails) AND LENGTH(email)>4 LIMIT 5";
            using (this.connection)
            {
                if (this.OpenConnection())
                {
                    new MySqlDataAdapter(new MySqlCommand(cmdText, this.connection)).Fill(dataSet);
                    this.CloseConnection();
                }
            }
            return dataSet;
        }

        public DataSet getEmailMember1(string email)
        {
            DataSet dataSet = new DataSet();
            string cmdText = "SELECT DISTINCT email FROM bulk_emails WHERE email=@email";
            using (this.connection)
            {
                if (this.OpenConnection())
                {
                    MySqlCommand selectCommand = new MySqlCommand(cmdText, this.connection);
                    selectCommand.Parameters.AddWithValue("@email", (object)email);
                    new MySqlDataAdapter(selectCommand).Fill(dataSet);
                    this.CloseConnection();
                }
            }
            return dataSet;
        }

        public void insertmails(
          string email,
          string subject,
          string body,
          string entered_by,
          string status)
        {
            try
            {
                string cmdText = "INSERT INTO bulk_emails(email,subject,body,entered_by,status) VALUES(@email,@subject,@body,@entered_by,@status)";
                using (this.connection)
                {
                    if (!this.OpenConnection())
                        return;
                    MySqlCommand mySqlCommand = new MySqlCommand(cmdText, this.connection);
                    mySqlCommand.Parameters.AddWithValue("@email", (object)email);
                    mySqlCommand.Parameters.AddWithValue("@subject", (object)subject);
                    mySqlCommand.Parameters.AddWithValue("@body", (object)body);
                    mySqlCommand.Parameters.AddWithValue("@entered_by", (object)entered_by);
                    mySqlCommand.Parameters.AddWithValue("@status", (object)status);
                    mySqlCommand.ExecuteNonQuery();
                    this.CloseConnection();
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void insertReply(
          string email_to,
          string email_from,
          string subject,
          string body,
          string email_source,
          int claim_id,
          int status = 2)
        {
            try
            {
                string cmdText = "INSERT INTO emails(email_to,email_from,subject,body,email_source,claim_id,status) VALUES (@email_to,@email_from,@subject,@body,@email_source,@claim_id,@status)";
                using (this.connection)
                {
                    if (!this.OpenConnection())
                        return;
                    MySqlCommand mySqlCommand = new MySqlCommand(cmdText, this.connection);
                    mySqlCommand.Parameters.AddWithValue("@email_to", (object)email_to);
                    mySqlCommand.Parameters.AddWithValue("@email_from", (object)email_from);
                    mySqlCommand.Parameters.AddWithValue("@subject", (object)subject);
                    mySqlCommand.Parameters.AddWithValue("@body", (object)body);
                    mySqlCommand.Parameters.AddWithValue("@email_source", (object)email_source);
                    mySqlCommand.Parameters.AddWithValue("@claim_id", (object)claim_id);
                    mySqlCommand.Parameters.AddWithValue("@status", (object)status);
                    mySqlCommand.ExecuteNonQuery().ToString();
                    this.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Errooororor :" + ex.Message);
            }
        }

        public DataSet getReplyid(int claim_id)
        {
            DataSet dataSet = new DataSet();
            string cmdText = "SELECT MAX(id) as id FROM emails WHERE claim_id=@claim_id";
            using (this.connection)
            {
                if (this.OpenConnection())
                {
                    MySqlCommand selectCommand = new MySqlCommand(cmdText, this.connection);
                    selectCommand.Parameters.AddWithValue("@claim_id", (object)claim_id);
                    new MySqlDataAdapter(selectCommand).Fill(dataSet);
                    this.CloseConnection();
                }
            }
            return dataSet;
        }

        public void insertClinicalNote(int claim_id, string notes, string username = "System")
        {
            try
            {
                string cmdText = "INSERT INTO intervention(claim_id,intervention_desc, owner) VALUES(@claim,@notes,@owner)";
                using (this.connection)
                {
                    if (!this.OpenConnection())
                        return;
                    MySqlCommand mySqlCommand = new MySqlCommand(cmdText, this.connection);
                    mySqlCommand.Parameters.AddWithValue("@claim", (object)claim_id);
                    mySqlCommand.Parameters.AddWithValue("@notes", (object)notes);
                    mySqlCommand.Parameters.AddWithValue("@owner", (object)username);
                    mySqlCommand.ExecuteNonQuery();
                    this.CloseConnection();
                }
            }
            catch (Exception ex)
            {
            }
        }

        public DataSet getClinicalReview(string note)
        {
            DataSet dataSet = new DataSet();
            string str = "2022-05-24";
            string cmdText = "SELECT claim_id,claim_number FROM claim WHERE Open=1 AND (gap>=25000 OR client_gap>=25000) AND date_entered>=@dat AND claim_id NOT IN(SELECT DISTINCT claim_id FROM intervention WHERE intervention_desc=@notes)";
            using (this.connection)
            {
                if (this.OpenConnection())
                {
                    MySqlCommand selectCommand = new MySqlCommand(cmdText, this.connection);
                    selectCommand.Parameters.AddWithValue("@notes", (object)note);
                    selectCommand.Parameters.AddWithValue("@dat", (object)str);
                    new MySqlDataAdapter(selectCommand).Fill(dataSet);
                    this.CloseConnection();
                }
            }
            return dataSet;
        }

        public void updateClinicalNote(int claim_id, int val = 4)
        {
            try
            {
                string cmdText = "Update claim SET Open=@num WHERE claim_id=@claim";
                using (this.connection)
                {
                    if (!this.OpenConnection())
                        return;
                    MySqlCommand mySqlCommand = new MySqlCommand(cmdText, this.connection);
                    mySqlCommand.Parameters.AddWithValue("@claim", (object)claim_id);
                    mySqlCommand.Parameters.AddWithValue("@num", (object)val);
                    mySqlCommand.ExecuteNonQuery();
                    this.CloseConnection();
                }
            }
            catch (Exception ex)
            {
            }
        }

        public DataSet getAllDetails()
        {
            DataSet dataSet = new DataSet();
            string cmdText = "SELECT *FROM users_information WHERE status=1 OR username='Wanda'";
            using (this.connection)
            {
                if (this.OpenConnection())
                {
                    new MySqlDataAdapter(new MySqlCommand(cmdText, this.connection)).Fill(dataSet);
                    this.CloseConnection();
                }
            }
            return dataSet;
        }
  


        public DataSet getUserClaims(string username)
        {
            DataSet dataSet = new DataSet();
            DateTime dateTime = DateTime.Today;
            dateTime = dateTime.AddDays(0.0);
            string str = dateTime.ToString("yyyy-MM-dd");
            Console.WriteLine(str);
            string cmdText = "SELECT claim_id,claim_number,savings_scheme+savings_discount as total FROM claim WHERE username=@username AND Open=0 AND date_closed like @date1 AND ((gap>=5000 OR client_gap>=5000) OR claim_id IN(SELECT a.claim_id FROM `claim` as a INNER JOIN member as b ON a.member_id=b.member_id INNER JOIN clients as c ON b.client_id=c.client_id WHERE username=@username AND c.client_name IN('Kaelo','SANLAM','Western') AND (a.savings_scheme+a.savings_discount)>=15000)) ORDER BY total DESC LIMIT 2";
           
            /*
             * if(username=="Stella")
            {
                cmdText = "SELECT claim_id,claim_number,savings_scheme+savings_discount as total FROM claim WHERE username=@username AND Open=0 AND date_closed like @date1 AND ((gap>=5000 OR client_gap>=5000) OR claim_id IN(SELECT a.claim_id FROM `claim` as a INNER JOIN member as b ON a.member_id=b.member_id INNER JOIN clients as c ON b.client_id=c.client_id WHERE username=@username AND c.client_name IN('Kaelo','SANLAM','Western') AND (a.savings_scheme+a.savings_discount)>=15000)) ORDER BY total DESC";

            }
            */
            using (this.connection)
            {
                if (this.OpenConnection())
                {
                    MySqlCommand selectCommand = new MySqlCommand(cmdText, this.connection);
                    selectCommand.Parameters.AddWithValue("@username", (object)username);
                    selectCommand.Parameters.AddWithValue("@date1", (object)(str + "%"));
                    new MySqlDataAdapter(selectCommand).Fill(dataSet);
                    this.CloseConnection();
                }
            }
            return dataSet;
        }

        public void updateQuality(int claim_id, int val = 1)
        {
            try
            {
                string cmdText = "Update claim SET quality=@num WHERE claim_id=@claim";
                using (this.connection)
                {
                    if (!this.OpenConnection())
                        return;
                    MySqlCommand mySqlCommand = new MySqlCommand(cmdText, this.connection);
                    mySqlCommand.Parameters.AddWithValue("@claim", (object)claim_id);
                    mySqlCommand.Parameters.AddWithValue("@num", (object)val);
                    mySqlCommand.ExecuteNonQuery();
                    this.CloseConnection();
                }
            }
            catch (Exception ex)
            {
            }
        }

        public DataSet getEmailMemberS(string email)
        {
            DataSet dataSet = new DataSet();
            string cmdText = "SELECT DISTINCT email FROM bulk_emails WHERE email=@email AND subscription=1";
            using (this.connection)
            {
                if (this.OpenConnection())
                {
                    MySqlCommand selectCommand = new MySqlCommand(cmdText, this.connection);
                    selectCommand.Parameters.AddWithValue("@email", (object)email);
                    new MySqlDataAdapter(selectCommand).Fill(dataSet);
                    this.CloseConnection();
                }
            }
            return dataSet;
        }

        public void updateSubscription(string email)
        {
            try
            {
                string cmdText = "Update bulk_emails SET subscription=1 WHERE email=@email";
                using (this.connection)
                {
                    if (!this.OpenConnection())
                        return;
                    MySqlCommand mySqlCommand = new MySqlCommand(cmdText, this.connection);
                    mySqlCommand.Parameters.AddWithValue("@email", (object)email);
                    mySqlCommand.ExecuteNonQuery();
                    this.CloseConnection();
                }
            }
            catch (Exception ex)
            {
            }
        }

        public DataSet getAdmedNightclaims(string dat)
        {
            DataSet dataSet = new DataSet();
            string str = "%" + dat + "%";
            string cmdText = "SELECT claim_id FROM claim as a inner join member as b on a.member_id=b.member_id WHERE a.date_entered like @dat AND b.client_id=6";
            using (this.connection)
            {
                if (this.OpenConnection())
                {
                    MySqlCommand selectCommand = new MySqlCommand(cmdText, this.connection);
                    selectCommand.Parameters.AddWithValue("@dat", (object)str);
                    new MySqlDataAdapter(selectCommand).Fill(dataSet);
                    this.CloseConnection();
                }
            }
            return dataSet;
        }

        public DataSet admedReasonCodes(string treatment_code)
        {
            DataSet dataSet = new DataSet();
            string cmdText = "SELECT BENEFIT FROM admed_reason_codes WHERE SERVICE_CODE=@treatment_code";
            using (this.connection)
            {
                if (this.OpenConnection())
                {
                    MySqlCommand selectCommand = new MySqlCommand(cmdText, this.connection);
                    selectCommand.Parameters.AddWithValue("@treatment_code", (object)treatment_code);
                    new MySqlDataAdapter(selectCommand).Fill(dataSet);
                    this.CloseConnection();
                }
            }
            return dataSet;
        }

        public void updateNewAdmed(int claim_id, string preassessor)
        {
            try
            {
                string cmdText = "Update claim SET Open=5,preassessor=@preassessor WHERE claim_id=@claim_id";
                using (this.connection)
                {
                    if (!this.OpenConnection())
                        return;
                    MySqlCommand mySqlCommand = new MySqlCommand(cmdText, this.connection);
                    mySqlCommand.Parameters.AddWithValue("@claim_id", (object)claim_id);
                    mySqlCommand.Parameters.AddWithValue("@preassessor", (object)preassessor);
                    mySqlCommand.ExecuteNonQuery();
                    this.CloseConnection();
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void insert8days(int claim_id)
        {
            try
            {
                string cmdText = "INSERT INTO 10daysclaims(claim_id) VALUES(@claim)";
                using (this.connection)
                {
                    if (!this.OpenConnection())
                        return;
                    MySqlCommand mySqlCommand = new MySqlCommand(cmdText, this.connection);
                    mySqlCommand.Parameters.AddWithValue("@claim", (object)claim_id);
                    mySqlCommand.ExecuteNonQuery();
                    this.CloseConnection();
                }
            }
            catch (Exception ex)
            {
            }
        }

        public DataTable get8DayClaims()
        {
            DataTable dataTable = new DataTable();
            DateTime dateTime = DateTime.Today;
            dateTime = dateTime.AddDays(-8.0);
            string str = dateTime.ToString("yyyy-MM-dd");
            string cmdText = "SELECT a.claim_id,a.claim_number,b.first_name,b.surname,b.medical_scheme, b.policy_number,a.username,c.client_name,a.date_entered FROM claim as a inner join member as b ON a.member_id=b.member_id INNER JOIN clients as c ON b.client_id=c.client_id WHERE a.date_entered <= @dat AND a.Open=1 ORDER BY a.claim_id DESC LIMIT 200";
            using (this.connection)
            {
                if (this.OpenConnection())
                {
                    MySqlCommand selectCommand = new MySqlCommand(cmdText, this.connection);
                    selectCommand.Parameters.AddWithValue("@dat", (object)str);
                    new MySqlDataAdapter(selectCommand).Fill(dataTable);
                    this.CloseConnection();
                }
            }
            return dataTable;
        }

        public void btc(string btcvalue)
        {
            try
            {
                string cmdText = "UPDATE scheme_options SET description=@descri WHERE id=1";
                using (this.connection)
                {
                    if (!this.OpenConnection())
                        return;
                    MySqlCommand mySqlCommand = new MySqlCommand(cmdText, this.connection);
                    mySqlCommand.Parameters.AddWithValue("@descri", (object)btcvalue);
                    mySqlCommand.ExecuteNonQuery();
                    this.CloseConnection();
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void updateModifierdetails(int id, string modifier, string modifier_name)
        {
            try
            {
                string cmdText = "UPDATE claim_line SET modifier=@modifier,modifier_name=@modifier_name WHERE id=@id";
                using (this.connection)
                {
                    if (!this.OpenConnection())
                        return;
                    MySqlCommand mySqlCommand = new MySqlCommand(cmdText, this.connection);
                    mySqlCommand.Parameters.AddWithValue("@id", (object)id);
                    mySqlCommand.Parameters.AddWithValue("@modifier", (object)modifier);
                    mySqlCommand.Parameters.AddWithValue("@modifier_name", (object)modifier_name);
                    mySqlCommand.ExecuteNonQuery();
                    this.CloseConnection();
                }
            }
            catch (Exception ex)
            {
            }
        }
        public void insertReopenedClaims(int claim_id, string reason, string entered_by,string date_closed,double last_scheme_savings,double last_discount_savings)
        {
            try
            {
                string cmdText = "INSERT INTO reopened_claims(claim_id,reason,entered_by,date_closed,last_scheme_savings,last_discount_savings) VALUES(@claim_id,@reason,@entered_by,@date_closed,@last_scheme_savings,@last_discount_savings)";
                using (this.connection)
                {
                    if (!this.OpenConnection())
                        return;
                    MySqlCommand mySqlCommand = new MySqlCommand(cmdText, this.connection);
                    mySqlCommand.Parameters.AddWithValue("@claim_id", claim_id);
                    mySqlCommand.Parameters.AddWithValue("@reason", reason);
                    mySqlCommand.Parameters.AddWithValue("@entered_by", entered_by);
                    mySqlCommand.Parameters.AddWithValue("@date_closed", date_closed);
                    mySqlCommand.Parameters.AddWithValue("@last_scheme_savings", last_scheme_savings);
                    mySqlCommand.Parameters.AddWithValue("@last_discount_savings", last_discount_savings);
                    //mySqlCommand.Parameters.AddWithValue("@modifier_name", (object)modifier_name);
                    mySqlCommand.ExecuteNonQuery();
                    this.CloseConnection();
                }
            }
            catch (Exception ex)
            {
            }
        }

        public DataSet checkDocument(int claim_id, string doc_description)
        {
            DataSet dataSet = new DataSet();
            string cmdText = "SELECT doc_id FROM documents WHERE claim_id=@claim_id AND doc_description=@doc_description";
            using (this.connection)
            {
                if (this.OpenConnection())
                {
                    MySqlCommand selectCommand = new MySqlCommand(cmdText, this.connection);
                    selectCommand.Parameters.AddWithValue("@claim_id", (object)claim_id);
                    selectCommand.Parameters.AddWithValue("@doc_description", (object)doc_description);
                    new MySqlDataAdapter(selectCommand).Fill(dataSet);
                    this.CloseConnection();
                }
            }
            return dataSet;
        }

        public void updateMember(
          int member_id,
          string policy_number = "",
          string member_name = "",
          string member_surname = "",
          string telephone = "",
          string email = "",
          string medical_aid = "Unknown",
          string cell = "",
          string product_name = "",
          string product_code = "",
          string benefitiary_number = "",
          string scheme_option = "",
          string scheme_number = "",
          string id_number = "")
        {
            string cmdText = "UPDATE member SET policy_number=@policy_number,first_name=@member_name,surname=@member_surname,productName=@productName,telephone=@telephone,cell=@cell,email=@email,medical_scheme=@medical_scheme,product_code=@product_code,beneficiary_number=@benefitiary_number,scheme_option=@scheme_option,scheme_number=@scheme_number,id_number=@id_number WHERE member_id=@member_id";
            using (this.connection)
            {
                if (!this.OpenConnection())
                    return;
                MySqlCommand mySqlCommand = new MySqlCommand(cmdText, this.connection);
                mySqlCommand.Parameters.AddWithValue("@member_id", (object)member_id);
                mySqlCommand.Parameters.AddWithValue("@policy_number", (object)policy_number);
                mySqlCommand.Parameters.AddWithValue("@member_name", (object)member_name);
                mySqlCommand.Parameters.AddWithValue("@member_surname", (object)member_surname);
                mySqlCommand.Parameters.AddWithValue("@productName", (object)product_name);
                mySqlCommand.Parameters.AddWithValue("@telephone", (object)telephone);
                mySqlCommand.Parameters.AddWithValue("@cell", (object)cell);
                mySqlCommand.Parameters.AddWithValue("@email", (object)email);
                mySqlCommand.Parameters.AddWithValue("@medical_scheme", (object)medical_aid);
                mySqlCommand.Parameters.AddWithValue("@product_code", (object)product_code);
                mySqlCommand.Parameters.AddWithValue("@benefitiary_number", (object)benefitiary_number);
                mySqlCommand.Parameters.AddWithValue("@scheme_option", (object)scheme_option);
                mySqlCommand.Parameters.AddWithValue("@scheme_number", (object)scheme_number);
                mySqlCommand.Parameters.AddWithValue("@id_number", (object)id_number);
                mySqlCommand.ExecuteNonQuery();
                this.CloseConnection();
                Thread.Sleep(1000);
            }
        }

        public void updateClaim(
          int claim_id,
          DateTime? newdate = null,
          string folder = "",
          DateTime? service_date = null,
          string icd10 = "",
          int pmb = 0,
          double amount_charged = 0.0,
          string icd10_descr = "",
          double scheme_paid = 0.0,
          double gap = 0.0,
          int open = 1,
          string preassessor = "",
          string patient_number = "")
        {
            string cmdText = "UPDATE claim SET icd10=@icd10,pmb=@pmb,charged_amnt=@charged_amnt,client_gap=@gap,Service_Date=@Service_Date,my_folder=@my_folder,icd10_desc=@icd10_desc,scheme_paid=@scheme_paid,Open=@open,preassessor=@preassessor,patient_number=@patient_number,date_entered=@date_entered WHERE claim_id=@claim_id";
            using (this.connection)
            {
                if (!this.OpenConnection())
                    return;
                MySqlCommand mySqlCommand = new MySqlCommand(cmdText, this.connection);
                mySqlCommand.Parameters.AddWithValue("@claim_id", (object)claim_id);
                mySqlCommand.Parameters.AddWithValue("@date_entered", (object)newdate);
                mySqlCommand.Parameters.AddWithValue("@icd10", (object)icd10);
                mySqlCommand.Parameters.AddWithValue("@pmb", (object)pmb);
                mySqlCommand.Parameters.AddWithValue("@charged_amnt", (object)amount_charged);
                mySqlCommand.Parameters.AddWithValue("@gap", (object)gap);
                mySqlCommand.Parameters.AddWithValue("@Service_Date", (object)service_date);
                mySqlCommand.Parameters.AddWithValue("@my_folder", (object)folder);
                mySqlCommand.Parameters.AddWithValue("@icd10_desc", (object)icd10_descr);
                mySqlCommand.Parameters.AddWithValue("@scheme_paid", (object)scheme_paid);
                mySqlCommand.Parameters.AddWithValue("@open", (object)open);
                mySqlCommand.Parameters.AddWithValue("@preassessor", (object)preassessor);
                mySqlCommand.Parameters.AddWithValue("@patient_number", (object)patient_number);
                mySqlCommand.ExecuteNonQuery();
                this.CloseConnection();
                Thread.Sleep(1000);
            }
        }
        public DataSet getSubscribers()
        {
            DataSet ds = new DataSet();
            string client = "client";
            string query = " SELECT name, surname, id_number, dob, email, contact_number, medical_scheme, scheme_option, medical_aid_number, role FROM `web_clients` WHERE role = @client AND status = 1";

            using (connection)
            {
                //Open connection
                if (this.OpenConnection() == true)
                {

                    MySqlCommand selectCommand = new MySqlCommand(query, this.connection);
                    selectCommand.Parameters.AddWithValue("@client", client);
                    new MySqlDataAdapter(selectCommand).Fill(ds);
                    this.CloseConnection();
                }
            }
            return ds;
        }

        public void insertSplitMember(string membership_number, string beneficiary_name, string beneficiary_scheme_join_date, string beneficiary_id_number, string beneficiary_date_of_birth, string entered_by, string file_name)
        {
            try
            {
                string cmdText = "INSERT INTO split_member(membership_number,beneficiary_name,beneficiary_scheme_join_date,beneficiary_id_number,beneficiary_date_of_birth,entered_by,file_name) VALUES (@membership_number,@beneficiary_name,@beneficiary_scheme_join_date,@beneficiary_id_number,@beneficiary_date_of_birth,@entered_by,@file_name)";
                using (this.connection)
                {
                    if (!this.OpenConnection())
                        return;
                    MySqlCommand mySqlCommand = new MySqlCommand(cmdText, this.connection);
                    mySqlCommand.Parameters.AddWithValue("@membership_number", (object)membership_number);
                    mySqlCommand.Parameters.AddWithValue("@beneficiary_name", (object)beneficiary_name);
                    mySqlCommand.Parameters.AddWithValue("@beneficiary_scheme_join_date", (object)beneficiary_scheme_join_date);
                    mySqlCommand.Parameters.AddWithValue("@beneficiary_id_number", (object)beneficiary_id_number);
                    mySqlCommand.Parameters.AddWithValue("@beneficiary_date_of_birth", (object)beneficiary_date_of_birth);
                    mySqlCommand.Parameters.AddWithValue("@entered_by", (object)entered_by);
                    mySqlCommand.Parameters.AddWithValue("@file_name", (object)file_name);
                    mySqlCommand.ExecuteNonQuery().ToString();
                    this.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Errooororor :" + ex.Message);
            }
        }
        public void insertSplitClaim(int split_member_id, string loyalty_number, string procedure_date, string admission_date, string discharge_date, string co_payment, string entered_by, string file_name)
        {
            try
            {
                string cmdText = "INSERT INTO split_claim(split_member_id,loyalty_number,procedure_date,admission_date,discharge_date,co_payment,entered_by,file_name) VALUES (@split_member_id,@loyalty_number,@procedure_date,@admission_date,@discharge_date,@co_payment,@entered_by,@file_name)";
                using (this.connection)
                {
                    if (!this.OpenConnection())
                        return;
                    MySqlCommand mySqlCommand = new MySqlCommand(cmdText, this.connection);
                    mySqlCommand.Parameters.AddWithValue("@split_member_id", (object)split_member_id);
                    mySqlCommand.Parameters.AddWithValue("@loyalty_number", (object)loyalty_number);
                    mySqlCommand.Parameters.AddWithValue("@procedure_date", (object)procedure_date);
                    mySqlCommand.Parameters.AddWithValue("@admission_date", (object)admission_date);
                    mySqlCommand.Parameters.AddWithValue("@discharge_date", (object)discharge_date);
                    mySqlCommand.Parameters.AddWithValue("@co_payment", (object)co_payment);
                    mySqlCommand.Parameters.AddWithValue("@entered_by", (object)entered_by);
                    mySqlCommand.Parameters.AddWithValue("@file_name", (object)file_name);
                    mySqlCommand.ExecuteNonQuery().ToString();
                    this.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Errooororor :" + ex.Message);
            }
        }

        public void insertSplitDoctor(int split_claim_id, string practice_number, string practice_name,string hospital_name, string entered_by, string file_name)
        {
            try
            {
                string cmdText = "INSERT INTO split_doctors(split_claim_id,practice_number,practice_name,hospital_name,entered_by,file_name) VALUES (@split_claim_id,@practice_number,@practice_name,@hospital_name,@entered_by,@file_name)";
                using (this.connection)
                {
                    if (!this.OpenConnection())
                        return;
                    MySqlCommand mySqlCommand = new MySqlCommand(cmdText, this.connection);
                    mySqlCommand.Parameters.AddWithValue("@split_claim_id", (object)split_claim_id);
                    mySqlCommand.Parameters.AddWithValue("@practice_number", (object)practice_number);
                    mySqlCommand.Parameters.AddWithValue("@practice_name", (object)practice_name);
                    mySqlCommand.Parameters.AddWithValue("@hospital_name", (object)hospital_name);
                    mySqlCommand.Parameters.AddWithValue("@entered_by", (object)entered_by);
                    mySqlCommand.Parameters.AddWithValue("@file_name", (object)file_name);
                    mySqlCommand.ExecuteNonQuery().ToString();
                    this.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Errooororor :" + ex.Message);
            }
        }

        public void insertSplitClaimLine(int split_claim_id,string practice_number, string claiminsureditemadditionalinfoid, string servicedate, string icdcode, string codedescription, string procedurecode, string procedurename, string proceduretype, string linkedprocedurecodedescription, string medicalschemepaidconcat, string medicalschemepaidinkedprocedurecodesum, string modifierpercentage, string modifiervalue,double amountcharged,double medicalschemerateinput,double medicalschemepaidinput,string medicalschemerejectioncode,string medicalschemerejectionreason,string insureditempayoutadditionalinfo,string rejectionreason,string entered_by,string file_name,string hospital_name,int copayment)
        {
            try
            {
                string cmdText = "INSERT INTO split_claim_line(split_claim_id,practice_number,claiminsureditemadditionalinfoid,servicedate,icdcode,codedescription,procedurecode,procedurename,proceduretype,linkedprocedurecodedescription,medicalschemepaidconcat,medicalschemepaidinkedprocedurecodesum,modifierpercentage,modifiervalue,amountcharged,medicalschemerateinput,medicalschemepaidinput,medicalschemerejectioncode,medicalschemerejectionreason,insureditempayoutadditionalinfo,rejectionreason,entered_by,file_name,hospital_name,copayment) VALUES (@split_claim_id,@practice_number,@claiminsureditemadditionalinfoid,@servicedate,@icdcode,@codedescription,@procedurecode,@procedurename,@proceduretype,@linkedprocedurecodedescription,@medicalschemepaidconcat,@medicalschemepaidinkedprocedurecodesum,@modifierpercentage,@modifiervalue,@amountcharged,@medicalschemerateinput,@medicalschemepaidinput,@medicalschemerejectioncode,@medicalschemerejectionreason,@insureditempayoutadditionalinfo,@rejectionreason,@entered_by,@file_name,@hospital_name,@copayment)";
                using (this.connection)
                {
                    if (!this.OpenConnection())
                        return;
                    MySqlCommand mySqlCommand = new MySqlCommand(cmdText, this.connection);
                    mySqlCommand.Parameters.AddWithValue("@split_claim_id", (object)split_claim_id);
                    mySqlCommand.Parameters.AddWithValue("@practice_number", (object)practice_number);
                    mySqlCommand.Parameters.AddWithValue("@claiminsureditemadditionalinfoid", (object)claiminsureditemadditionalinfoid);
                    mySqlCommand.Parameters.AddWithValue("@servicedate", (object)servicedate);
                    mySqlCommand.Parameters.AddWithValue("@icdcode", (object)icdcode);
                    mySqlCommand.Parameters.AddWithValue("@codedescription", (object)codedescription);
                    mySqlCommand.Parameters.AddWithValue("@procedurecode", (object)procedurecode);
                    mySqlCommand.Parameters.AddWithValue("@procedurename", (object)procedurename);
                    mySqlCommand.Parameters.AddWithValue("@proceduretype", (object)proceduretype);
                    mySqlCommand.Parameters.AddWithValue("@linkedprocedurecodedescription", (object)linkedprocedurecodedescription);
                    mySqlCommand.Parameters.AddWithValue("@medicalschemepaidconcat", (object)medicalschemepaidconcat);
                    mySqlCommand.Parameters.AddWithValue("@medicalschemepaidinkedprocedurecodesum", (object)medicalschemepaidinkedprocedurecodesum);
                    mySqlCommand.Parameters.AddWithValue("@modifierpercentage", (object)modifierpercentage);
                    mySqlCommand.Parameters.AddWithValue("@modifiervalue", (object)modifiervalue);
                    mySqlCommand.Parameters.AddWithValue("@amountcharged", (object)amountcharged);
                    mySqlCommand.Parameters.AddWithValue("@medicalschemerateinput", (object)medicalschemerateinput);
                    mySqlCommand.Parameters.AddWithValue("@medicalschemepaidinput", (object)medicalschemepaidinput);
                    mySqlCommand.Parameters.AddWithValue("@medicalschemerejectioncode", (object)medicalschemerejectioncode);
                    mySqlCommand.Parameters.AddWithValue("@medicalschemerejectionreason", (object)medicalschemerejectionreason);
                    mySqlCommand.Parameters.AddWithValue("@insureditempayoutadditionalinfo", (object)insureditempayoutadditionalinfo);
                    mySqlCommand.Parameters.AddWithValue("@rejectionreason", (object)rejectionreason);
                    mySqlCommand.Parameters.AddWithValue("@entered_by", (object)entered_by);
                    mySqlCommand.Parameters.AddWithValue("@file_name", (object)file_name);
                    mySqlCommand.Parameters.AddWithValue("@hospital_name", (object)hospital_name);
                    mySqlCommand.Parameters.AddWithValue("@copayment", (object)copayment);
                    mySqlCommand.ExecuteNonQuery().ToString();
                    this.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Errooororor :" + ex.Message);
            }
        }
        public void insertSplitFiles(string file_name, int total_claims, int total_loaded, string entered_by, string status,int total_actclaims=0)
        {
            try
            {
                string cmdText = "INSERT INTO split_files(file_name,total_claims,total_loaded,entered_by,status,total_actclaims) VALUES (@file_name,@total_claims,@total_loaded,@entered_by,@status,@total_actclaims)";
                using (this.connection)
                {
                    if (!this.OpenConnection())
                        return;
                    MySqlCommand mySqlCommand = new MySqlCommand(cmdText, this.connection);
                    mySqlCommand.Parameters.AddWithValue("@file_name", (object)file_name);
                    mySqlCommand.Parameters.AddWithValue("@total_claims", (object)total_claims);
                    mySqlCommand.Parameters.AddWithValue("@total_loaded", (object)total_loaded);
                    mySqlCommand.Parameters.AddWithValue("@entered_by", (object)entered_by);
                    mySqlCommand.Parameters.AddWithValue("@status", (object)status);                  
                    mySqlCommand.Parameters.AddWithValue("@total_actclaims", (object)total_actclaims);                  
                    
                    mySqlCommand.ExecuteNonQuery().ToString();
                    this.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Errooororor :" + ex.Message);
            }
        }

        public DataSet getSplitMembers(string membership_number,string beneficiary_id_number, string file_name)
        {
            DataSet ds = new DataSet();            
            string query = " SELECT id,membership_number,beneficiary_name,file_name FROM split_member WHERE membership_number = @membership_number AND beneficiary_id_number = @beneficiary_id_number AND file_name=@file_name";

            using (connection)
            {
                //Open connection
                if (this.OpenConnection() == true)
                {

                    MySqlCommand selectCommand = new MySqlCommand(query, this.connection);
                    selectCommand.Parameters.AddWithValue("@membership_number", membership_number);
                    selectCommand.Parameters.AddWithValue("@beneficiary_id_number", beneficiary_id_number);
                    selectCommand.Parameters.AddWithValue("@file_name", file_name);
                    new MySqlDataAdapter(selectCommand).Fill(ds);
                    this.CloseConnection();
                }
            }
            return ds;
        }
        public DataSet getSplitClaim(int split_member_id, string loyalty_number)
        {
            DataSet ds = new DataSet();
            string query = "SELECT id,loyalty_number FROM split_claim WHERE split_member_id = @split_member_id AND loyalty_number = @loyalty_number";

            using (connection)
            {
                //Open connection
                if (this.OpenConnection() == true)
                {

                    MySqlCommand selectCommand = new MySqlCommand(query, this.connection);
                    selectCommand.Parameters.AddWithValue("@split_member_id", split_member_id);
                    selectCommand.Parameters.AddWithValue("@loyalty_number", loyalty_number);
                    new MySqlDataAdapter(selectCommand).Fill(ds);
                    this.CloseConnection();
                }
            }
            return ds;
        }
        public DataSet getSplitDoctor(int split_claim_id, string hospital_name)
        {
            DataSet ds = new DataSet();
            string query = " SELECT id,split_claim_id,practice_number,practice_name,hospital_name FROM split_doctors WHERE split_claim_id = @split_claim_id AND hospital_name = @hospital_name";

            using (connection)
            {
                //Open connection
                if (this.OpenConnection() == true)
                {

                    MySqlCommand selectCommand = new MySqlCommand(query, this.connection);
                    selectCommand.Parameters.AddWithValue("@split_claim_id", split_claim_id);
                    selectCommand.Parameters.AddWithValue("@hospital_name", hospital_name);
                    new MySqlDataAdapter(selectCommand).Fill(ds);
                    this.CloseConnection();
                }
            }
            return ds;
        }

        public DataSet getSplitClaimLine(int split_claim_id, string hospital_name, string servicedate, string icdcode,string procedurecode, double amountcharged, double medicalschemepaidinput)
        {
            DataSet ds = new DataSet();
            string query = " SELECT id,split_claim_id,practice_number,hospital_name FROM split_claim_line WHERE split_claim_id = @split_claim_id AND hospital_name = @hospital_name AND servicedate=@servicedate AND icdcode=@icdcode AND procedurecode=@procedurecode AND amountcharged=@amountcharged AND medicalschemepaidinput=@medicalschemepaidinput";

            using (connection)
            {
                //Open connection
                if (this.OpenConnection() == true)
                {

                    MySqlCommand selectCommand = new MySqlCommand(query, this.connection);
                    selectCommand.Parameters.AddWithValue("@split_claim_id", split_claim_id);
                    selectCommand.Parameters.AddWithValue("@hospital_name", hospital_name);
                    selectCommand.Parameters.AddWithValue("@servicedate", servicedate);
                    selectCommand.Parameters.AddWithValue("@icdcode", icdcode);
                    selectCommand.Parameters.AddWithValue("@procedurecode", procedurecode);
                    selectCommand.Parameters.AddWithValue("@amountcharged", amountcharged);
                    selectCommand.Parameters.AddWithValue("@medicalschemepaidinput", medicalschemepaidinput);
                    new MySqlDataAdapter(selectCommand).Fill(ds);
                    this.CloseConnection();
                }
            }
            return ds;
        }
        public DataSet getAAAFTP(string mission)
        {
            DataSet ds = new DataSet();
            string query = "SELECT * FROM aaa WHERE mission=@mission";

            using (connection)
            {
                //Open connection
                if (this.OpenConnection() == true)
                {

                    MySqlCommand selectCommand = new MySqlCommand(query, this.connection);
                    selectCommand.Parameters.AddWithValue("@mission", mission);
                    new MySqlDataAdapter(selectCommand).Fill(ds);
                    this.CloseConnection();
                }
            }
            return ds;
        }
        public DataSet getAAA(int id)
        {
            DataSet ds = new DataSet();
            string query = "SELECT * FROM aaa WHERE id=@id";

            using (connection)
            {
                //Open connection
                if (this.OpenConnection() == true)
                {

                    MySqlCommand selectCommand = new MySqlCommand(query, this.connection);
                    selectCommand.Parameters.AddWithValue("@id", id);
                    new MySqlDataAdapter(selectCommand).Fill(ds);
                    this.CloseConnection();
                }
            }
            return ds;
        }
        public DataSet getFilecheck(string file_name)
        {
            DataSet ds = new DataSet();
            string query = "SELECT * FROM split_files WHERE file_name=@file_name";

            using (connection)
            {
                //Open connection
                if (this.OpenConnection() == true)
                {

                    MySqlCommand selectCommand = new MySqlCommand(query, this.connection);
                    selectCommand.Parameters.AddWithValue("@file_name", file_name);
                    new MySqlDataAdapter(selectCommand).Fill(ds);
                    this.CloseConnection();
                }
            }
            return ds;
        }
        public void updateAAA(int id, string files)
        {
            try
            {
                DateTime last_update = DateTime.Now;
                string cmdText = "UPDATE aaa SET files=@files,last_update=@last_update WHERE id=@id";
                using (this.connection)
                {
                    if (!this.OpenConnection())
                        return;
                    MySqlCommand mySqlCommand = new MySqlCommand(cmdText, this.connection);
                    mySqlCommand.Parameters.AddWithValue("@id", (object)id);
                    mySqlCommand.Parameters.AddWithValue("@files", (object)files);
                    mySqlCommand.Parameters.AddWithValue("@last_update", (object)last_update);
            

                    mySqlCommand.ExecuteNonQuery().ToString();
                    this.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Errooororor :" + ex.Message);
            }
        }
        public void updateTempSplit(int split_claim_id, string hospital_name, string procedure_date, string icd10_code, string tariff_code, double charged_amount, double scheme_paid_amount, int copayment)
        {
            try
            {
                DateTime last_update = DateTime.Now;
                string cmdText = "UPDATE split_claim_line SET copayment=@copayment WHERE split_claim_id=@split_claim_id AND hospital_name=@hospital_name AND servicedate=@procedure_date AND icdcode=@icd10_code AND procedurecode=@tariff_code AND amountcharged=@charged_amount AND medicalschemepaidinput=@scheme_paid_amount";
                using (this.connection)
                {
                    if (!this.OpenConnection())
                        return;
                    MySqlCommand mySqlCommand = new MySqlCommand(cmdText, this.connection);
                    mySqlCommand.Parameters.AddWithValue("@copayment", (object)copayment);
                    mySqlCommand.Parameters.AddWithValue("@split_claim_id", (object)split_claim_id);
                    mySqlCommand.Parameters.AddWithValue("@hospital_name", (object)hospital_name);
                    mySqlCommand.Parameters.AddWithValue("@procedure_date", (object)procedure_date);
                    mySqlCommand.Parameters.AddWithValue("@icd10_code", (object)icd10_code);
                    mySqlCommand.Parameters.AddWithValue("@tariff_code", (object)tariff_code);
                    mySqlCommand.Parameters.AddWithValue("@charged_amount", (object)charged_amount);
                    mySqlCommand.Parameters.AddWithValue("@scheme_paid_amount", (object)scheme_paid_amount);


                    mySqlCommand.ExecuteNonQuery().ToString();
                    this.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Errooororor :" + ex.Message);
            }
        }
        public DataSet getBrokerTotals(int broker_id)
        {
            DataSet dataSet = new DataSet();
            string query = "SELECT COUNT(*) as totals FROM web_clients WHERE broker_id=@broker_id";
            using (this.connection)
            {
                if (this.OpenConnection())
                {
                    MySqlCommand selectCommand = new MySqlCommand(query, this.connection);
                    selectCommand.Parameters.AddWithValue("@broker_id", broker_id);
                    new MySqlDataAdapter(selectCommand).Fill(dataSet);
                    this.CloseConnection();
                }
            }
            return dataSet;
        }
        public DataSet getClientSeamless(string client)
        {
            string subbrokers = "('Kaelo Gap','MedExpense','Centriq Cancer','Dis-Chem Health','OLD Dis-Chem Health - Western National')";
            if(client=="Sanlam")
            {
                subbrokers = "('Sanlam Gap')";
            }
            else if (client == "Western")
            {
                subbrokers = "('Western Gap Care','Western Gap')";
            }
            DateTime dateTime1 = DateTime.Today;
            dateTime1 = dateTime1.AddMonths(-6);
            string policy_cancellationdate = dateTime1.ToString("yyyy-MM-dd");           
            DataSet dataSet = new DataSet();
            string query = "SELECT COUNT(*) as totals FROM chf WHERE ProductName IN "+ subbrokers + " AND (policy_cancellationdate >@policy_cancellationdate OR policy_cancellationdate is null OR policy_cancellationdate = '')";
       
            using (this.connection_seamless)
            {
                if (this.OpenConnection_seamless())
                {
                    MySqlCommand selectCommand = new MySqlCommand(query, this.connection_seamless);
                    selectCommand.Parameters.AddWithValue("@policy_cancellationdate", policy_cancellationdate);
                    new MySqlDataAdapter(selectCommand).Fill(dataSet);
                    this.CloseConnection_seamless();
                }
            }
            return dataSet;
        }


        public DataSet getClientsWithSavings(string client_name,string dat)
        {
            
            dat="%"+ dat + "%";
            DataSet dataSet = new DataSet();
            string query = "SELECT SUM(a.savings_scheme + a.savings_discount) as savings FROM `claim` as a INNER JOIN member as b on a.member_id=b.member_id INNER JOIN clients as c ON b.client_id=c.client_id WHERE Open = 0 AND a.date_closed LIKE @dat AND c.client_name=@client_name";
            using (this.connection)
            {
                if (this.OpenConnection())
                {
                    MySqlCommand selectCommand = new MySqlCommand(query, this.connection);
                    selectCommand.Parameters.AddWithValue("@dat", dat);
                    selectCommand.Parameters.AddWithValue("@client_name", client_name);
                    new MySqlDataAdapter(selectCommand).Fill(dataSet);
                    this.CloseConnection();
                }
            }
            return dataSet;
        }
        public DataSet getReopenedClaimsPerClient(string client_name,string mydate)
        {
          
            string dat = "%" + mydate + "%";
            DataSet dataSet = new DataSet();
            string query = "SELECT k.claim_id, a.claim_number,a.username,a.date_entered,k.date_closed,last_scheme_savings+last_discount_savings AS first_savings,k.reopened_date,a.date_closed as final_date_closed,a.savings_scheme+a.savings_discount as final_savings,c.client_name FROM `reopened_claims` as k INNER JOIN claim as a ON k.claim_id = a.claim_id INNER JOIN member as b ON a.member_id = b.member_id INNER JOIN clients as c ON b.client_id = c.client_id WHERE k.reopened_date like @dd AND a.Open = 0 AND k.date_closed not like @dd AND k.date_closed <@dat AND c.client_name =@client_name";
            using (this.connection)
            {
                if (this.OpenConnection())
                {
                    MySqlCommand selectCommand = new MySqlCommand(query, this.connection);
                    selectCommand.Parameters.AddWithValue("@dd", dat);
                    selectCommand.Parameters.AddWithValue("@dat", mydate);
                    selectCommand.Parameters.AddWithValue("@client_name", client_name);
                    new MySqlDataAdapter(selectCommand).Fill(dataSet);
                    this.CloseConnection();
                }
            }
            return dataSet;
        }

        public DataSet getTotalClaims(string client_name, string dat)
        {

            dat = "%" + dat + "%";
            DataSet dataSet = new DataSet();
            string query = "SELECT COUNT(*) as claims FROM `claim` as a INNER JOIN member as b on a.member_id=b.member_id INNER JOIN clients as c ON b.client_id=c.client_id WHERE a.date_entered LIKE @date_entered AND c.client_name=@client_name";
            using (this.connection)
            {
                if (this.OpenConnection())
                {
                    MySqlCommand selectCommand = new MySqlCommand(query, this.connection);
                    selectCommand.Parameters.AddWithValue("@date_entered", dat);
                    selectCommand.Parameters.AddWithValue("@client_name", client_name);
                    new MySqlDataAdapter(selectCommand).Fill(dataSet);
                    this.CloseConnection();
                }
            }
            return dataSet;
        }
        public DataSet getCinigeClaims(string client_name, string dat,string claim_type)
        {

            dat = "%" + dat + "%";
            DataSet dataSet = new DataSet();
            string query = "SELECT COUNT(*) as claims FROM `claim` as a INNER JOIN member as b on a.member_id=b.member_id INNER JOIN clients as c ON b.client_id=c.client_id WHERE a.date_entered LIKE @date_entered AND c.client_name=@client_name AND claim_type=@claim_type";
            using (this.connection)
            {
                if (this.OpenConnection())
                {
                    MySqlCommand selectCommand = new MySqlCommand(query, this.connection);
                    selectCommand.Parameters.AddWithValue("@date_entered", dat);
                    selectCommand.Parameters.AddWithValue("@client_name", client_name);
                    selectCommand.Parameters.AddWithValue("@claim_type", claim_type);
                    new MySqlDataAdapter(selectCommand).Fill(dataSet);
                    this.CloseConnection();
                }
            }
            return dataSet;
        }
        public DataSet getSnapClaims(string dat)
        {

            dat = "%" + dat + "%";
            DataSet dataSet = new DataSet();
            string query = "SELECT claim_id FROM closed_cases_snap WHERE date_closed LIKE @date_closed";
            using (this.connection)
            {
                if (this.OpenConnection())
                {
                    MySqlCommand selectCommand = new MySqlCommand(query, this.connection);
                    selectCommand.Parameters.AddWithValue("@date_closed", dat);
                    new MySqlDataAdapter(selectCommand).Fill(dataSet);
                    this.CloseConnection();
                }
            }
            return dataSet;
        }
        public void insertClosedCasesSnap(string dat)
        {
            dat = "%" + dat + "%";
            try
            {
                string cmdText = "INSERT INTO closed_cases_snap (claim_id, charged_amount, scheme_paid, gap,client_gap,scheme_savings,discount_savings,date_closed) select claim_id,charged_amnt,scheme_paid,gap,client_gap,savings_scheme,savings_discount,date_closed from claim where date_closed like @date_closed and Open = 0";
                using (this.connection)
                {
                    if (!this.OpenConnection())
                        return;
                    MySqlCommand mySqlCommand = new MySqlCommand(cmdText, this.connection);
                    mySqlCommand.Parameters.AddWithValue("@date_closed", dat);
                    mySqlCommand.ExecuteNonQuery();                 
                  this.CloseConnection();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("There is an error" + ex.Message);
            }
        }

    }
}
