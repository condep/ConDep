using System;
using ConDep.Dsl.Builders;
using ConDep.Dsl.Operations.Infrastructure.IIS;
using ConDep.Dsl.Operations.Windows;

namespace ConDep.Dsl.Operations.Infrastructure
{
    public class IisInfrastructureIncludeOptions : IOfferIisInfrastructureRoleOptions
    {
        private readonly IisInfrastructureOperation _iisOperation;

        public IisInfrastructureIncludeOptions(IisInfrastructureOperation iisOperation)
        {
            _iisOperation = iisOperation;
        }

        public IOfferIisInfrastructureRoleOptions HttpRedirect()
        {
            _iisOperation.AddRoleService("Web-Http-Redirect");
            return this;
        }

        public IOfferIisInfrastructureRoleOptions DavPublishing()
        {
            _iisOperation.AddRoleService("Web-DAV-Publishing");
            return this;
        }

        public IOfferIisInfrastructureRoleOptions AspNet()
        {
            _iisOperation.AddRoleService("Web-Asp-Net");
            return this;
        }

        public IOfferIisInfrastructureRoleOptions Asp()
        {
            _iisOperation.AddRoleService("Web-ASP");
            return this;
        }

        public IOfferIisInfrastructureRoleOptions Cgi()
        {
            _iisOperation.AddRoleService("Web-CGI");
            return this;
        }

        public IOfferIisInfrastructureRoleOptions ServerSideIncludes()
        {
            _iisOperation.AddRoleService("Web-Includes");
            return this;
        }

        public IOfferIisInfrastructureRoleOptions LoggingTools()
        {
            _iisOperation.AddRoleService("Web-Log-Libraries");
            return this;
        }

        public IOfferIisInfrastructureRoleOptions Tracing()
        {
            _iisOperation.AddRoleService("Web-Http-Tracing");
            return this;
        }

        public IOfferIisInfrastructureRoleOptions CustomLogging()
        {
            _iisOperation.AddRoleService("Web-Custom-Logging");
            return this;
        }

        public IOfferIisInfrastructureRoleOptions OdbcLogging()
        {
            _iisOperation.AddRoleService("Web-ODBC-Logging");
            return this;
        }

        public IOfferIisInfrastructureRoleOptions BasicAuth()
        {
            _iisOperation.AddRoleService("Web-Basic-Auth");
            return this;
        }

        public IOfferIisInfrastructureRoleOptions WindowsAuth()
        {
            _iisOperation.AddRoleService("Web-Windows-Auth");
            return this;
        }

        public IOfferIisInfrastructureRoleOptions DigestAuth()
        {
            _iisOperation.AddRoleService("Web-Digest-Auth");
            return this;
        }

        public IOfferIisInfrastructureRoleOptions ActiveDirectoryClientCertMappingAuth()
        {
            _iisOperation.AddRoleService("Web-Client-Auth");
            return this;
        }

        public IOfferIisInfrastructureRoleOptions IisClientCertMappingAuth()
        {
            _iisOperation.AddRoleService("Web-Cert-Auth");
            return this;
        }

        public IOfferIisInfrastructureRoleOptions UrlAuth()
        {
            _iisOperation.AddRoleService("Web-Url-Auth");
            return this;
        }

        public IOfferIisInfrastructureRoleOptions IpSecurity()
        {
            _iisOperation.AddRoleService("Web-IP-Security");
            return this;
        }

        public IOfferIisInfrastructureRoleOptions DynamicContentCompression()
        {
            _iisOperation.AddRoleService("Web-Dyn-Compression");
            return this;
        }

        public IOfferIisInfrastructureRoleOptions MngmntScriptsAndTools()
        {
            _iisOperation.AddRoleService("Web-Scripting-Tools");
            return this;
        }

        public IOfferIisInfrastructureRoleOptions MgmtService()
        {
            _iisOperation.AddRoleService("Web-Mgmt-Service");
            return this;
        }
    }
}