namespace ConDep.Dsl.Operations.Infrastructure.IIS
{
    public class ApplicationPool
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