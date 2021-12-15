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
using System.IO;
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
            {
                Console.WriteLine("Initiate LSM Channel");
                channel = new LoginService.LoginServiceClient(GrpcChannel.ForAddress("https://localhost:48053", new GrpcChannelOptions
                {
                    HttpClient = new HttpClient(handler),
                    Credentials = new SslCredentials()
                }));
                Console.WriteLine("Channel Created");
            } else
            {
                Console.WriteLine("Channel is not null");
            }

        }

        public Task<LoginRepley> LoginAD(LoginRequset requset, ServerCallContext context )
        {
            return Task.FromResult(channel.LoginAD(requset));
        }
       
    }
}
