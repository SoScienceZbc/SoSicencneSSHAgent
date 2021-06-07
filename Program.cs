using System;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
            Console.WriteLine("Wait A thread has started as a background services.");
            Console.ReadLine();



        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args).ConfigureServices((hostContext, services) =>
           {               
               services.AddHostedService<MicroServicesSetUp>();

           });
    }
}
