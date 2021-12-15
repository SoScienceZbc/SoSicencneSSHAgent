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
            {
                channel = new GrpcDatabaseProject.GrpcDatabaseProjectClient(GrpcChannel.ForAddress("http://localhost:48041", new GrpcChannelOptions
                {
                    HttpClient = new HttpClient(handler),
                    Credentials = ChannelCredentials.Insecure
                }));
            }
                

        }

        #region Project
        public Task<D_Project> GetProject(UserDbInfomation information)
        {
            Console.WriteLine("Returning GetProject");
            return Task.FromResult(channel.GetProject(information));
        }
        public Task<intger> AddProject(ProjectUserInfomation information)
        {
            Console.WriteLine("Returning AddProject");
            return Task.FromResult(channel.AddProject(information));
        }
        public Task<intger> EditProject(ProjectUserInfomation information)
        {
            Console.WriteLine("Returning EditProject");
            return Task.FromResult(channel.EditProject(information));
        }
        public Task<intger> RemoveProject(ProjectUserInfomation information)
        {
            return Task.FromResult(channel.RemoveProject(information));
        }
        public Task<D_Projects> GetProjects(UserDbInfomation information)
        {
            Console.WriteLine("Returning GetProjects");
            return Task.FromResult(channel.GetProjects(information));
        }
        public Task<intger> AddProjectMember(MemberInformation information)
        {
            return Task.FromResult(channel.AddProjectMember(information));
        }
        public Task<intger> RemoveProjectMember(MemberInformation information)
        {
            return Task.FromResult(channel.RemoveProjectMember(information));
        }
        #endregion
        #region Docoment
        public Task<D_Documents> GetDocuments(UserDbInfomation information)
        {
            return Task.FromResult(channel.GetDocuments(information));
        }
        // documents
        public Task<intger> AddDocument(D_Document information)
        {
            return Task.FromResult(channel.AddDocument(information));
        }
        public Task<D_Document> GetDocument(UserDbInfomation information)
        {
            return Task.FromResult(channel.GetDocument(information));
        }
        public Task<intger> UpdateDocument(D_Document information)
        {
            return Task.FromResult(channel.UpdateDocument(information));
        }

        public Task<intger> RemoveDocument(ProjectUserInfomation information)
        {
            return Task.FromResult(channel.RemoveDocument(information));
        }
        #endregion
        #region Remote
        public Task<intger> AddRemoteFile(D_RemoteFile information)
        {
            return Task.FromResult(channel.AddRemoteFile(information));
        }
        public Task<D_RemoteFile> GetRemoteFile(UserDbInfomation information)
        {
            return Task.FromResult(channel.GetRemoteFile(information));
        }
        public Task<D_RemoteFile> UpdateRemoteFile(D_RemoteFile information)
        {
            return Task.FromResult(channel.UpdateRemoteFile(information));
        }
        public Task<intger> RemoveRemoteFile(UserDbInfomation information)
        {
            return Task.FromResult(channel.RemoveRemoteFile(information));
        }
        public Task<D_RemoteFiles> GetRemoteFiles(UserDbInfomation information)
        {
            return Task.FromResult(channel.GetRemoteFiles(information));
        }
        #endregion
        #region Teacher
        public Task<D_Teacher> CheckAndInsertTeacher(D_Teacher information)
        {
            Console.WriteLine("Returning CheckAndInsertTeacher");
                return Task.FromResult(channel.CheckAndInsertTeacher(information));
        }
        #endregion
        #region Subject
        public Task<intger> AddSubject(D_Subject subject)
        {
            return Task.FromResult(channel.AddSubject(subject));
        }
        public Task<D_Subjects> GetSubjects(UserDbInfomation information)
        {
            return Task.FromResult(channel.GetSubjects(information));
        }
        #endregion
        #region Project Theme
        public Task<intger> AddProjectTheme(D_ProjectTheme theme)
        {
            return Task.FromResult(channel.AddProjectTheme(theme));
        }
        public Task<D_ProjectThemes> GetProjectThemes(UserDbInfomation information)
        {
            return Task.FromResult(channel.GetProjectThemes(information));
        }
        public Task<D_ProjectThemes> GetProjectThemesFromSubject(ThemeFromSubject information)
        {
            return Task.FromResult(channel.GetProjectThemesFromSubject(information));
        }
        public Task<intger> AddProjectThemeCoTeacher(ProjectThemeUserInfomation information)
        {
            return Task.FromResult(channel.AddProjectThemeCoTeacher(information));
        }
        public Task<intger> RemoveProjectTheme(ProjectThemeUserInfomation information)
        {
            return Task.FromResult(channel.RemoveProjectTheme(information));
        }
        public Task<intger> RemoveProjectThemeCoTeacher(ProjectThemeUserInfomation information)
        {
            return Task.FromResult(channel.RemoveProjectThemeCoTeacher(information));
        }
        #endregion
    }
}
