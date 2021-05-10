using Google.Protobuf;
using SoScienceDataServer;
using SoSicencneSSHAgent.CryptoLogic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SoSicencneSSHAgent.ServerManger
{
    class Receiver
    {
        public DateTime lastPulse = DateTime.Now;
        public DateTime myLastPulse;
        private static readonly Dictionary<string, byte[]> UserData = new Dictionary<string, byte[]>();
        private readonly MicroServicesManger messageHandler;

        // pulse for confirm
        public object ComfirmObj = new object();

        // Crypteren for the Stream
        public AESCryptor Cryptor { get; set; }

        public Receiver()
        {
            this.messageHandler = new MicroServicesManger();
        }

        /// <summary>
        /// Runs a request
        /// </summary>
        /// <param name="buffer">the request</param>
        /// <returns>the response</returns>
        public async Task<List<Message>> RunAsync(byte[] buffer)
        {
            await Task.Delay(1);
            try
            {

                Message message = Message.Parser.ParseFrom(buffer);
                message.Data = ByteString.CopyFrom(Cryptor.Decrypt(message.Data.ToByteArray()));
                byte[] data = message.Data.ToByteArray();
                string user = "";

                switch (message.MessageType)
                {
                    case Message.Types.MessageType.Heartbeat:
                        // Reads the heartbeat
                        DateTime dateTime = DateTime.Parse(Encoding.UTF8.GetString(message.Data.ToByteArray()));
                        if (!myLastPulse.Equals(dateTime))
                        {
                            lastPulse = dateTime;
                        }
                        return null;
                    case Message.Types.MessageType.Confirm:
                        // Pulses confirm
                        Console.WriteLine("pulsing");
                        Monitor.Enter(ComfirmObj);
                        Monitor.Pulse(ComfirmObj);
                        Monitor.Exit(ComfirmObj);
                        return null;
                    case Message.Types.MessageType.Part:
                        // Reads part message and saves it for the user
                        user = Cryptor.Decrypt(message.Username);
                        if (UserData.ContainsKey(user))
                        {
                            int length = UserData[user].Length;
                            byte[] tempData = UserData[user];
                            Array.Resize(ref tempData, length + data.Length);
                            data.CopyTo(tempData, length);
                            UserData[user] = tempData;
                        }
                        else
                        {
                            UserData.Add(user, data);
                        }
                        // Returns a message done 
                        message.MessageType = Message.Types.MessageType.Done;
                        return GenerateMessages(message, new byte[0]);
                    case Message.Types.MessageType.Done:
                        user = Cryptor.Decrypt(message.Username);
                        if (UserData.ContainsKey(user))
                        {
                            // Gets if there is a part message
                            byte[] tempData = UserData[user];
                            int length = tempData.Length;
                            Array.Resize(ref tempData, length + data.Length);
                            data.CopyTo(tempData, length);
                            data = tempData;
                            UserData.Remove(user);
                        }
                        switch (message.RequestType)
                        {
                            case Message.Types.RequestType.Login:
                                return GenerateMessages(message, messageHandler.Login(data));
                            case Message.Types.RequestType.PostProject:
                                return GenerateMessages(message, messageHandler.AddProject(data, user));
                            case Message.Types.RequestType.RemoveProject:
                                return GenerateMessages(message, messageHandler.RemoveProject(data, user));
                            case Message.Types.RequestType.GetProjectLite:
                                return GenerateMessages(message, messageHandler.GetProjects(user));
                            case Message.Types.RequestType.GetProject:
                                return GenerateMessages(message, messageHandler.GetProject(data, user));
                            case Message.Types.RequestType.PostDocument:
                                return GenerateMessages(message, messageHandler.AddDocument(data));
                            case Message.Types.RequestType.RemoveDocument:
                                return GenerateMessages(message, messageHandler.RemoveDocument(data));
                            case Message.Types.RequestType.GetDocument:
                                return GenerateMessages(message, messageHandler.GetDocument(data));
                            case Message.Types.RequestType.GetFile:
                                return GenerateMessages(message, messageHandler.GetRemoteFile(data));
                            case Message.Types.RequestType.UploadFile:
                                return GenerateMessages(message, messageHandler.WriteRemoteFile(data));
                            case Message.Types.RequestType.GetPartFile:
                                return GenerateMessages(message, messageHandler.GetRemotePartFile(data));
                            default:
                                break;
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                Message message = new Message();
                message.MessageType = Message.Types.MessageType.Error;
                return GenerateMessages(message, Encoding.UTF8.GetBytes(e.Message));

            }
            return null;
        }

        private List<Message> GenerateMessages(Message oldMessage, byte[] data)
        {
            if (oldMessage.Identifier == "")
            {
                Console.WriteLine("help im broken");
            }
            int partLenght;
            if (oldMessage.RequestType == Message.Types.RequestType.GetPartFile)
            {
                partLenght = 60000;
            }
            else
            {
                partLenght = 60000;
            }
            oldMessage.Username = "";
            List<Message> messages = new List<Message>();
            if (data.Length > partLenght)
            {
                int pos = 0;
                byte[] buffer = new byte[partLenght];
                while (pos + partLenght < data.Length)
                {
                    Message tempMessage = new Message(oldMessage);
                    tempMessage.MessageType = Message.Types.MessageType.Part;
                    Array.Copy(data, pos, buffer, 0, partLenght);
                    tempMessage.Data = ByteString.CopyFrom(Cryptor.Encrypt(buffer));
                    messages.Add(tempMessage);
                    pos += partLenght;
                }
                Array.Copy(data, pos, buffer, 0, data.Length - pos);
                Array.Resize(ref buffer, data.Length - pos);
                oldMessage.Data = ByteString.CopyFrom(Cryptor.Encrypt(buffer));
                oldMessage.MessageType = Message.Types.MessageType.Done;
                messages.Add(oldMessage);
            }
            else if (data.Length > 0)
            {
                oldMessage.Data = ByteString.CopyFrom(Cryptor.Encrypt(data));
                messages.Add(oldMessage);
            }
            else
            {
                oldMessage.Data = ByteString.Empty;
                messages.Add(oldMessage);
            }
            return messages;
        }
    }
}
