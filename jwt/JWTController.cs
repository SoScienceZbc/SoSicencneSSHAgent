using Microsoft.Extensions.Configuration;
using SoSicencneSSHAgent.jwt.managers;
using SoSicencneSSHAgent.jwt.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace SoSicencneSSHAgent.jwt
{
    class JWTController
    {
        private static IConfiguration _config;
        private IConfiguration config { get
            {
                if (_config == null)
                    _config = new ConfigurationBuilder().AddJsonFile("./SSHAgentConfig.json").Build();

                return _config;
            } 
        }

        /// <summary>
        /// Creates a token using the username and role
        /// </summary>
        /// <param name="username">The username of the user</param>
        /// <param name="roleType">The role of the user</param>
        /// <returns>A JWT token</returns>
        public string CreateToken(string username, RoleType roleType)
        {
            IAuthContainerModel model = JWTContainerModel.GetJWTContainerModel( username, (int)roleType);
            IAuthService authService = new JWTService(GetSecret());

            string token = authService.GenerateToken(model);
            return token;
        }

        /// <summary>
        /// Looks if the jwt token is valid
        /// </summary>
        /// <param name="token">The jwt token</param>
        /// <returns>If it was valid or not</returns>
        public bool ValidateJWT(string token)
        {
            IAuthService authService = new JWTService(GetSecret());
            return authService.IsTokenValid(token);
        }

        /// <summary>
        /// Looks if the token is valid and if it is over the min role
        /// </summary>
        /// <param name="token">The jwt token</param>
        /// <param name="minRole">The min role</param>
        /// <returns>If the token meets the min role</returns>
        public bool ValidateRoleLevel(string token, RoleType minRole)
        {
            IAuthService authService = new JWTService(GetSecret());
            if (!authService.IsTokenValid(token))
                return false;
            else
            {
                List<Claim> claims = authService.GetTokenClaims(token).ToList();

                return (Convert.ToInt32(claims.FirstOrDefault(e => e.Type.Equals(ClaimTypes.Role)).Value) >= (int)minRole);
            }
        }

        /// <summary>
        /// Gets the username out from the token
        /// </summary>
        /// <param name="token">The jwttoken</param>
        /// <returns>The username</returns>
        public string GetUsername(string token)
        {
            IAuthService authService = new JWTService(GetSecret());
            if (!authService.IsTokenValid(token))
                return "";
            else
            {
                List<Claim> claims = authService.GetTokenClaims(token).ToList();

                return (claims.FirstOrDefault(e => e.Type.Equals(ClaimTypes.NameIdentifier)).Value);
            }
        }

        #region Private Method
        private string GetSecret()
        {
            return config.GetSection("JWTConfig")["Secret"];
        }
        #endregion
    }
}
