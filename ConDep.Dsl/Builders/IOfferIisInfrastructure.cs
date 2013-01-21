namespace ConDep.Dsl.Builders
{
    public interface IOfferIisInfrastructure
    {
        /// <summary>
        /// Enables additional IIS role services to default role services
        /// </summary>
        IOfferIisInfrastructureRoleServices Include { get; }

        /// <summary>
        /// Disables IIS role services from default role services
        /// </summary>
        IOfferIisInfrastructureRoleServices RemoveIfExist { get; }
    }
}