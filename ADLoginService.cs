using DatabaseService_Grpc;
using Grpc.Core;
using LoginService_Grpc;
using SoSicencneSSHAgent.jwt;
using SoSicencneSSHAgent.MicroServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SoSicencneSSHAgent
{
    class ADLoginService : LoginService.LoginServiceBase
    {
        private static DatabaseMicroserivces dbm = new DatabaseMicroserivces();
        private static LoginServiceMicroserivces lsm = new LoginServiceMicroserivces();

        public override Task<LoginRepley> LoginAD(LoginRequset request, ServerCallContext context)
        {
            Console.WriteLine($"Host:{context.Host} called Method:{context.Method}");
            var data = lsm.LoginAD(request, context).Result;
            data.Token = new JWTController().CreateToken(request.Username, data.Admin ? RoleType.teacher : RoleType.user);
            if (data.Admin)
            {
                dbm.CheckAndInsertTeacher(new D_Teacher() { Username = request.Username });
            }
            return Task.FromResult(data);
        }

        public override Task<LoginRepley> ValidateToken(LoginRepley request, ServerCallContext context)
        {
            LoginRepley loginRepley = new LoginRepley();
            if (new JWTController().ValidateJWT(request.Token))
            {
                loginRepley.LoginSucsefull = true;
                loginRepley.Token = request.Token;
                loginRepley.Admin = new JWTController().GetRole(request.Token) == RoleType.teacher;
            }
            return Task.FromResult(loginRepley);
        }
    }
}
