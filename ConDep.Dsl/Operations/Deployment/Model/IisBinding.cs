using System;

namespace ConDep.Dsl
{
    public class IisBinding
    {
        public IisBinding(BindingType bindingType, int port)
        {
            BindingType = bindingType;
            Port = port;
        }

        public int Port { get; set; }

        public BindingType BindingType { get; set; }

        public string HostHeader { get; set; }

        public string Ip { get; set; }

        public string CertificateCommonName { get; set; }
    }
}