using Grpc.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;
using Proto;
using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace SoSicencneSSHAgent.MicroServices
{
    public class LoginServiceMicroserivces : LoginServcie.LoginServcieBase
    {

        public override Task<LoginRepley> LoginAD(LoginRequset requset,ServerCallContext context )
        {
            return Task.FromResult(new LoginRepley { LoginSucsefull = ADLookup(requset.Username, requset.Password) });
        }
        //looks if user can login in AD
        public bool ADLookup(string username, string password)
        {
            bool valid;
            //  LdapDirectoryIdentifier identifier = new LdapDirectoryIdentifier("dc01.efif.dk", 389);
            LdapDirectoryIdentifier identifier = new LdapDirectoryIdentifier("10.255.1.1", 389);

            LdapConnection connection = new LdapConnection(identifier)
            {
                Credential = new NetworkCredential(username, password)
            };
            try
            {
                connection.Bind();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{username} have been login...");
                Console.ForegroundColor = ConsoleColor.White;
                valid = true;
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{username} cant login...");
                Console.ForegroundColor = ConsoleColor.White;
                valid = false;
            }
            finally
            {                
                connection.Dispose();
            }

            return valid;
        }
       
    }
}
