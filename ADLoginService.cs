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
        public override Task<LoginRepley> LoginAD(LoginRequset request, ServerCallContext context)
        {
            Console.WriteLine($"Host:{context.Host} called Method:{context.Method}");
            var data = new LoginServiceMicroserivces().LoginAD(request, context).Result;
            data.Token = new JWTController().CreateToken(request.Username, data.Admin ? RoleType.teacher : RoleType.user);
            if (data.Admin)
            {
                new DatabaseMicroserivces().CheckAndInsertTeacher(new D_Teacher() { Username = request.Username });
            }
            return Task.FromResult(data);
        }

        public override Task<LoginRepley> ValidateToken(LoginRepley request, ServerCallContext context)
        {
            if (request.Admin)
            {
                if (new JWTController().ValidateRoleLevel(request.Token, RoleType.teacher)) 
                {
                    request.LoginSucsefull = true;
                    return Task.FromResult(request);
                } else
                {
                    return Task.FromResult(new LoginRepley());
                }
            } else
            {
                if (new JWTController().ValidateRoleLevel(request.Token, RoleType.user))
                {
                    request.LoginSucsefull = true;
                    return Task.FromResult(request);
                } else
                {
                    return Task.FromResult(new LoginRepley());
                }
            }
        }
    }
}
