using SoSicencneSSHAgent.jwt.models;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace SoSicencneSSHAgent.jwt.managers
{
    interface IAuthService
    {
        string SecretKey { get; set; }

        bool IsTokenValid(string token);
        string GenerateToken(IAuthContainerModel model);
        IEnumerable<Claim> GetTokenClaims(string token);
    }
}
