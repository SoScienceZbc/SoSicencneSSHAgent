using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SoSicencneSSHAgent.SSHClasss
{
    class SSHAgent
    {
        #region Fields
        SshClient sshclient;
        SshCommand sc;
        ShellStream stream;
        ConnectionInfo connectionInfo;
        string sshServer = "40.87.150.18";
        int port = 22;
       // string username = "root";
        string username = "SoScienceUser";
        //string password = "skpit4200!"; // SSh use RSA keyauth so password is not used.
        #endregion
        #region Construtor
        public SSHAgent()
        {
            connectionInfo = new ConnectionInfo(sshServer, port, username, new PrivateKeyAuthenticationMethod(username, new PrivateKeyFile("SoScienceServer_key.pem")));
            sshclient = new SshClient(connectionInfo);
        }
        #endregion
        #region methoeds        
        public void ConnnectToSShServer()
        {
            Console.WriteLine($"Trying to connect to ssh server {sshServer} on {port}");
            PrivateKeyFile keyFile = new PrivateKeyFile("SoScienceServer_key.pem");
            sshclient = new SshClient(new ConnectionInfo(sshServer, 7557, username, new PrivateKeyAuthenticationMethod(username, keyFile)));
            sshclient.Connect();
        }
        public void CreateSshStream()
        {
            if (!sshclient.IsConnected)
            {
                ConnnectToSShServer();
            }

            Console.WriteLine($"{sshclient.ConnectionInfo.Username } : Is connected");
            sc = sshclient.CreateCommand("ps -aux -f -all");
            sc.Execute();
            stream = sshclient.CreateShellStream("ps -aux -f -all", 80, 24, 800, 600, 2048);
            string answer = sc.Result;
            Console.WriteLine(answer);

        }
        public void CreateSshTunnel()
        {
            SshTunnel sshtunnel = new SshTunnel(new ForwardedPortLocal(0, "127.0.0.0", 2002), this.sshclient);
        }        
        public string sendCommand(string customCMD)
        {
            StringBuilder answer;

            var reader = new StreamReader(stream);
            var writer = new StreamWriter(stream);
            writer.AutoFlush = true;
            WriteStream(customCMD, writer, stream);
            answer = ReadStream(reader);
            return answer.ToString();
        }

        private void WriteStream(string cmd, StreamWriter writer, ShellStream stream)
        {
            writer.WriteLine(cmd);
            while (stream.Length == 0)
            {
                Thread.Sleep(50);
            }
        }

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
    }

    class SshTunnel : IDisposable
    {
        private SshClient client;
        private ForwardedPortLocal port;
        private int localPort;
        public int LocalPort { get { return localPort; } }
        public SshTunnel(ForwardedPortLocal forwardedPort, SshClient client)
        {
            // ("192.168.1.100", 7557, @"192.168.0.220");  
            port = forwardedPort;
            this.client = client;
            StartSshtunnel(22, 0, "127.0.0.0");
        }
        //remotePort is the port whihce to hit via the ssh tunnel
        //note the "hack" this have been done to ensure a more flexbly asignment of the ports when the connection comes in.
        public void StartSshtunnel(int? remotePort, int localport, string localhostIp)
        {
            try
            {
                //string RemoteHost = "40.87.150.18";
                string localBoundHost = "127.0.0.1";
                //int remoteHostPort = 22;
                //client = new SshClient(connectionInfo);
                // boundport is the port to accsse on the remote server,
                //port = new ForwardedPortLocal(localBoundHost, RemoteHost, (uint)remoteHostPort);
                //ForwardedPortDynamic port = new ForwardedPortDynamic(RemoteHost,22);                
                ForwardedPortDynamic port = new ForwardedPortDynamic(localBoundHost,7557);      
                client.Connect();
                client.AddForwardedPort(port);               
                port.Start();

                if (port.IsStarted && client.IsConnected)
                {


                    Console.WriteLine($"Clinet Infomation : {client.ConnectionInfo.Username}\n" +
                        $"Host : {client.ConnectionInfo.Host}\nProxyHost: {client.ConnectionInfo.ProxyHost} The tunnel have been createde..");

                }
                else
                {
                    Console.WriteLine("nop");
                }

                //TODO:Fix the dynamically allocated clietn port when there is time for it..
                // HACK to get the dynamically allocated client port
                //var listener = (TcpListener)typeof(ForwardedPortLocal).GetField("_listener", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(port);
                //localPort = ((System.Net.IPEndPoint)listener.LocalEndpoint).Port;
            }
            catch
            {
                Dispose();
                throw;
            }
        }


        public void Dispose()
        {
            if (port != null)
                port.Dispose();
            if (client != null)
                client.Dispose();
        }
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
