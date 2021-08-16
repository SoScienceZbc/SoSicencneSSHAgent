using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using LoginService_Grpc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace SoSicencneSSHAgent.MicroServices
{
    public class LoginServiceMicroserivces
    {
        static LoginService.LoginServiceClient channel;
        static GrpcWebHandler handler = new GrpcWebHandler(GrpcWebMode.GrpcWebText, new HttpClientHandler());
        
        public LoginServiceMicroserivces()
        {
            if (channel == null)
                channel = new LoginService.LoginServiceClient(GrpcChannel.ForAddress("http://localhost:48053", new GrpcChannelOptions
                {
                    HttpClient = new HttpClient(handler),
                    Credentials = ChannelCredentials.Insecure
                }));

        }

        public Task<LoginRepley> LoginAD(LoginRequset requset, ServerCallContext context )
        {
            return Task.FromResult(channel.LoginAD(requset));
        }
       
    }
}
