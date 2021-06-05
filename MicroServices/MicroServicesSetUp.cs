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

namespace SoSicencneSSHAgent.MicroServices
{
    public class MicroServicesSetUp : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            //AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            //AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);

            await Host.CreateDefaultBuilder().ConfigureWebHostDefaults(o =>
            {
                
                //o.UseKestrel()
                o.UseKestrel().UseStartup<GrpcAgentStartUp>().ConfigureKestrel(k => 
                { 
                    k.Listen(System.Net.IPAddress.Any, 33700, kj => {
                        kj.Protocols = HttpProtocols.Http1;                        
                    });
                    k.Listen(System.Net.IPAddress.Any,33701,kj => {
                        kj.Protocols = HttpProtocols.Http2;                        
                    });
                });
            }).Build().StartAsync(stoppingToken);
            
            //using (SSHAgent agent = new SSHAgent())
            //{
            //    Thread th = new Thread(agent.StartSshtunnel);
            //    th.Start();
            //    //agent.StartSshtunnel();
            //};
            //agent.CreateSshTunnel();
            //th.Start();
            //ThreadPool.QueueUserWorkItem(new TcpHandler().StartThread);
        }
    }
}
