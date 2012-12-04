namespace ConDep.Dsl.Operations.Infrastructure.IIS.WebSite
{
    public class IdentityInfrastructureOptions
    {
        private readonly ApplicationPool _applicationPool;

        public IdentityInfrastructureOptions(ApplicationPool applicationPool)
        {
            _applicationPool = applicationPool;
        }

        public IdentityInfrastructureOptions UserName(string userName)
        {
            _applicationPool.IdentityUsername = userName;
            return this;
        }

        public IdentityInfrastructureOptions Password(string password)
        {
            _applicationPool.IdentityPassword = password;
            return this;
        }
    }
}