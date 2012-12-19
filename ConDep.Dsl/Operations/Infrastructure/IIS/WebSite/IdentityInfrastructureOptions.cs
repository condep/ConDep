namespace ConDep.Dsl.Operations.Infrastructure.IIS.WebSite
{
    public class IdentityInfrastructureOptions
    {
        private readonly IisAppPoolOptions _iisAppPoolOptions;

        public IdentityInfrastructureOptions(IisAppPoolOptions iisAppPoolOptions)
        {
            _iisAppPoolOptions = iisAppPoolOptions;
        }

        public IdentityInfrastructureOptions UserName(string userName)
        {
            _iisAppPoolOptions.IdentityUsername = userName;
            return this;
        }

        public IdentityInfrastructureOptions Password(string password)
        {
            _iisAppPoolOptions.IdentityPassword = password;
            return this;
        }
    }
}