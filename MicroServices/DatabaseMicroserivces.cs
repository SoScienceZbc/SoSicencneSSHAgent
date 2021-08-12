using System;
using System.Collections.Generic;
using System.Net.Http;
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
        static GrpcDatabaseProject.GrpcDatabaseProjectClient channel;
        static GrpcWebHandler handler = new GrpcWebHandler(GrpcWebMode.GrpcWebText, new HttpClientHandler());
        public DatabaseMicroserivces()
        {
            if (channel == null)
                channel = new GrpcDatabaseProject.GrpcDatabaseProjectClient(GrpcChannel.ForAddress("http://localhost:48041", new GrpcChannelOptions { 
                    HttpClient = new HttpClient(handler), 
                    Credentials = ChannelCredentials.Insecure }));

        }

        #region Project
        public Task<D_Project> GetProject(UserDbInfomation infomation)
        {
            Console.WriteLine("Returning GetProject");
            return Task.FromResult(channel.GetProject(infomation));
        }
        public Task<intger> AddProject(ProjectUserInfomation infomation)
        {
            Console.WriteLine("Returning AddProject");
            return Task.FromResult(channel.AddProject(infomation));
        }

        public Task<intger> EditProject(ProjectUserInfomation infomation)
        {
            Console.WriteLine("Returning EditProject");
            return Task.FromResult(channel.EditProject(infomation));
        }

        public Task<intger> RemoveProject(ProjectUserInfomation infomation)
        {
            return Task.FromResult(channel.RemoveProject(infomation));
        }

        public Task<D_Projects> GetProjects(UserDbInfomation infomation)
        {
            Console.WriteLine("Returning GetProjects");
            return Task.FromResult(channel.GetProjects(infomation));
        }

        #endregion
        #region Docoment
        public Task<D_Documents> GetDocuments(UserDbInfomation infomation)
        {
            return Task.FromResult(channel.GetDocuments(infomation));
        }
        // documents
        public Task<intger> AddDocument(D_Document infomation)
        {
            return Task.FromResult(channel.AddDocument(infomation));
        }
        public Task<D_Document> GetDocument(UserDbInfomation infomation)
        {
            return Task.FromResult(channel.GetDocument(infomation));
        }
        public Task<intger> UpdateDocument(D_Document infomation)
        {
            return Task.FromResult(channel.UpdateDocument(infomation));
        }

        public Task<intger> RemoveDocument(ProjectUserInfomation infomation)
        {
            return Task.FromResult(channel.RemoveDocument(infomation));
        }
        #endregion
        #region Remote
        public Task<intger> AddRemoteFile(D_RemoteFile infomation)
        {
            return Task.FromResult(channel.AddRemoteFile(infomation));
        }
        public Task<D_RemoteFile> GetRemoteFile(UserDbInfomation infomation)
        {
            return Task.FromResult(channel.GetRemoteFile(infomation));
        }
        public Task<D_RemoteFile> UpdateRemoteFile(D_RemoteFile infomation)
        {
            return Task.FromResult(channel.UpdateRemoteFile(infomation));
        }
        public Task<intger> RemoveRemoteFile(UserDbInfomation infomation)
        {
            return Task.FromResult(channel.RemoveRemoteFile(infomation));
        }
        public Task<D_RemoteFiles> GetRemoteFiles(UserDbInfomation infomation)
        {
            return Task.FromResult(channel.GetRemoteFiles(infomation));
        }
        #endregion
        #region Teacher
        public Task<D_Teacher> CheckAndInsertTeacher(D_Teacher infomation)
        {
            Console.WriteLine("Returning CheckAndInsertTeacher");
                return Task.FromResult(channel.CheckAndInsertTeacher(infomation));
        }
        #endregion
        #region Subject
        public Task<intger> AddSubject(D_Subject subject)
        {
            return Task.FromResult(channel.AddSubject(subject));
        }

        public Task<D_Subjects> GetSubjects(UserDbInfomation infomation)
        {
            return Task.FromResult(channel.GetSubjects(infomation));
        }
        #endregion
        #region Project Theme
        public Task<intger> AddProjectTheme(D_ProjectTheme theme)
        {
            return Task.FromResult(channel.AddProjectTheme(theme));
        }
        public Task<D_ProjectThemes> GetProjectThemes(UserDbInfomation infomation)
        {
            return Task.FromResult(channel.GetProjectThemes(infomation));
        }
        public Task<D_ProjectThemes> GetProjectThemesFromSubject(ThemeFromSubject infomation)
        {
            return Task.FromResult(channel.GetProjectThemesFromSubject(infomation));
        }
        #endregion
    }
}
