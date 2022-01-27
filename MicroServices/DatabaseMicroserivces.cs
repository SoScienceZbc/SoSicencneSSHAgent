using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using DatabaseService_Grpc;
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;

namespace SoSicencneSSHAgent.MicroServices
{
    /// <summary>
    /// This is the thing that calls locahost for data from the sql database
    /// </summary>
    public class DatabaseMicroserivces
    {
        static GrpcDatabaseProject.GrpcDatabaseProjectClient client;
        //This is a hashed serial used in DangerousServerCertificateCustomValidationCallback() to validate the server certificate.
        private string HashedSerial { get; } = File.ReadAllText(Directory.GetCurrentDirectory() + "/HashedSerial.txt");
        public DatabaseMicroserivces()
        {
            //AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);

            if (client == null)
            {
                client = CreateGrpcClient("https://localhost:48041");
            }
        }
        /// <summary>
        /// Sets up a Proto client and channel for making Grpc calls to the Database service
        /// </summary>
        /// <param name="channelURL"></param>
        /// <returns></returns>
        private GrpcDatabaseProject.GrpcDatabaseProjectClient CreateGrpcClient(string channelURL)
        {
            HttpClientHandler http_handler = new HttpClientHandler();

            http_handler.ServerCertificateCustomValidationCallback = DangerousServerCertificateCustomValidationCallback;

            GrpcWebHandler handler = new GrpcWebHandler(GrpcWebMode.GrpcWebText, http_handler);

            GrpcDatabaseProject.GrpcDatabaseProjectClient client = new GrpcDatabaseProject.GrpcDatabaseProjectClient(
                GrpcChannel.ForAddress(new Uri(channelURL),
                new GrpcChannelOptions
                {
                    HttpClient = new HttpClient(handler),
                    Credentials = new SslCredentials()
                }));

            return client;
        }
        /// <summary>
        /// This checks whether the server's certificate should be trusted or not. More of a workaround. Implement a better way when possible.
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <param name="arg4"></param>
        /// <returns></returns>
        private bool DangerousServerCertificateCustomValidationCallback(HttpRequestMessage arg1, X509Certificate2 arg2, X509Chain arg3, SslPolicyErrors arg4)
        {
            /*
            //Used for debugging purposes
            Console.WriteLine("\n******CertificateResponse******\n");
            Console.WriteLine(arg1);
            Console.WriteLine("X509Certificate2:");
            Console.WriteLine(arg2);
            Console.WriteLine("X509Chain:");
            Console.WriteLine(arg3);
            Console.WriteLine("SslPolicyErrors:");
            Console.WriteLine(arg4);
            Console.WriteLine("\n******CertificateResponse******\n");
            */

            Sha256Hasher hasher = new Sha256Hasher();
            string serverSerialHashed = hasher.HashString(arg2.SerialNumber);

            //Compares the received serial with the expected serial
            if (serverSerialHashed.ToLower() == HashedSerial.ToLower())
                return true;
            else
                return false;
        }

        #region Project
        public Task<D_Project> GetProject(UserDbInfomation information)
        {
            Console.WriteLine("Returning GetProject");
            return Task.FromResult(client.GetProject(information));
        }
        public Task<intger> AddProject(ProjectUserInfomation information)
        {
            Console.WriteLine("Returning AddProject");
            return Task.FromResult(client.AddProject(information));
        }
        public Task<intger> EditProject(ProjectUserInfomation information)
        {
            Console.WriteLine("Returning EditProject");
            return Task.FromResult(client.EditProject(information));
        }
        public Task<intger> RemoveProject(ProjectUserInfomation information)
        {
            return Task.FromResult(client.RemoveProject(information));
        }
        public Task<D_Projects> GetProjects(UserDbInfomation information)
        {
            Console.WriteLine("Returning GetProjects");
            return Task.FromResult(client.GetProjects(information));
        }
        public Task<intger> AddProjectMember(MemberInformation information)
        {
            return Task.FromResult(client.AddProjectMember(information));
        }
        public Task<intger> RemoveProjectMember(MemberInformation information)
        {
            return Task.FromResult(client.RemoveProjectMember(information));
        }
        #endregion
        #region Docoment
        public Task<D_Documents> GetDocuments(UserDbInfomation information)
        {
            return Task.FromResult(client.GetDocuments(information));
        }
        // documents
        public Task<intger> AddDocument(D_Document information)
        {
            return Task.FromResult(client.AddDocument(information));
        }
        public Task<D_Document> GetDocument(UserDbInfomation information)
        {
            return Task.FromResult(client.GetDocument(information));
        }
        public Task<intger> UpdateDocument(D_Document information)
        {
            return Task.FromResult(client.UpdateDocument(information));
        }

        public Task<intger> RemoveDocument(ProjectUserInfomation information)
        {
            return Task.FromResult(client.RemoveDocument(information));
        }
        #endregion
        #region Remote
        public Task<intger> AddRemoteFile(D_RemoteFile information)
        {
            return Task.FromResult(client.AddRemoteFile(information));
        }
        public Task<D_RemoteFile> GetRemoteFile(UserDbInfomation information)
        {
            return Task.FromResult(client.GetRemoteFile(information));
        }
        public Task<D_RemoteFile> UpdateRemoteFile(D_RemoteFile information)
        {
            return Task.FromResult(client.UpdateRemoteFile(information));
        }
        public Task<intger> RemoveRemoteFile(UserDbInfomation information)
        {
            return Task.FromResult(client.RemoveRemoteFile(information));
        }
        public Task<D_RemoteFiles> GetRemoteFiles(UserDbInfomation information)
        {
            return Task.FromResult(client.GetRemoteFiles(information));
        }
        #endregion
        #region Teacher
        public Task<D_Teacher> CheckAndInsertTeacher(D_Teacher information)
        {
            Console.WriteLine("Returning CheckAndInsertTeacher");
                return Task.FromResult(client.CheckAndInsertTeacher(information));
        }
        #endregion
        #region Subject
        public Task<intger> AddSubject(D_Subject subject)
        {
            return Task.FromResult(client.AddSubject(subject));
        }
        public Task<D_Subjects> GetSubjects(UserDbInfomation information)
        {
            return Task.FromResult(client.GetSubjects(information));
        }
        #endregion
        #region Project Theme
        public Task<intger> AddProjectTheme(D_ProjectTheme theme)
        {
            return Task.FromResult(client.AddProjectTheme(theme));
        }
        public Task<D_ProjectThemes> GetProjectThemes(UserDbInfomation information)
        {
            return Task.FromResult(client.GetProjectThemes(information));
        }
        public Task<D_ProjectThemes> GetProjectThemesFromSubject(ThemeFromSubject information)
        {
            return Task.FromResult(client.GetProjectThemesFromSubject(information));
        }
        public Task<intger> AddProjectThemeCoTeacher(ProjectThemeUserInfomation information)
        {
            return Task.FromResult(client.AddProjectThemeCoTeacher(information));
        }
        public Task<intger> RemoveProjectTheme(ProjectThemeUserInfomation information)
        {
            return Task.FromResult(client.RemoveProjectTheme(information));
        }
        public Task<intger> RemoveProjectThemeCoTeacher(ProjectThemeUserInfomation information)
        {
            return Task.FromResult(client.RemoveProjectThemeCoTeacher(information));
        }
        #endregion
    }
}
