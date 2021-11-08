using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace SoSicencneSSHAgent.jwt.models
{
    interface IAuthContainerModel
    {
        #region Members
        string SecurityAlgorithm { get; set; }
        int ExpireMinutes { get; set; }

        Claim[] Claims { get; set; }
        #endregion
    }
}
