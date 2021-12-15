using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace SoSicencneSSHAgent
{
    class CertificateHelper
    {
        public static X509Certificate2 LoadX509Certificate(string certificatename, string password)
        {
            var keyStorageFlags = X509KeyStorageFlags.EphemeralKeySet;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                keyStorageFlags = X509KeyStorageFlags.DefaultKeySet;
            }
            return new X509Certificate2(certificatename, password, keyStorageFlags);
        }
    }
}
