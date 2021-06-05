using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseService_Grpc;
using SoSicencneSSHAgent.MicroServices;
using Grpc.Core;

namespace GrpcServiceForAngular.Services
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
            return Task.FromResult(new DatabaseMicroserivces().GetProject(infomation));
        }
        public override Task<intger> AddProject(ProjectUserInfomation infomation, ServerCallContext context)
        {
            Console.WriteLine($"Host:{context.Host} called Method:{context.Method}");
            return Task.FromResult(new DatabaseMicroserivces().AddProject(infomation));
        }
        public override Task<intger> EditProject(ProjectUserInfomation infomation, ServerCallContext context)
        {

            return Task.FromResult(new DatabaseMicroserivces().EditProject(infomation));
        }

        public override Task<intger> RemoveProject(ProjectUserInfomation infomation, ServerCallContext context)
        {
            return Task.FromResult(new DatabaseMicroserivces().RemoveProject(infomation));
        }

        public override Task<D_Projects> GetProjects(UserDbInfomation infomation, ServerCallContext context)
        {
            return Task.FromResult(new DatabaseMicroserivces().GetProjects(infomation));
        }

        #endregion
        #region Docoment
        public override Task<D_Documents> GetDocuments(UserDbInfomation infomation, ServerCallContext context)
        {
            return Task.FromResult(new DatabaseMicroserivces().GetDocuments(infomation));
        }
        // documents
        public override Task<intger> AddDocument(D_Document infomation, ServerCallContext context)
        {
            return Task.FromResult(new DatabaseMicroserivces().AddDocument(infomation));
        }
        public override Task<D_Document> GetDocument(UserDbInfomation infomation, ServerCallContext context)
        {
            return Task.FromResult(new DatabaseMicroserivces().GetDocument(infomation));
        }
        public override Task<intger> UpdateDocument(D_Document infomation, ServerCallContext context)
        {
            return Task.FromResult(new DatabaseMicroserivces().UpdateDocument(infomation));
        }

        public override Task<intger> RemoveDocument(UserDbInfomation infomation,ServerCallContext context)
        {
            return Task.FromResult(new DatabaseMicroserivces().RemoveDocument(infomation));
        }
        #endregion
        #region Remote
        public override Task<intger> AddRemoteFile(D_RemoteFile infomation, ServerCallContext context)
        {
            return Task.FromResult(new DatabaseMicroserivces().AddRemoteFile(infomation));
        }
        public override Task<D_RemoteFile> GetRemoteFile(UserDbInfomation infomation, ServerCallContext context)
        {
            return Task.FromResult(new DatabaseMicroserivces().GetRemoteFile(infomation));
        }
        public override Task<D_RemoteFile> UpdateRemoteFile(D_RemoteFile infomation, ServerCallContext context)
        {
            return Task.FromResult(new DatabaseMicroserivces().UpdateRemoteFile(infomation));
        }
        public override Task<intger> RemoveRemoteFile(UserDbInfomation infomation, ServerCallContext context)
        {
            return Task.FromResult(new DatabaseMicroserivces().RemoveRemoteFile(infomation));
        }
        public override Task<D_RemoteFiles> GetRemoteFiles(UserDbInfomation infomation, ServerCallContext context)
        {
            return Task.FromResult(new DatabaseMicroserivces().GetRemoteFiles(infomation));
        }
        #endregion

    }
}
