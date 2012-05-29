namespace ConDep.Dsl
{
    public class BindingOptions
    {
        private readonly IisBinding _binding;

        public BindingOptions(IisBinding binding)
        {
            _binding = binding;
        }

        public BindingOptions HostHeader(string hostname)
        {
            _binding.HostHeader = hostname;
            return this;
        }

        public BindingOptions Ip(string ip)
        {
            _binding.Ip = ip;
            return this;
        }
    }
}