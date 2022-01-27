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
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace SoSicencneSSHAgent.MicroServices
{
    public class LoginServiceMicroserivces
    {
        static LoginService.LoginServiceClient client;

        //This is a hashed serial used in DangerousServerCertificateCustomValidationCallback() to validate the server certificate.
        private string HashedSerial { get; } = File.ReadAllText(Directory.GetCurrentDirectory() + "/HashedSerial.txt");

        //static GrpcWebHandler handler = new GrpcWebHandler(GrpcWebMode.GrpcWebText, new HttpClientHandler());
        public LoginServiceMicroserivces()
        {
            //AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);

            if (client == null)
            {
                Console.WriteLine("Initiate LSM Channel");
                client = CreateGrpcClient("https://localhost:48053");
                Console.WriteLine("Channel Created");
            } else
            {
                Console.WriteLine("Channel is not null");
            }

        }
        /// <summary>
        /// Sets up a Proto client and channel for making Grpc calls to the Login service
        /// </summary>
        /// <param name="channelURL"></param>
        /// <returns></returns>
        private LoginService.LoginServiceClient CreateGrpcClient(string channelURL)
        {
            HttpClientHandler http_handler = new HttpClientHandler();

            http_handler.ServerCertificateCustomValidationCallback = DangerousServerCertificateCustomValidationCallback;

            GrpcWebHandler handler = new GrpcWebHandler(GrpcWebMode.GrpcWebText, http_handler);

            LoginService.LoginServiceClient client = new LoginService.LoginServiceClient(
                GrpcChannel.ForAddress(new Uri(channelURL),
                new GrpcChannelOptions
                {
                    HttpClient = new HttpClient(handler),
                    Credentials = new SslCredentials()
                }));

            return client;
        }
        /// <summary>
        /// This checks whether the server's certificate should be trusted or not. More of a workaround. Implement a better way when possible.
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <param name="arg4"></param>
        /// <returns></returns>
        private bool DangerousServerCertificateCustomValidationCallback(HttpRequestMessage arg1, X509Certificate2 arg2, X509Chain arg3, SslPolicyErrors arg4)
        {
            /*
            //Used for debugging purposes
            Console.WriteLine("\n******CertificateResponse******\n");
            Console.WriteLine(arg1);
            Console.WriteLine("X509Certificate2:");
            Console.WriteLine(arg2);
            Console.WriteLine("X509Chain:");
            Console.WriteLine(arg3);
            Console.WriteLine("SslPolicyErrors:");
            Console.WriteLine(arg4);
            Console.WriteLine("\n******CertificateResponse******\n");
            */

            Sha256Hasher hasher = new Sha256Hasher();
            string serverSerialHashed = hasher.HashString(arg2.SerialNumber);

            //Compares the received serial with the expected serial
            if (serverSerialHashed.ToLower() == HashedSerial.ToLower())
                return true;
            else
                return false;
        }

        public Task<LoginRepley> LoginAD(LoginRequset requset, ServerCallContext context )
        {
            return Task.FromResult(client.LoginAD(requset));
        }
       
    }
}
