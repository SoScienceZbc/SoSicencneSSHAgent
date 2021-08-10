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
            string token = new JWTController().CreateToken("hello world", RoleType.theacher);
            Console.WriteLine("token: " + token);
            Console.WriteLine("Validate: " + new JWTController().ValidateJWT(token));
            Console.WriteLine("Validate For theacher: " + new JWTController().ValidateRoleLevel(token, RoleType.theacher));
            Console.WriteLine("username: " + new JWTController().GetUsername(token));

            Console.ReadLine();

            //SSHAgent agent = new SSHAgent();
            //Thread th = new Thread(agent.CreateSshTunnel);
            //th.Start();
            //CreateHostBuilder(args).Build().StartAsync();
            //Console.WriteLine("The BackGroundservice have been started, press enter to stop this program.");
            //Console.ReadLine();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args).ConfigureServices((hostContext, services) =>
           {               
               services.AddHostedService<MicroServicesSetUp>();

           });
    }
}
