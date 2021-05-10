using System;
using System.Threading;
using SoSicencneSSHAgent.ServerManger;
using SoSicencneSSHAgent.SSHClasss;

namespace SoSicencneSSHAgent
{
    class Program
    {
        static void Main(string[] args)
        {


            //PemReader.PemReaderHandler key = new PemReader.PemReaderHandler();
            //key.GetKey();
            SSHAgent agent = new SSHAgent();
            agent.CreateSshTunnel();
            ThreadPool.QueueUserWorkItem(new TcpHandler().StartThread);
            Console.WriteLine("Wait it is a background services.");
            Console.ReadLine();
            //agent.CreateSshTunnel();
            //agent.CreateSshStream();
            //while (true)
            //{
            //    string userinput = Console.ReadLine();
            //    Console.WriteLine(agent.sendCommand(userinput));
            //}

        }
    }
}
