using System;
using ConDep.Dsl.Builders;

namespace ConDep.Dsl.Operations.Infrastructure
{
    public class IisInfrastructureIncludeOptions : IOfferIisInfrastructureOptions
    {
        private readonly IisInfrastructureOperation _iisOperation;

        public IisInfrastructureIncludeOptions(IisInfrastructureOperation iisOperation)
        {
            _iisOperation = iisOperation;
        }

        public IOfferIisInfrastructureOptions HttpRedirect()
        {
            _iisOperation.AddRoleService("Web-Http-Redirect");
            return this;
        }

        public IOfferIisInfrastructureOptions DavPublishing()
        {
            _iisOperation.AddRoleService("Web-DAV-Publishing");
            return this;
        }

        public IOfferIisInfrastructureOptions AspNet()
        {
            _iisOperation.AddRoleService("Web-Asp-Net");
            return this;
        }

        public IOfferIisInfrastructureOptions Asp()
        {
            _iisOperation.AddRoleService("Web-ASP");
            return this;
        }

        public IOfferIisInfrastructureOptions Cgi()
        {
            _iisOperation.AddRoleService("Web-CGI");
            return this;
        }

        public IOfferIisInfrastructureOptions ServerSideIncludes()
        {
            _iisOperation.AddRoleService("Web-Includes");
            return this;
        }

        public IOfferIisInfrastructureOptions LoggingTools()
        {
            _iisOperation.AddRoleService("Web-Log-Libraries");
            return this;
        }

        public IOfferIisInfrastructureOptions Tracing()
        {
            _iisOperation.AddRoleService("Web-Http-Tracing");
            return this;
        }

        public IOfferIisInfrastructureOptions CustomLogging()
        {
            _iisOperation.AddRoleService("Web-Custom-Logging");
            return this;
        }

        public IOfferIisInfrastructureOptions OdbcLogging()
        {
            _iisOperation.AddRoleService("Web-ODBC-Logging");
            return this;
        }

        public IOfferIisInfrastructureOptions BasicAuth()
        {
            _iisOperation.AddRoleService("Web-Basic-Auth");
            return this;
        }

        public IOfferIisInfrastructureOptions WindowsAuth()
        {
            _iisOperation.AddRoleService("Web-Windows-Auth");
            return this;
        }

        public IOfferIisInfrastructureOptions DigestAuth()
        {
            _iisOperation.AddRoleService("Web-Digest-Auth");
            return this;
        }

        public IOfferIisInfrastructureOptions ActiveDirectoryClientCertMappingAuth()
        {
            _iisOperation.AddRoleService("Web-Client-Auth");
            return this;
        }

        public IOfferIisInfrastructureOptions IisClientCertMappingAuth()
        {
            _iisOperation.AddRoleService("Web-Cert-Auth");
            return this;
        }

        public IOfferIisInfrastructureOptions UrlAuth()
        {
            _iisOperation.AddRoleService("Web-Url-Auth");
            return this;
        }

        public IOfferIisInfrastructureOptions IpSecurity()
        {
            _iisOperation.AddRoleService("Web-IP-Security");
            return this;
        }

        public IOfferIisInfrastructureOptions DynamicContentCompression()
        {
            _iisOperation.AddRoleService("Web-Dyn-Compression");
            return this;
        }

        public IOfferIisInfrastructureOptions MngmntScriptsAndTools()
        {
            _iisOperation.AddRoleService("Web-Scripting-Tools");
            return this;
        }

        public IOfferIisInfrastructureOptions MgmtService()
        {
            _iisOperation.AddRoleService("Web-Mgmt-Service");
            return this;
        }
    }
}