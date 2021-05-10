using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace SoSicencneSSHAgent.PemReader
{
    class PemReaderHandler
    {
        public byte[] GetKey()
        {
            var key = ECDsa.Create();
            var certPemArray = File.ReadAllLines("SoScienceServer_key.pem");
            string certPem = "";
            foreach (string item in certPemArray)
            {
                if (!item.Contains("PRIVATE"))
                {
                    certPem += item;
                }
            }
            //var cert = new X509Certificate2(Convert.FromBase64String(certPem));

            // can be combined with the private key from the previous section 
            //X509Certificate2 certWithKey = cert.CopyWithPrivateKey(key);
            //Console.WriteLine("Read the key and this is the key: " + certPem);
            return Convert.FromBase64String(certPem);
        }

    }
}
