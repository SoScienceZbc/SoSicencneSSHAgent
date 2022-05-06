﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Proto;
using Grpc.Core;
using Grpc.Net.Client.Web;
using Grpc.Net.Client;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace SoSicencneSSHAgent.MicroServices
{
    class MediaMicroService
    {
        static RemoteMediaService.RemoteMediaServiceClient client;

        //This is a hashed serial used in DangerousServerCertificateCustomValidationCallback() to validate the server certificate.
        private string HashedSerial { get; } = File.ReadAllText(Directory.GetCurrentDirectory() + "/HashedSerial.txt");

        public MediaMicroService()
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);
            client = CreateGrpcClient("https://localhost:48048"); //redirects to SoScienceMediaService
        }

        #region GrpcClientSetup
        /// <summary>
        /// Sets up a Proto client and channel for making Grpc calls to the Login service
        /// </summary>
        /// <param name="channelURL"></param>
        /// <returns></returns>
        private RemoteMediaService.RemoteMediaServiceClient CreateGrpcClient(string channelURL)
        {
            HttpClientHandler http_handler = new HttpClientHandler();

            http_handler.ServerCertificateCustomValidationCallback = DangerousServerCertificateCustomValidationCallback;

            GrpcWebHandler handler = new GrpcWebHandler(GrpcWebMode.GrpcWebText, http_handler);

            RemoteMediaService.RemoteMediaServiceClient client = new RemoteMediaService.RemoteMediaServiceClient(
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
        #endregion

        public Task<MediaReply> SendMedia(MediaRequest request)
        {
            Console.WriteLine("Entered SendMedia() in MediaMicroService");
            return Task.FromResult(client.SendMedia(request));
        }
        public Task<MediaRequests> GetMedias(ProjectInformation project)
        {
            Console.WriteLine("Entered GetMedias() in MediaMicroService");
            return Task.FromResult(client.GetMedias(project));
        }
        public Task<RetrieveMediaReply> RetrieveMedia(RetrieveMediaRequest request)
        {
            Console.WriteLine("Entered RetrieveMedia() in MediaMicroService");
            return Task.FromResult(client.RetrieveMedia(request));
        }
        public Task<MediaReply> DeleteMedia(RetrieveMediaRequest request)
        {
            Console.WriteLine("Entered DeleteMedia() in MediaMicroService");
            return Task.FromResult(client.DeleteMedia(request));
        }
        public Task<MediaReply> UpdateMedia(ChangeTitleRequest request)
        {
            Console.WriteLine("Entered UpdateMedia() in MediaMicroService");
            return Task.FromResult(client.UpdateMedia(request));
        }
    }
}
