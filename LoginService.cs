using Grpc.Core;
using LoginService_Grpc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SoSicencneSSHAgent
{
    class LoginService : LoginService_Grpc.LoginService.LoginServiceBase
    {
        public override Task<LoginRepley> LoginAD(LoginRequset request, ServerCallContext context)
        {
            Console.WriteLine($"Host:{context.Host} called Method:{context.Method}");
            return Task.FromResult(new LoginService().LoginAD(request, context).Result);
        }
    }
}
