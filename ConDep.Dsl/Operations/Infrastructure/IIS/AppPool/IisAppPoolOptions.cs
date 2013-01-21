using ConDep.Dsl.Builders;

namespace ConDep.Dsl.Operations.Infrastructure.IIS.AppPool
{
    public class IisAppPoolOptions : IOfferIisAppPoolOptions
    {
        private readonly IisAppPoolOptionsValues _values = new IisAppPoolOptionsValues();

        IOfferIisAppPoolOptions IOfferIisAppPoolOptions.NetFrameworkVersion(NetFrameworkVersion version)
        {
            _values.NetFrameworkVersion = version;
            return this;
        }

        IOfferIisAppPoolOptions IOfferIisAppPoolOptions.ManagedPipeline(ManagedPipeline pipeline)
        {
            _values.ManagedPipeline = pipeline;
            return this;
        }

        IOfferIisAppPoolOptions IOfferIisAppPoolOptions.IdentityUsername(string userName)
        {
            _values.IdentityUsername = userName;
            return this;
        }

        IOfferIisAppPoolOptions IOfferIisAppPoolOptions.IdentityPassword(string password)
        {
            _values.IdentityPassword = password;
            return this;
        }

        IOfferIisAppPoolOptions IOfferIisAppPoolOptions.Enable32Bit(bool enable)
        {
            _values.Enable32Bit = enable;
            return this;
        }

        IOfferIisAppPoolOptions IOfferIisAppPoolOptions.IdleTimeoutInMinutes(int minutes)
        {
            _values.IdleTimeoutInMinutes = minutes;
            return this;
        }

        IOfferIisAppPoolOptions IOfferIisAppPoolOptions.LoadUserProfile(bool load)
        {
            _values.LoadUserProfile = load;
            return this;
        }

        IOfferIisAppPoolOptions IOfferIisAppPoolOptions.RecycleTimeInMinutes(int minutes)
        {
            _values.RecycleTimeInMinutes = minutes;
            return this;
        }

        public IisAppPoolOptionsValues Values { get { return _values; } }

        public class IisAppPoolOptionsValues
        {
            public NetFrameworkVersion? NetFrameworkVersion { get; set; }

            public ManagedPipeline? ManagedPipeline { get; set; }

            public string IdentityUsername { get; set; }

            public string IdentityPassword { get; set; }

            public bool? Enable32Bit { get; set; }

            public int? IdleTimeoutInMinutes { get; set; }

            public bool? LoadUserProfile { get; set; }

            public int? RecycleTimeInMinutes { get; set; }
        }

    }
}