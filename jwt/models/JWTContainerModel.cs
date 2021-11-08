using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace SoSicencneSSHAgent.jwt.models
{
    class JWTContainerModel : IAuthContainerModel
    {
        public string SecurityAlgorithm { get; set; } = SecurityAlgorithms.HmacSha256Signature;
        public int ExpireMinutes { get; set; } = (60 * 24);

        public Claim[] Claims { get; set; }

        #region Private Methods
        public static JWTContainerModel GetJWTContainerModel(string username, int role)
        {
            return new JWTContainerModel()
            {
                Claims = new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, username),
                    new Claim(ClaimTypes.Role, role.ToString())
                }
            };
        }
        #endregion
    }
}
