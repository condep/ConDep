namespace ConDep.Dsl
{
    public interface IOfferIisInfrastructureOptions
    {
        IOfferIisInfrastructureRoleOptions Include { get; }
        IOfferIisInfrastructureRoleOptions RemoveIfPresent { get; }
    }
}