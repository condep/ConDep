using ConDep.Dsl.Providers.IIS.Binding;

namespace ConDep.Dsl
{
    public class IisBindingInfrastructureOptions
    {
        private readonly HttpBindingInfrastructureProvider _binding;

        public IisBindingInfrastructureOptions(HttpBindingInfrastructureProvider binding)
        {
            _binding = binding;
        }

        public IisBindingInfrastructureOptions HostHeader(string hostname)
        {
            _binding.HostHeader = hostname;
            return this;
        }

        public IisBindingInfrastructureOptions Ip(string ip)
        {
            _binding.Ip = ip;
            return this;
        }
    }
}