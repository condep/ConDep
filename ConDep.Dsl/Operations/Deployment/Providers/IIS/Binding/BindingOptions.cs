using ConDep.Dsl.Providers.IIS.Binding;

namespace ConDep.Dsl
{
    public class BindingOptions
    {
        private readonly HttpBindingProvider _binding;

        public BindingOptions(HttpBindingProvider binding)
        {
            _binding = binding;
        }

        //public BindingOptions HostHeader(string hostname)
        //{
        //    _binding.HostHeader = hostname;
        //    return this;
        //}

        //public BindingOptions Ip(string ip)
        //{
        //    _binding.Ip = ip;
        //    return this;
        //}
    }
}