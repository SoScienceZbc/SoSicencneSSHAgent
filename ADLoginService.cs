using Grpc.Core;
using LoginService_Grpc;
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
            return Task.FromResult(new LoginServiceMicroserivces().LoginAD(request, context).Result);
        }
    }
}
