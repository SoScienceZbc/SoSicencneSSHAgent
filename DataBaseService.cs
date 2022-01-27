using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseService_Grpc;
using SoSicencneSSHAgent.MicroServices;
using Grpc.Core;
using SoSicencneSSHAgent.jwt;

namespace GrpcServiceForAngular.Services.DataBase
{
    /// <summary>
    /// This is the thing that is the server for the azure server..
    /// </summary>
    public class DataBaseService : GrpcDatabaseProject.GrpcDatabaseProjectBase
    {
        private static DatabaseMicroserivces dbm = new DatabaseMicroserivces();

        #region Project
        public override Task<D_Project> GetProject(UserDbInfomation request, ServerCallContext context)
        {
            request.DbName = new JWTController().GetUsername(request.DbName);
            if (request.DbName != "")
                return Task.FromResult(dbm.GetProject(request).Result);
            return Task.FromResult(new D_Project());
        }
        public override Task<intger> AddProject(ProjectUserInfomation request, ServerCallContext context)
        {
            request.User.DbName = new JWTController().GetUsername(request.User.DbName);
            if (request.User.DbName != "")
                return Task.FromResult(dbm.AddProject(request).Result);
            return Task.FromResult(new intger() { Number = 0 });
        }
        public override Task<intger> EditProject(ProjectUserInfomation request, ServerCallContext context)
        {
            request.User.DbName = new JWTController().GetUsername(request.User.DbName);
            if (request.User.DbName != "")
                return Task.FromResult(dbm.EditProject(request).Result);
            return Task.FromResult(new intger() { Number = 0 });
        }
        public override Task<intger> RemoveProject(ProjectUserInfomation request, ServerCallContext context)
        {
            request.User.DbName = new JWTController().GetUsername(request.User.DbName);
            if (request.User.DbName != "")
                return Task.FromResult(dbm.RemoveProject(request).Result);
            return Task.FromResult(new intger() { Number = 0 });
        }
        public override Task<D_Projects> GetProjects(UserDbInfomation request, ServerCallContext context)
        {
            Console.WriteLine("Entered DataBaseService GetProjects");
            request.DbName = new JWTController().GetUsername(request.DbName);
            if (request.DbName != "")
            {
                Console.WriteLine("DbName request not empty");
                return Task.FromResult(dbm.GetProjects(request).Result);
            }
            Console.WriteLine("DbName request empty");
            return Task.FromResult(new D_Projects());
        }
        public override Task<intger> AddProjectMember(MemberInformation request, ServerCallContext context)
        {
            request.User.DbName = new JWTController().GetUsername(request.User.DbName);
            if (request.User.DbName != "")
                return Task.FromResult(dbm.AddProjectMember(request).Result);
            return Task.FromResult(new intger() { Number = 0 });
        }
        public override Task<intger> RemoveProjectMember(MemberInformation request, ServerCallContext context)
        {
            request.User.DbName = new JWTController().GetUsername(request.User.DbName);
            if (request.User.DbName != "")
                return Task.FromResult(dbm.RemoveProjectMember(request).Result);
            return Task.FromResult(new intger() { Number = 0 });
        }
        #endregion
        #region Docoment
        public override Task<D_Documents> GetDocuments(UserDbInfomation request, ServerCallContext context)
        {
            request.DbName = new JWTController().GetUsername(request.DbName);
            if (request.DbName != "")
                return Task.FromResult(dbm.GetDocuments(request).Result);
            return Task.FromResult(new D_Documents());
        }
        public override Task<intger> AddDocument(D_Document request, ServerCallContext context)
        {
            return Task.FromResult(dbm.AddDocument(request).Result);
        }
        public override Task<D_Document> GetDocument(UserDbInfomation request, ServerCallContext context)
        {
            return Task.FromResult(dbm.GetDocument(request).Result);
        }
        public override Task<intger> UpdateDocument(D_Document request, ServerCallContext context)
        {
            return Task.FromResult(dbm.UpdateDocument(request).Result);
        }
        public override Task<intger> RemoveDocument(ProjectUserInfomation request, ServerCallContext context)
        {
            request.User.DbName = new JWTController().GetUsername(request.User.DbName);
            if (request.User.DbName != "")
                return Task.FromResult(dbm.RemoveDocument(request).Result);
            return Task.FromResult(new intger() { Number = 0 });
        }
        #endregion
        #region Remote
        public override Task<intger> AddRemoteFile(D_RemoteFile request, ServerCallContext context)
        {
            return Task.FromResult(dbm.AddRemoteFile(request).Result);
        }
        public override Task<D_RemoteFile> GetRemoteFile(UserDbInfomation request, ServerCallContext context)
        {
            request.DbName = new JWTController().GetUsername(request.DbName);
            if (request.DbName != "")
                return Task.FromResult(dbm.GetRemoteFile(request).Result);
            return Task.FromResult(new D_RemoteFile());
        }
        public override Task<D_RemoteFile> UpdateRemoteFile(D_RemoteFile request, ServerCallContext context)
        {
            return Task.FromResult(dbm.UpdateRemoteFile(request).Result);
        }
        public override Task<intger> RemoveRemoteFile(UserDbInfomation request, ServerCallContext context)
        {
            request.DbName = new JWTController().GetUsername(request.DbName);
            if (request.DbName != "")
                return Task.FromResult(dbm.RemoveRemoteFile(request).Result);
            return Task.FromResult(new intger() { Number = 0 });
        }
        public override Task<D_RemoteFiles> GetRemoteFiles(UserDbInfomation request, ServerCallContext context)
        {
            request.DbName = new JWTController().GetUsername(request.DbName);
            if (request.DbName != "")
                return Task.FromResult(dbm.GetRemoteFiles(request).Result);
            return Task.FromResult(new D_RemoteFiles());
        }
        #endregion
        #region Subject
        public override Task<intger> AddSubject(D_Subject request, ServerCallContext context)
        {
            if (new JWTController().ValidateRoleLevel(request.User.DbName,RoleType.teacher))
            {
                return Task.FromResult(dbm.AddSubject(request).Result);
            }
            return Task.FromResult(new intger() { Number = 0 });
        }
        public override Task<D_Subjects> GetSubjects(UserDbInfomation request, ServerCallContext context)
        {
            if (new JWTController().ValidateRoleLevel(request.DbName, RoleType.user))
            {
                return Task.FromResult(dbm.GetSubjects(request).Result);
            }
            return Task.FromResult(new D_Subjects());
        }
        #endregion.
        #region project Theme
        public override Task<intger> AddProjectTheme(D_ProjectTheme request, ServerCallContext context)
        {
            if (new JWTController().ValidateRoleLevel(request.Teacher, RoleType.teacher))
            {
                request.Teacher = new JWTController().GetUsername(request.Teacher);
                return Task.FromResult(dbm.AddProjectTheme(request).Result);
            }
            NotValid();
            return Task.FromResult(new intger() { Number = 0 });
        }
        public override Task<D_ProjectThemes> GetProjectThemes(UserDbInfomation request, ServerCallContext context)
        {
            if (new JWTController().ValidateRoleLevel(request.DbName, RoleType.teacher))
            {
                request.DbName = new JWTController().GetUsername(request.DbName);
                return Task.FromResult(dbm.GetProjectThemes(request).Result);
            }
            NotValid();
            return Task.FromResult(new D_ProjectThemes());
        }
        public override Task<D_ProjectThemes> GetProjectThemesFromSubject(ThemeFromSubject request, ServerCallContext context)
        {
            request.User.DbName = new JWTController().GetUsername(request.User.DbName);
            if (!string.IsNullOrEmpty(request.User.DbName))
            {
                return Task.FromResult(dbm.GetProjectThemesFromSubject(request).Result);
            }
            NotValid();
            return Task.FromResult(new D_ProjectThemes());
        }
        public override Task<intger> AddProjectThemeCoTeacher(ProjectThemeUserInfomation request, ServerCallContext context)
        {
            if (new JWTController().ValidateRoleLevel(request.User.DbName, RoleType.teacher))
            {
                request.User.DbName = new JWTController().GetUsername(request.User.DbName);
                return Task.FromResult(dbm.AddProjectThemeCoTeacher(request).Result);
            }
            NotValid();
            return Task.FromResult(new intger() { Number = 0 });
        }
        public override Task<intger> RemoveProjectTheme(ProjectThemeUserInfomation request, ServerCallContext context)
        {
            if (new JWTController().ValidateRoleLevel(request.User.DbName, RoleType.teacher))
            {
                request.User.DbName = new JWTController().GetUsername(request.User.DbName);
                return Task.FromResult(dbm.RemoveProjectTheme(request).Result);
            }
            NotValid();
            return Task.FromResult(new intger() { Number = 0 });
        }
        public override Task<intger> RemoveProjectThemeCoTeacher(ProjectThemeUserInfomation request, ServerCallContext context)
        {
            if (new JWTController().ValidateRoleLevel(request.User.DbName, RoleType.teacher))
            {
                request.User.DbName = new JWTController().GetUsername(request.User.DbName);
                return Task.FromResult(dbm.RemoveProjectThemeCoTeacher(request).Result);
            }
            NotValid();
            return Task.FromResult(new intger() { Number = 0 });
        }
        #endregion

        private void NotValid()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("NOT Valid Token");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
