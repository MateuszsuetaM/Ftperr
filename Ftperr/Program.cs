using System.Net;
using System.Reflection;
using WinSCP;

//    public partial class ScriptMain : VSTARTScriptObjectModelBase
    class Program 
    {
        static void Main(string[] args)
        {

            DeleteFTPDirectory("w", "ftp.noterr.fajnit.pl/", "mateusz2-ftp", "fTp_f4jn1t!");
            // Generate random, yet meaningful name of the temporary file
            /*            string tempName = Path.GetTempFileName();
                        string executableName = "WinSCP." + Path.ChangeExtension(Path.GetFileName(tempName), "exe");
                        string executablePath = Path.Combine(Path.GetDirectoryName(tempName), executableName);
                        File.Delete(tempName);

                        // Extract the resource to the temporary file
                        Assembly executingAssembly = Assembly.GetExecutingAssembly();
                        string resName = executingAssembly.GetName().Name + "." + "WinSCP.exe";

                        using (Stream resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(resName))
                        using (Stream file = File.Create(executablePath))
                        {
                            resource.CopyTo(file);
                        }

                        try
                        {
                            using (Session session = new Session())
                            {
                                // Use the temporarily extracted executable
                                session.ExecutablePath = executablePath;

                                // Connect
                                session.Open(sessionOptions);

                                // Your code
                            }
                        }
                        finally
                        {
                            // Clean up
                            File.Delete(executablePath);
                        }
            */
        }
        public static List<string> DirectoryListing(string Path, string ServerAdress, string Login, string Password)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + ServerAdress + Path);
            request.Credentials = new NetworkCredential(Login, Password);

            request.Method = WebRequestMethods.Ftp.ListDirectory;

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);

            List<string> result = new List<string>();

            while (!reader.EndOfStream)
            {
                result.Add(reader.ReadLine());
            }

            reader.Close();
            response.Close();

            return result;
        }
        public static void DeleteFTPFile(string Path, string ServerAdress, string Login, string Password)
        {
            FtpWebRequest clsRequest = (System.Net.FtpWebRequest)WebRequest.Create("ftp://" + ServerAdress + Path);
            clsRequest.Credentials = new System.Net.NetworkCredential(Login, Password);

            clsRequest.Method = WebRequestMethods.Ftp.RemoveDirectory;
            //clsRequest.Method = WebRequestMethods.Ftp.DeleteFile;

        string result = string.Empty;
            FtpWebResponse response = (FtpWebResponse)clsRequest.GetResponse();
            long size = response.ContentLength;
            Stream datastream = response.GetResponseStream();
            StreamReader sr = new StreamReader(datastream);
            result = sr.ReadToEnd();
            sr.Close();
            datastream.Close();
            response.Close();
        }
        public static void DeleteFTPDirectory(string Path, string ServerAdress, string Login, string Password)
        {
            FtpWebRequest clsRequest = (System.Net.FtpWebRequest)WebRequest.Create("ftp://" + ServerAdress + Path);
            clsRequest.Credentials = new System.Net.NetworkCredential(Login, Password);

            List<string> filesList = DirectoryListing(Path, ServerAdress, Login, Password);

            foreach (string file in filesList)
            {
                DeleteFTPFile(file, ServerAdress, Login, Password);
                //DeleteFTPFile(Path + file, ServerAdress, Login, Password);
            }

            clsRequest.Method = WebRequestMethods.Ftp.RemoveDirectory;

            string result = string.Empty;
            FtpWebResponse response = (FtpWebResponse)clsRequest.GetResponse();
            long size = response.ContentLength;
            Stream datastream = response.GetResponseStream();
            StreamReader sr = new StreamReader(datastream);
            result = sr.ReadToEnd();
            sr.Close();
            datastream.Close();
            response.Close();
        }
    }
