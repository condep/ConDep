using System;

namespace ConDep.Dsl.Core
{
    public class DeploymentUser
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public bool IsDefined { get { return !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password); } }
    }
}