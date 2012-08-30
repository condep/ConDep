using System;

namespace ConDep.Dsl
{
    public class ConDepWebSiteBinding
    {
        private readonly string _port;
        private readonly string _ip;
        private readonly string _hostHeader;
        private readonly string _bindingType;

        public ConDepWebSiteBinding(string bindingType, string port, string ip, string hostHeader)
        {
            _bindingType = bindingType;
            _port = port;
            _ip = ip;
            _hostHeader = hostHeader;
        }

        public string HostHeader
        {
            get { return _hostHeader; }
        }

        public string Ip
        {
            get { return _ip; }
        }

        public string Port
        {
            get { return _port; }
        }

        public string BindingType
        {
            get {
                return _bindingType;
            }
        }
    }
}