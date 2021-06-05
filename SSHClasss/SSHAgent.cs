using Renci.SshNet;
using Renci.SshNet.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SoSicencneSSHAgent.SSHClasss
{
    public class SSHAgent : IDisposable
    {
        #region Fields

        SshClient clientS;
        SshCommand sc;
        ShellStream stream;
        ConnectionInfo connectionInfo;

        #region TestSSH Parms
        /*This is for testing ssh towards a test serve.*/
        //string sshServer = "192.168.1.102";
        // string username = "root";
        //string password = "skpit4200!"; // SSh use RSA keyauth so password is not used.
        #endregion

        string sshServer = "40.87.150.18";
        int port = 22;
        string username = "SoScienceUser";
        #endregion
        #region Construtor
        public SSHAgent()
        {
            connectionInfo =
                new ConnectionInfo(
                sshServer,
                port,
                username,
                new PrivateKeyAuthenticationMethod(username, new PrivateKeyFile("SoScienceServer_key.pem")));

            clientS = new SshClient(connectionInfo);
            //CreateSshTunnel();
        }
        #endregion
        #region methoeds        
        public void ConnnectToSShServer()
        {
            clientS.Connect();
        }
        public void CreateSshStream()
        {

            if (!clientS.IsConnected)
            {
                ConnnectToSShServer();
            }

            Console.WriteLine($"{clientS.ConnectionInfo.Username } : Is connected");
            sc = clientS.CreateCommand("ps -aux -f -all");
            sc.Execute();
            stream = clientS.CreateShellStream("ps -aux -f -all", 80, 24, 800, 600, 2048);
            string answer = sc.Result;
            Console.WriteLine(answer);

        }
        public void CreateSshTunnel()
        {
            sendCommand("ssh -N -T -R 33700:localhost:33700 SoScienceUser@40.87.150.18");
            //sendCommand("ssh -R 33700:localhost:33700 SoScienceUser@40.87.150.18");

            //sendCommand("ssh -T -p 33700 SoScienceUser@localhost");

            SshTunnel sshtunnel = new SshTunnel(this.clientS);
        }
        #endregion
        #region SSHCommands
        /// <summary>
        /// this sends ssh commands
        /// </summary>
        /// <param name="customCMD"></param>
        /// <returns></returns>
        public string sendCommand(string customCMD)
        {
            CreateSshStream();
            StringBuilder answer;

            var reader = new StreamReader(stream);
            var writer = new StreamWriter(stream);
            writer.AutoFlush = true;
            WriteStream(customCMD, writer, stream);
            answer = ReadStream(reader);
            return answer.ToString();
        }

        /// <summary>
        /// This writes to the stream
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="writer"></param>
        /// <param name="stream"></param>
        private void WriteStream(string cmd, StreamWriter writer, ShellStream stream)
        {
            writer.WriteLine(cmd);
            while (stream.Length == 0)
            {
                Thread.Sleep(50);
            }
        }

        /// <summary>
        /// This reads the stream.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private StringBuilder ReadStream(StreamReader reader)
        {
            StringBuilder result = new StringBuilder();

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                result.AppendLine(line);
            }
            return result;
        }
        #endregion


        #region Methods
        /// <summary>
        /// This the to start the protforwarding.
        /// remotePort is the port whihce to hit via the ssh tunnel
        /// </summary>
        public void Dispose()
        {
            if (clientS != null)
                clientS.Dispose();
        }
        #endregion
    }

    class SshTunnel
    {
        #region Fields
        private SshClient client;
        #endregion
        #region Construtor
        public SshTunnel(SshClient client)
        {
            // ("192.168.1.100", 7557, @"192.168.0.220");  
            this.client = client;
            StartSshtunnel();
        }
        #endregion
        #region Methods
        /// <summary>
        /// This the to start the protforwarding.
        /// remotePort is the port whihce to hit via the ssh tunnel
        /// </summary>
        public void StartSshtunnel()
        {
            try
            {
                if (!client.IsConnected)
                {

                    client.Connect();
                }
                ForwardedPortRemote portRemote = new ForwardedPortRemote(System.Net.IPAddress.Parse("127.0.0.1"), 33700, System.Net.IPAddress.Loopback, 33700);
                //ForwardedPortRemote portRemote = new ForwardedPortRemote(5002,"127.0.0.1",33700);
                //ForwardedPortLocal port = new ForwardedPortLocal(33700, client.ConnectionInfo.Host, 33700);
                portRemote.RequestReceived += new EventHandler<PortForwardEventArgs>(port_Request);
                //port.RequestReceived += new EventHandler<PortForwardEventArgs>(portL_Request);
                portRemote.Exception += new EventHandler<ExceptionEventArgs>(Port_Ex);
                //client.AddForwardedPort(port);
                client.KeepAliveInterval = TimeSpan.FromSeconds(10);
                client.AddForwardedPort(portRemote);
                foreach (var item in client.ForwardedPorts)
                {
                    item.Start();
                }

                if (portRemote.IsStarted && client.IsConnected)
                {


                    Console.WriteLine("BoundHost: " + portRemote.BoundHost);
                    Console.WriteLine("BoundPort:" + portRemote.BoundPort);
                    Console.WriteLine($"Clinet Infomation : {client.ConnectionInfo.Username}\n" +
                        $"Host : {client.ConnectionInfo.Host}\nProxyHost: {client.ConnectionInfo.ProxyHost} The tunnel have been createde..");
                    //while (client.IsConnected)
                    //{
                    //    Thread.Sleep(30000);
                    //    client.SendKeepAlive();
                    //}
                }
                else
                {
                    Console.WriteLine("nop");
                }
            }
            catch
            {
                client.Disconnect();
                client.Dispose();
                Dispose();
                throw;
            }
        }
        public void Dispose()
        {
            if (client != null)
                client.Dispose();
        }
        #endregion

        #region Events
        public void port_Request(object sender, PortForwardEventArgs s)
        {
            ForwardedPortRemote senders = (ForwardedPortRemote)sender;

            Console.WriteLine($"PortRemote: {s.OriginatorHost} :{s.OriginatorPort}\ncallerHost: {senders.Host}" +
                $"\nCallerBoundPort: {senders.BoundPort}\nForwardRemotePortStarteted: {senders.IsStarted}");
        }
        public void portL_Request(object sender, PortForwardEventArgs s)
        {

            ForwardedPortLocal senders = (ForwardedPortLocal)sender;
            Console.WriteLine($"PortRemote: {s.OriginatorHost} :{s.OriginatorPort}\ncallerHost: {senders.Host}" +
                $"\nCallerBoundPort: {senders.BoundPort}\nForwardRemotePortStarteted: {senders.IsStarted}");
        }
        public void Port_Ex(object sender, ExceptionEventArgs s)
        {
            Console.WriteLine("Exaction" + s.Exception.Message);
        }
        #endregion
    }

    #region sshmysqlagent_Testing
    public class sshMysqlAgent
    {
        public static (SshClient SshClient, uint Port) ConnectSsh(string sshHostName, string sshUserName, string sshPassword = null,
            string sshKeyFile = null, string sshPassPhrase = null, int sshPort = 22, string databaseServer = "localhost", int databasePort = 3306)
        {
            #region check arguments
            // check arguments
            if (string.IsNullOrEmpty(sshHostName))
                throw new ArgumentException($"{nameof(sshHostName)} must be specified.", nameof(sshHostName));
            if (string.IsNullOrEmpty(sshHostName))
                throw new ArgumentException($"{nameof(sshUserName)} must be specified.", nameof(sshUserName));
            if (string.IsNullOrEmpty(sshPassword) && string.IsNullOrEmpty(sshKeyFile))
                throw new ArgumentException($"One of {nameof(sshPassword)} and {nameof(sshKeyFile)} must be specified.");
            if (string.IsNullOrEmpty(databaseServer))
                throw new ArgumentException($"{nameof(databaseServer)} must be specified.", nameof(databaseServer));
            // define the authentication methods to use (in order)
            var authenticationMethods = new List<AuthenticationMethod>();
            if (!string.IsNullOrEmpty(sshKeyFile))
            {
                authenticationMethods.Add(new PrivateKeyAuthenticationMethod(sshUserName,
                    new PrivateKeyFile(sshKeyFile, string.IsNullOrEmpty(sshPassPhrase) ? null : sshPassPhrase)));
            }
            if (!string.IsNullOrEmpty(sshPassword))
            {
                authenticationMethods.Add(new PasswordAuthenticationMethod(sshUserName, sshPassword));
            }
            #endregion

            // connect to the SSH server
            var sshClient = new SshClient(new ConnectionInfo(sshHostName, sshPort, sshUserName, authenticationMethods.ToArray()));
            sshClient.Connect();

            // forward a local port to the database server and port, using the SSH server
            var forwardedPort = new ForwardedPortLocal("127.0.0.1", databaseServer, (uint)databasePort);
            sshClient.AddForwardedPort(forwardedPort);
            forwardedPort.Start();

            return (sshClient, forwardedPort.BoundPort);
        }

    }
    #endregion
}
