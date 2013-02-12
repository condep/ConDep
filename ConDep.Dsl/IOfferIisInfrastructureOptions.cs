namespace ConDep.Dsl
{
    public interface IOfferIisInfrastructureOptions
    {
        /// <summary>
        /// Let you include IIS roles
        /// </summary>
        IOfferIisInfrastructureRoleOptions Include { get; }

        /// <summary>
        /// Let you remove IIS roles if installed
        /// </summary>
        IOfferIisInfrastructureRoleOptions RemoveIfPresent { get; }
    }
}