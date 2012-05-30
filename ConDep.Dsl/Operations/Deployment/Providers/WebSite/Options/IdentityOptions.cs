using System;

namespace ConDep.Dsl
{
    public class IdentityOptions
    {
        private readonly ApplicationPool _applicationPool;

        public IdentityOptions(ApplicationPool applicationPool)
        {
            _applicationPool = applicationPool;
        }

        public IdentityOptions UserName(string userName)
        {
            _applicationPool.IdentityUsername = userName;
            return this;
        }

        public IdentityOptions Password(string password)
        {
            _applicationPool.IdentityPassword = password;
            return this;
        }
    }
}