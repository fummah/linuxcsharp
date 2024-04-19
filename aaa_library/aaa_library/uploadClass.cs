using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using WinSCP;
using System.Threading;

namespace aaa_library
{
    public class uploadClass
    {
        public static int myUpload(string HostName,string UserName, string Password,string srcFilename, string destFilename)
        {

            try
            {

                // Setup session options
                SessionOptions sessionOptions = new SessionOptions
                {

                    Protocol = Protocol.Sftp,
                    HostName = HostName,
                    UserName = UserName,
                    Password = Password,
                    GiveUpSecurityAndAcceptAnySshHostKey = true
                };

                using (Session session = new Session())
                {
                    // Connect
                    sessionOptions.GiveUpSecurityAndAcceptAnySshHostKey = true;
                    session.Open(sessionOptions);
                    //File Upload
                    Console.WriteLine("Starting..........");
                    TransferOptions transferOptions = new TransferOptions();
                    transferOptions.TransferMode = TransferMode.Binary;                   
                    TransferOperationResult transferResult;
                    transferResult =
                        session.PutFiles(srcFilename, destFilename, false, transferOptions);

                    // Throw on any error
                    transferResult.Check();

                    // Print results
                    foreach (TransferEventArgs transfer in transferResult.Transfers)
                    {
                        Console.WriteLine("Upload of {0} succeeded", transfer.FileName);
                    }
                }

                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e);
                return 1;
            }
        }
    }
}
