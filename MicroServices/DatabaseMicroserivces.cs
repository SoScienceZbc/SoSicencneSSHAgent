using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DatabaseService_Grpc;
using Grpc.Core;

namespace SoSicencneSSHAgent.MicroServices
{
    /// <summary>
    /// This is the thing that calls locahost for data from the sql database
    /// </summary>
    public class DatabaseMicroserivces : GrpcDatabaseProject.GrpcDatabaseProjectClient
    {
        #region Project
        public  Task<D_Project> GetProject(UserDbInfomation infomation, ServerCallContext context)
        {
            Console.WriteLine($"Host:{context.Host} called Method:{context.Method}");
            return null;
        }
        public  Task<intger> AddProject(ProjectUserInfomation infomation, ServerCallContext context)
        {
            Console.WriteLine($"Host:{context.Host} called Method:{context.Method}");
            return null;
        }
        public  Task<intger> EditProject(ProjectUserInfomation infomation, ServerCallContext context)
        {

            return null;
        }

        public  Task<intger> RemoveProject(ProjectUserInfomation infomation, ServerCallContext context)
        {
            return null;
        }

        public  Task<D_Projects> GetProjects(UserDbInfomation infomation, ServerCallContext context)
        {
            return null;
        }

        #endregion
        #region Docoment
        public  Task<D_Documents> GetDocuments(UserDbInfomation infomation, ServerCallContext context)
        {
            return null;
        }
        // documents
        public  Task<intger> AddDocument(D_Document infomation, ServerCallContext context)
        {
            return null;
        }
        public  Task<D_Document> GetDocument(UserDbInfomation infomation, ServerCallContext context)
        {
            return null;
        }
        public  Task<intger> UpdateDocument(D_Document infomation, ServerCallContext context)
        {
            return null;
        }

        public Task<intger> RemoveDocument(UserDbInfomation infomation,ServerCallContext context)
        {
            return null;
        }
        #endregion
        #region Remote
        public  Task<intger> AddRemoteFile(D_RemoteFile infomation, ServerCallContext context)
        {
            return null;
        }
        public  Task<D_RemoteFile> GetRemoteFile(UserDbInfomation infomation, ServerCallContext context)
        {
            return null;
        }
        public  Task<D_RemoteFile> UpdateRemoteFile(D_RemoteFile infomation, ServerCallContext context)
        {
            return null;
        }
        public  Task<intger> RemoveRemoteFile(UserDbInfomation infomation, ServerCallContext context)
        {
            return null;
        }
        public  Task<D_RemoteFiles> GetRemoteFiles(UserDbInfomation infomation, ServerCallContext context)
        {
            return null;
        }
        #endregion
    }
}
