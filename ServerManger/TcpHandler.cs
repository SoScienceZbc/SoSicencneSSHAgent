using Google.Protobuf;
using SoScienceDataServer;
using SoSicencneSSHAgent.CryptoLogic;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SoSicencneSSHAgent.ServerManger
{
    class TcpHandler
    {
        // lockes
        private readonly object sendingLock = new object();

        private object[] readWriteLock = new object[] { new object(), new object() };
        private System.Timers.Timer timer;
        private Socket socket;
        private Receiver receiver;
        private NetworkStream stream;
        private TcpClient client;
        private readonly List<Task> Tasks = new List<Task>();
        public bool run = true;
        private readonly int buffersize = 1400;

        /// <summary>
        /// starts the connection with the proxy server
        /// </summary>
        /// <param name="ip">the ip of the server </param>
        /// <param name="port">the port the server is lising to</param>
        /// <param name="db">the data base name</param>
        public async Task Start(string ip, int port, string db)
        {
            // Creates the Timer for the heartbeat
            timer = new System.Timers.Timer(10000);
            timer.AutoReset = true;
            timer.Elapsed += Heartbeat;

            receiver = new Receiver();

            while (run)
            {
                // Logs
                try
                {
                    // Creates a TcpClient
                    client = new TcpClient();
                    client.Connect(ip, port);
                    stream = client.GetStream();
                    if (Handshake())
                    {
                        // Only runs if handshake was completed 
                        stream.Flush();
                        timer.Start();
                        while (client.Connected && run)
                        {
                            try
                            {
                                // starts a task with a request from the server
                                //_ = SendAsync(receiver.RunAsync(Read()));                                
                                await Task.FromResult(SendAsync(receiver.RunAsync(Read())));                                
                            }
                            catch (Exception e)
                            {
                                // Logs
                                Console.WriteLine("SocketController Start: " + e.Message);
                            }
                        }
                        timer.Stop();
                        // Logs
                        Console.WriteLine("Connection closed");
                    }
                }
                catch (Exception e)
                {
                    // Logs
                    Console.WriteLine("Error: " + e.Message);
                }
            }
        }
        /// <summary>
        /// Sendes a heartbeat to the proxy server looks if servers heartbeat is valid 
        /// </summary>
        private void Heartbeat(object sender, System.Timers.ElapsedEventArgs e)
        {

            // Looks if server heartbeat is vaildt
            int time = receiver.lastPulse.CompareTo(DateTime.Now.AddMinutes(-1));
            if (time >= 1)
            {
                // makes the heartbeat
                DateTime dateTime = DateTime.Now;
                receiver.myLastPulse = DateTime.Parse(dateTime.ToString("O"));
                Message message = new Message
                {
                    MessageType = Message.Types.MessageType.Heartbeat,
                    Data = ByteString.CopyFrom(receiver.Cryptor.Encrypt(Encoding.UTF8.GetBytes(receiver.myLastPulse.ToString("O"))))
                };
                // Sends the heartbeat to the server
                Send(message.ToByteArray());
            }
            else
            {
                stream.Close();
                client.Close();
            }
        }

        /// <summary>
        /// sends data 
        /// </summary>
        /// <param name="dataTask"> a task that returns the data that will be send to the server</param>
        private async Task SendAsync(Task<List<Message>> dataTask)
        {
            List<Message> data = await dataTask;
            if (data != null)
            {
                foreach (Message message in data)
                {
                                       
                    Send(message.ToByteArray());

                    if (message.MessageType == Message.Types.MessageType.Part)
                    {
                        // if messages is part wait to a Confirm
                        Monitor.Enter(receiver.ComfirmObj);
                        Monitor.Wait(receiver.ComfirmObj);
                        Monitor.Exit(receiver.ComfirmObj);
                    }
                }
            }
        }

        /// <summary>
        /// Makes a Diffiehelman encryption with the server
        /// </summary>
        /// <returns>if the handshake is created</returns>
        private bool Handshake()
        {
            RSACryptor rsa = new RSACryptor(2048);

            // creates message
            Message message = new Message
            {
                MessageType = Message.Types.MessageType.Confirm,
                RequestType = Message.Types.RequestType.Key
            };

            // creates the key
            Key key = new Key
            {
                // Adds the dh public data
                Type = Key.Types.KeyType.Rsa,
                Value1 = ByteString.CopyFrom(rsa.GetPublicKey())
            };

            // adds the key to the message
            message.Data = key.ToByteString();

            // sends the message
            Send(message.ToByteArray());

            // gets the message from the server and parses it
            message = Message.Parser.ParseFrom(Read());

            // gets the key from the message
            key = Key.Parser.ParseFrom(rsa.Decrypt(message.Data.ToByteArray()));
            if (key.Type == Key.Types.KeyType.Aes)
            {
                AESCryptor aes = new AESCryptor(key.Value1.ToByteArray(), key.Value2.ToByteArray());

                // gives receiver the aes encryption object
                receiver.Cryptor = aes;
                return true;
            }
            return false;
        }

        /// <summary>
        /// reads the data from the proxy server
        /// </summary>
        /// <returns> the data from the server</returns>
        private byte[] Read()
        {
            byte[] packet = new byte[0];
            int oldPacketSize = 0;
            int receiveSize = 0;
            int packetsAmount = 0;

            // Locks the read part of the stream
            lock (readWriteLock[1])
            {
                do
                {
                    // gets the lenght of the comming message
                    byte[] bytes = new byte[buffersize];
                    int lenght = stream.Read(bytes, 0, buffersize);

                    // gets the lenght of the packet
                    receiveSize = BitConverter.ToInt32(bytes);

                    // gets the packets left
                    packetsAmount = BitConverter.ToInt32(bytes, 4);
                    Array.Resize(ref packet, packet.Length + receiveSize);

                    // Addeds the new packet to the full data packet
                    Array.Copy(bytes, 8, packet, oldPacketSize, receiveSize);
                    oldPacketSize = packet.Length;
                }
                while (packetsAmount > 0);
            }
            return packet;
        }


        /// <summary>
        /// sends data to the proxy server
        /// </summary>
        /// <param name="bytes">the data that will be send</param>
        private void Send(byte[] bytes)
        {
            // setup for packet handling
            int usefullSpace = buffersize - 8;
            int size = bytes.Length;
            int packets = (int)Math.Ceiling((float)(size) / usefullSpace);
            byte[] packet;
            lock (readWriteLock[0])
            {
                for (int i = 0; i < packets; i++)
                {
                    // creates all the packets
                    packet = new byte[buffersize];

                    // sets the amount of packets left to come
                    BitConverter.GetBytes(packets - 1 - i).CopyTo(packet, 4);

                    // sets the size of the packet
                    if (usefullSpace * (i + 1) < size)
                    {
                        BitConverter.GetBytes(usefullSpace).CopyTo(packet, 0);
                        Array.Copy(bytes, usefullSpace * i, packet, 8, usefullSpace);
                    }
                    else
                    {
                        BitConverter.GetBytes(size - (usefullSpace * i)).CopyTo(packet, 0);
                        Array.Copy(bytes, (usefullSpace * i), packet, 8, (size - (usefullSpace * i)));
                        //Array.Resize(ref packet, (size - (usefullSpace * i)) + 8);
                    }
                    // sendes the packet
                    stream.Write(packet, 0, packet.Length);
                }
            }
        }

        /// <summary>
        /// closes everything
        /// </summary>
        public void Close()
        {
            run = false;
            if (stream != null)
                stream.Close();
            if (client != null)
                client.Close();
        }
    }
}
