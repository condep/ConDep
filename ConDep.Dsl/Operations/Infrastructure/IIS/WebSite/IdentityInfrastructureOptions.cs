namespace ConDep.Dsl.Operations.Infrastructure.IIS.WebSite
{
    public class IdentityInfrastructureOptions
    {
        private readonly IOfferIisAppPoolOptions _iisAppPoolOptions;

        public IdentityInfrastructureOptions(IOfferIisAppPoolOptions iisAppPoolOptions)
        {
            _iisAppPoolOptions = iisAppPoolOptions;
        }

        public IdentityInfrastructureOptions UserName(string userName)
        {
            _iisAppPoolOptions.IdentityUsername(userName);
            return this;
        }

        public IdentityInfrastructureOptions Password(string password)
        {
            _iisAppPoolOptions.IdentityPassword(password);
            return this;
        }
    }
}