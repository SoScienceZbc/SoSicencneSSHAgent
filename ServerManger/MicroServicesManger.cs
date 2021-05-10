using System;
using System.Collections.Generic;
using System.Text;
using SoScienceDataServer;

namespace SoSicencneSSHAgent.ServerManger
{
    class MicroServicesManger : IMicroServicesManger
    {
        public byte[] AddDocument(byte[] data)
        {
            throw new NotImplementedException();
        }

        public byte[] AddProject(byte[] data, string user)
        {
            throw new NotImplementedException();
        }

        public byte[] GetDocument(byte[] data)
        {
            throw new NotImplementedException();
        }

        public byte[] GetProject(byte[] data, string user)
        {
            throw new NotImplementedException();
        }

        public byte[] GetProjects(string user)
        {
            throw new NotImplementedException();
        }

        public byte[] GetRemoteFile(byte[] data)
        {
            throw new NotImplementedException();
        }

        public byte[] GetRemotePartFile(byte[] data)
        {
            throw new NotImplementedException();
        }
        public byte[] Login(byte[] data)
        {
            throw new NotImplementedException();
        }

        public byte[] RemoveDocument(byte[] data)
        {
            throw new NotImplementedException();
        }

        public byte[] RemoveProject(byte[] data, string user)
        {
            throw new NotImplementedException();
        }

        public byte[] WriteRemoteFile(byte[] data)
        {
            throw new NotImplementedException();
        }
    }

    interface IMicroServicesManger
    {
        byte[] AddDocument(byte[] data);
        byte[] AddProject(byte[] data, string user);
        byte[] GetDocument(byte[] data);
        byte[] GetProject(byte[] data, string user);
        byte[] GetProjects(string user);
        byte[] GetRemoteFile(byte[] data);
        byte[] GetRemotePartFile(byte[] data);
        byte[] Login(byte[] data);
        byte[] RemoveDocument(byte[] data);
        byte[] RemoveProject(byte[] data, string user);
        byte[] WriteRemoteFile(byte[] data);
    }
}
