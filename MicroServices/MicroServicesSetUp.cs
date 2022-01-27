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
            //AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);
            string cert = "/home/soscience/Desktop/Services/soscience.dk.pfx";
            string pass = File.ReadAllText("/home/soscience/Desktop/Services/PassPhrase.txt");
            await Host.CreateDefaultBuilder().ConfigureWebHostDefaults(o =>
            {
                Console.WriteLine("Awaiting config");
                o.UseKestrel().UseStartup<GrpcAgentStartUp>().ConfigureKestrel(k =>
                {
                    Console.WriteLine("Configuring kestrel");
                    k.Listen(System.Net.IPAddress.Any, 33700, kj =>
                    {
                        Console.WriteLine("Using http");
                        kj.Protocols = HttpProtocols.Http1;
                    });
                    k.Listen(System.Net.IPAddress.Any, 33701, kj =>
                    {
                        Console.WriteLine("Using https");
                        kj.Protocols = HttpProtocols.Http2;
                        kj.UseHttps(cert, pass.Trim());
                    });
                });
            }).Build().StartAsync(stoppingToken);
            Console.WriteLine("Config done");
        }
    }
}
