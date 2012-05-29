using System;

namespace ConDep.Dsl
{
    public class IisBinding
    {
        private BindingType _bindingType;
        private int _port;

        public IisBinding(BindingType bindingType, int port)
        {
            _bindingType = bindingType;
            _port = port;
        }

        public int Port
        {
            get { return _port; }
            set { _port = value; }
        }

        public BindingType BindingType
        {
            get { return _bindingType; }
            set { _bindingType = value; }
        }

        public string HostHeader { get; set; }

        public string Ip { get; set; }

        public string CertificateCommonName { get; set; }
    }
}