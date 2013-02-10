namespace ConDep.Dsl.Operations.Infrastructure
{
    public class IisInfrastructureOptions : IOfferIisInfrastructureOptions
    {
        private readonly IisInfrastructureOperation _operation;

        public IisInfrastructureOptions(IisInfrastructureOperation operation)
        {
            _operation = operation;
        }

        public IOfferIisInfrastructureRoleOptions Include
        {
            get { return new IisInfrastructureIncludeOptions(_operation); }
        }

        public IOfferIisInfrastructureRoleOptions RemoveIfPresent
        {
            get { return new IisInfrastructureExcludeOptions(_operation); }
        }
    }
}