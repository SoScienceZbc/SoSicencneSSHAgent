using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using SoSicencneSSHAgent.SSHClasss;
using System.Net.Http;
using Grpc.Net.Client.Web;
using System.IO;

namespace SoSicencneSSHAgent.MicroServices
{
    public class MicroServicesSetUp : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //TODO: Implement a ssl here and uncomment appcontext.setswitch.
            //TODO: Implement SSL certificat on port 33701 via kj.UserHttps(ssl_certPath,Name);
            //AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);
            string cert = "/home/soscience/Desktop/Services/soscience.dk.pfx";
            string pass = File.ReadAllText("/home/soscience/Desktop/Services/PassPhrase.txt");
            Console.WriteLine("password length: " + pass.Length);
            await Host.CreateDefaultBuilder().ConfigureWebHostDefaults(o =>
            {
                o.UseKestrel().UseStartup<GrpcAgentStartUp>().ConfigureKestrel(k =>
                {
                    k.Listen(System.Net.IPAddress.Any, 33700, kj =>
                    {
                        kj.Protocols = HttpProtocols.Http1;
                    });
                    k.Listen(System.Net.IPAddress.Any, 33701, kj =>
                    {
                        kj.Protocols = HttpProtocols.Http2;
                        kj.UseHttps(cert,pass);
                        //TODO: Convert SSL to OpenSSL
                    });
                });
            }).Build().StartAsync(stoppingToken);
        }
    }
}
