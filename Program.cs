using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SoSicencneSSHAgent.jwt;
using SoSicencneSSHAgent.jwt.managers;
using SoSicencneSSHAgent.jwt.models;
using SoSicencneSSHAgent.MicroServices;
using SoSicencneSSHAgent.SSHClasss;

namespace SoSicencneSSHAgent
{
    class Program
    {
        static void Main(string[] args)
        {
            
            SSHAgent agent = new SSHAgent();
            Thread th = new Thread(agent.CreateSshTunnel);
            th.Start();
            CreateHostBuilder(args).Build().StartAsync();
            Console.WriteLine("The BackGroundservice have been started, press enter to stop this program.");
            Console.ReadLine();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args).ConfigureServices((hostContext, services) =>
           {               
               services.AddHostedService<MicroServicesSetUp>();

           });
    }
}
