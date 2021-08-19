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
        //This is the Angular Server side og the application.

        #region Project
        public override Task<D_Project> GetProject(UserDbInfomation request, ServerCallContext context)
        {
            request.DbName = new JWTController().GetUsername(request.DbName);
            if (request.DbName != "")
                return Task.FromResult(new DatabaseMicroserivces().GetProject(request).Result);
            return Task.FromResult(new D_Project());
        }
        public override Task<intger> AddProject(ProjectUserInfomation request, ServerCallContext context)
        {
            request.User.DbName = new JWTController().GetUsername(request.User.DbName);
            if (request.User.DbName != "")
                return Task.FromResult(new DatabaseMicroserivces().AddProject(request).Result);
            return Task.FromResult(new intger() { Number = 0 });
        }
        public override Task<intger> EditProject(ProjectUserInfomation request, ServerCallContext context)
        {
            request.User.DbName = new JWTController().GetUsername(request.User.DbName);
            if (request.User.DbName != "")
                return Task.FromResult(new DatabaseMicroserivces().EditProject(request).Result);
            return Task.FromResult(new intger() { Number = 0 });
        }
        public override Task<intger> RemoveProject(ProjectUserInfomation request, ServerCallContext context)
        {
            request.User.DbName = new JWTController().GetUsername(request.User.DbName);
            if (request.User.DbName != "")
                return Task.FromResult(new DatabaseMicroserivces().RemoveProject(request).Result);
            return Task.FromResult(new intger() { Number = 0 });
        }
        public override Task<D_Projects> GetProjects(UserDbInfomation request, ServerCallContext context)
        {
            request.DbName = new JWTController().GetUsername(request.DbName);
            if (request.DbName != "")
                return Task.FromResult(new DatabaseMicroserivces().GetProjects(request).Result);
            return Task.FromResult(new D_Projects());
        }
        public override Task<intger> AddProjectMember(MemberInformation request, ServerCallContext context)
        {
            request.User.DbName = new JWTController().GetUsername(request.User.DbName);
            if (request.User.DbName != "")
                return Task.FromResult(new DatabaseMicroserivces().AddProjectMember(request).Result);
            return Task.FromResult(new intger() { Number = 0 });
        }
        public override Task<intger> RemoveProjectMember(MemberInformation request, ServerCallContext context)
        {
            request.User.DbName = new JWTController().GetUsername(request.User.DbName);
            if (request.User.DbName != "")
                return Task.FromResult(new DatabaseMicroserivces().RemoveProjectMember(request).Result);
            return Task.FromResult(new intger() { Number = 0 });
        }
        #endregion
        #region Docoment
        public override Task<D_Documents> GetDocuments(UserDbInfomation request, ServerCallContext context)
        {
            request.DbName = new JWTController().GetUsername(request.DbName);
            if (request.DbName != "")
                return Task.FromResult(new DatabaseMicroserivces().GetDocuments(request).Result);
            return Task.FromResult(new D_Documents());
        }
        public override Task<intger> AddDocument(D_Document request, ServerCallContext context)
        {
            return Task.FromResult(new DatabaseMicroserivces().AddDocument(request).Result);
        }
        public override Task<D_Document> GetDocument(UserDbInfomation request, ServerCallContext context)
        {
            return Task.FromResult(new DatabaseMicroserivces().GetDocument(request).Result);
        }
        public override Task<intger> UpdateDocument(D_Document request, ServerCallContext context)
        {
            return Task.FromResult(new DatabaseMicroserivces().UpdateDocument(request).Result);
        }
        public override Task<intger> RemoveDocument(ProjectUserInfomation request, ServerCallContext context)
        {
            request.User.DbName = new JWTController().GetUsername(request.User.DbName);
            if (request.User.DbName != "")
                return Task.FromResult(new DatabaseMicroserivces().RemoveDocument(request).Result);
            return Task.FromResult(new intger() { Number = 0 });
        }
        #endregion
        #region Remote
        public override Task<intger> AddRemoteFile(D_RemoteFile request, ServerCallContext context)
        {
            return Task.FromResult(new DatabaseMicroserivces().AddRemoteFile(request).Result);
        }
        public override Task<D_RemoteFile> GetRemoteFile(UserDbInfomation request, ServerCallContext context)
        {
            request.DbName = new JWTController().GetUsername(request.DbName);
            if (request.DbName != "")
                return Task.FromResult(new DatabaseMicroserivces().GetRemoteFile(request).Result);
            return Task.FromResult(new D_RemoteFile());
        }
        public override Task<D_RemoteFile> UpdateRemoteFile(D_RemoteFile request, ServerCallContext context)
        {
            return Task.FromResult(new DatabaseMicroserivces().UpdateRemoteFile(request).Result);
        }
        public override Task<intger> RemoveRemoteFile(UserDbInfomation request, ServerCallContext context)
        {
            request.DbName = new JWTController().GetUsername(request.DbName);
            if (request.DbName != "")
                return Task.FromResult(new DatabaseMicroserivces().RemoveRemoteFile(request).Result);
            return Task.FromResult(new intger() { Number = 0 });
        }
        public override Task<D_RemoteFiles> GetRemoteFiles(UserDbInfomation request, ServerCallContext context)
        {
            request.DbName = new JWTController().GetUsername(request.DbName);
            if (request.DbName != "")
                return Task.FromResult(new DatabaseMicroserivces().GetRemoteFiles(request).Result);
            return Task.FromResult(new D_RemoteFiles());
        }
        #endregion
        #region Subject
        public override Task<intger> AddSubject(D_Subject request, ServerCallContext context)
        {
            if (new JWTController().ValidateRoleLevel(request.User.DbName,RoleType.teacher))
            {
                return Task.FromResult(new DatabaseMicroserivces().AddSubject(request).Result);
            }
            return Task.FromResult(new intger() { Number = 0 });
        }
        public override Task<D_Subjects> GetSubjects(UserDbInfomation request, ServerCallContext context)
        {
            if (new JWTController().ValidateRoleLevel(request.DbName, RoleType.user))
            {
                return Task.FromResult(new DatabaseMicroserivces().GetSubjects(request).Result);
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
                return Task.FromResult(new DatabaseMicroserivces().AddProjectTheme(request).Result);
            }
            NotValid();
            return Task.FromResult(new intger() { Number = 0 });
        }
        public override Task<D_ProjectThemes> GetProjectThemes(UserDbInfomation request, ServerCallContext context)
        {
            if (new JWTController().ValidateRoleLevel(request.DbName, RoleType.teacher))
            {
                request.DbName = new JWTController().GetUsername(request.DbName);
                return Task.FromResult(new DatabaseMicroserivces().GetProjectThemes(request).Result);
            }
            NotValid();
            return Task.FromResult(new D_ProjectThemes());
        }
        public override Task<D_ProjectThemes> GetProjectThemesFromSubject(ThemeFromSubject request, ServerCallContext context)
        {
            request.User.DbName = new JWTController().GetUsername(request.User.DbName);
            if (!string.IsNullOrEmpty(request.User.DbName))
            {
                return Task.FromResult(new DatabaseMicroserivces().GetProjectThemesFromSubject(request).Result);
            }
            NotValid();
            return Task.FromResult(new D_ProjectThemes());
        }
        public override Task<intger> AddProjectThemeCoTeacher(ProjectThemeUserInfomation request, ServerCallContext context)
        {
            if (new JWTController().ValidateRoleLevel(request.User.DbName, RoleType.teacher))
            {
                request.User.DbName = new JWTController().GetUsername(request.User.DbName);
                return Task.FromResult(new DatabaseMicroserivces().AddProjectThemeCoTeacher(request).Result);
            }
            NotValid();
            return Task.FromResult(new intger() { Number = 0 });
        }
        public override Task<intger> RemoveProjectTheme(ProjectThemeUserInfomation request, ServerCallContext context)
        {
            if (new JWTController().ValidateRoleLevel(request.User.DbName, RoleType.teacher))
            {
                request.User.DbName = new JWTController().GetUsername(request.User.DbName);
                return Task.FromResult(new DatabaseMicroserivces().RemoveProjectTheme(request).Result);
            }
            NotValid();
            return Task.FromResult(new intger() { Number = 0 });
        }
        public override Task<intger> RemoveProjectThemeCoTeacher(ProjectThemeUserInfomation request, ServerCallContext context)
        {
            if (new JWTController().ValidateRoleLevel(request.User.DbName, RoleType.teacher))
            {
                request.User.DbName = new JWTController().GetUsername(request.User.DbName);
                return Task.FromResult(new DatabaseMicroserivces().RemoveProjectThemeCoTeacher(request).Result);
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
