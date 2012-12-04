namespace ConDep.Dsl.Operations.Infrastructure.IIS.Binding
{
    public class IisBindingInfrastructureOptions
    {
        private readonly HttpBindingInfrastructureProvider _httpBinding;

        public IisBindingInfrastructureOptions(HttpBindingInfrastructureProvider httpBinding)
        {
            _httpBinding = httpBinding;
        }

        public IisBindingInfrastructureOptions HttpHostHeader(string hostname)
        {
            _httpBinding.HostHeader = hostname;
            return this;
        }

        public IisBindingInfrastructureOptions HttpIp(string ip)
        {
            _httpBinding.Ip = ip;
            return this;
        }
    }
}