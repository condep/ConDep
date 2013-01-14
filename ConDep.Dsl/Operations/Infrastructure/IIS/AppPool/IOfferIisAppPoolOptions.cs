namespace ConDep.Dsl.Operations.Infrastructure.IIS
{
    public interface IOfferIisAppPoolOptions
    {
        IOfferIisAppPoolOptions NetFrameworkVersion(NetFrameworkVersion version);
        IOfferIisAppPoolOptions ManagedPipeline(ManagedPipeline pipeline);
        IOfferIisAppPoolOptions IdentityUsername(string userName);
        IOfferIisAppPoolOptions IdentityPassword(string password);
        IOfferIisAppPoolOptions Enable32Bit(bool enable);
        IOfferIisAppPoolOptions IdleTimeoutInMinutes(int minutes);
        IOfferIisAppPoolOptions LoadUserProfile(bool load);
        IOfferIisAppPoolOptions RecycleTimeInMinutes(int minutes);
    }
}