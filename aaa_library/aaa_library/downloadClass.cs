using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinSCP;
using System.IO;

namespace aaa_library
{
    class downloadClass
    { 
    public static void downloadFiles(string HostName,string UserName,string Password,string localPath, string remotePath)
    {
        try { 
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

                    // Download files
                    if (!Directory.Exists(localPath))
                    {
                        Directory.CreateDirectory(localPath);
                    }
                    TransferOptions transferOptions = new TransferOptions();
            transferOptions.TransferMode = TransferMode.Binary;        

                TransferOperationResult transferResult;
                transferResult =
                    session.GetFiles(remotePath, localPath, false, transferOptions);
                // Throw on any error
                transferResult.Check();
                // Print results                
                foreach (TransferEventArgs transfer in transferResult.Transfers)
                {
                    Console.WriteLine("Download of {0} succeeded", transfer.FileName);                 
                }
         
            Console.WriteLine("End;;;;;; : ");
        }
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: {0}", e);           
        }
    }



}
}
