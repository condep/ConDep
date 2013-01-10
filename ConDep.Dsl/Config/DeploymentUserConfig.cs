using System;

namespace ConDep.Dsl.Config
{
    public class DeploymentUserConfig
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsDefined { get { return !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password); } }
        
        public string UserNameWithoutDomain
        {
            get
            {
                if (string.IsNullOrWhiteSpace(UserName))
                    return string.Empty;

                var split = UserName.Split(new[] { '\\' });
                return split.Length == 1 ? UserName : split[1];
            }
        }

        public string Domain
        {
            get
            {
                if (string.IsNullOrWhiteSpace(UserName))
                    return string.Empty;

                var split = UserName.Split(new[] { '\\' });
                return split.Length == 1 ? string.Empty : split[0];
            }
        }
    }
}