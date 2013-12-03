using System;
using System.Linq;
using System.Security;

namespace ConDep.Dsl.Config
{
    [Serializable]
    public class DeploymentUserConfig
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsDefined() { return !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password); }
    }
}