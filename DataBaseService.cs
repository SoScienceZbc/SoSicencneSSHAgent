using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseService_Grpc;
using SoSicencneSSHAgent.MicroServices;
using Grpc.Core;

namespace GrpcServiceForAngular.Services.DataBase
{
    /// <summary>
    /// This is the thing that is the server for the azure server..
    /// </summary>
    public class DataBaseService : GrpcDatabaseProject.GrpcDatabaseProjectBase
    {
        //This is the Angular Server side og the application.

        #region Project
        public override Task<D_Project> GetProject(UserDbInfomation infomation, ServerCallContext context)
        {
            Console.WriteLine($"Host:{context.Host} called Method:{context.Method}");
            return Task.FromResult(new DatabaseMicroserivces().GetProject(infomation).Result);
        }
        public override Task<intger> AddProject(ProjectUserInfomation infomation, ServerCallContext context)
        {
            Console.WriteLine($"Host:{context.Host} called Method:{context.Method}");
            return Task.FromResult(new DatabaseMicroserivces().AddProject(infomation).Result);
        }
        public override Task<intger> EditProject(ProjectUserInfomation infomation, ServerCallContext context)
        {

            return Task.FromResult(new DatabaseMicroserivces().EditProject(infomation).Result);
        }

        public override Task<intger> RemoveProject(ProjectUserInfomation infomation, ServerCallContext context)
        {
            return Task.FromResult(new DatabaseMicroserivces().RemoveProject(infomation).Result);
        }

        public override Task<D_Projects> GetProjects(UserDbInfomation infomation, ServerCallContext context)
        {
            return Task.FromResult(new DatabaseMicroserivces().GetProjects(infomation).Result);
        }

        #endregion
        #region Docoment
        public override Task<D_Documents> GetDocuments(UserDbInfomation infomation, ServerCallContext context)
        {
            return Task.FromResult(new DatabaseMicroserivces().GetDocuments(infomation).Result);
        }
        // documents
        public override Task<intger> AddDocument(D_Document infomation, ServerCallContext context)
        {
            return Task.FromResult(new DatabaseMicroserivces().AddDocument(infomation).Result);
        }
        public override Task<D_Document> GetDocument(UserDbInfomation infomation, ServerCallContext context)
        {
            return Task.FromResult(new DatabaseMicroserivces().GetDocument(infomation).Result);
        }
        public override Task<intger> UpdateDocument(D_Document infomation, ServerCallContext context)
        {
            return Task.FromResult(new DatabaseMicroserivces().UpdateDocument(infomation).Result);
        }

        public override Task<intger> RemoveDocument(ProjectUserInfomation infomation,ServerCallContext context)
        {
            return Task.FromResult(new DatabaseMicroserivces().RemoveDocument(infomation).Result);
        }
        #endregion
        #region Remote
        public override Task<intger> AddRemoteFile(D_RemoteFile infomation, ServerCallContext context)
        {
            return Task.FromResult(new DatabaseMicroserivces().AddRemoteFile(infomation).Result);
        }
        public override Task<D_RemoteFile> GetRemoteFile(UserDbInfomation infomation, ServerCallContext context)
        {
            return Task.FromResult(new DatabaseMicroserivces().GetRemoteFile(infomation).Result);
        }
        public override Task<D_RemoteFile> UpdateRemoteFile(D_RemoteFile infomation, ServerCallContext context)
        {
            return Task.FromResult(new DatabaseMicroserivces().UpdateRemoteFile(infomation).Result);
        }
        public override Task<intger> RemoveRemoteFile(UserDbInfomation infomation, ServerCallContext context)
        {
            return Task.FromResult(new DatabaseMicroserivces().RemoveRemoteFile(infomation).Result);
        }
        public override Task<D_RemoteFiles> GetRemoteFiles(UserDbInfomation infomation, ServerCallContext context)
        {
            return Task.FromResult(new DatabaseMicroserivces().GetRemoteFiles(infomation).Result);
        }
        #endregion

    }
}
