using System;
using ConDep.Dsl.Builders;

namespace ConDep.Dsl.Operations.Infrastructure
{
    public class IisInfrastructureOptions
    {
        private readonly IisInfrastructureOperation _operation;

        public IisInfrastructureOptions(IisInfrastructureOperation operation)
        {
            _operation = operation;
        }

        public IOfferIisInfrastructureOptions Include
        {
            get { return new IisInfrastructureIncludeOptions(_operation); }
        }

        public IOfferIisInfrastructureOptions RemoveIfPresent
        {
            get { return new IisInfrastructureExcludeOptions(_operation); }
        }
    }

    public class IisInfrastructureExcludeOptions : IOfferIisInfrastructureOptions
    {
        private readonly IisInfrastructureOperation _iisOperation;

        public IisInfrastructureExcludeOptions(IisInfrastructureOperation iisOperation)
        {
            _iisOperation = iisOperation;
        }

        public IOfferIisInfrastructureOptions HttpRedirect()
        {
            _iisOperation.RemoveRoleService("Web-Http-Redirect");
            return this;
        }

        public IOfferIisInfrastructureOptions DavPublishing()
        {
            _iisOperation.RemoveRoleService("Web-DAV-Publishing");
            return this;
        }

        public IOfferIisInfrastructureOptions AspNet()
        {
            _iisOperation.RemoveRoleService("Web-Asp-Net");
            return this;
        }

        public IOfferIisInfrastructureOptions Asp()
        {
            _iisOperation.RemoveRoleService("Web-ASP");
            return this;
        }

        public IOfferIisInfrastructureOptions Cgi()
        {
            _iisOperation.RemoveRoleService("Web-CGI");
            return this;
        }

        public IOfferIisInfrastructureOptions ServerSideIncludes()
        {
            _iisOperation.RemoveRoleService("Web-Includes");
            return this;
        }

        public IOfferIisInfrastructureOptions LoggingTools()
        {
            _iisOperation.RemoveRoleService("Web-Log-Libraries");
            return this;
        }

        public IOfferIisInfrastructureOptions Tracing()
        {
            _iisOperation.RemoveRoleService("Web-Http-Tracing");
            return this;
        }

        public IOfferIisInfrastructureOptions CustomLogging()
        {
            _iisOperation.RemoveRoleService("Web-Custom-Logging");
            return this;
        }

        public IOfferIisInfrastructureOptions OdbcLogging()
        {
            _iisOperation.RemoveRoleService("Web-ODBC-Logging");
            return this;
        }

        public IOfferIisInfrastructureOptions BasicAuth()
        {
            _iisOperation.RemoveRoleService("Web-Basic-Auth");
            return this;
        }

        public IOfferIisInfrastructureOptions WindowsAuth()
        {
            _iisOperation.RemoveRoleService("Web-Windows-Auth");
            return this;
        }

        public IOfferIisInfrastructureOptions DigestAuth()
        {
            _iisOperation.RemoveRoleService("Web-Digest-Auth");
            return this;
        }

        public IOfferIisInfrastructureOptions ActiveDirectoryClientCertMappingAuth()
        {
            _iisOperation.RemoveRoleService("Web-Client-Auth");
            return this;
        }

        public IOfferIisInfrastructureOptions IisClientCertMappingAuth()
        {
            _iisOperation.RemoveRoleService("Web-Cert-Auth");
            return this;
        }

        public IOfferIisInfrastructureOptions UrlAuth()
        {
            _iisOperation.RemoveRoleService("Web-Url-Auth");
            return this;
        }

        public IOfferIisInfrastructureOptions IpSecurity()
        {
            _iisOperation.RemoveRoleService("Web-IP-Security");
            return this;
        }

        public IOfferIisInfrastructureOptions DynamicContentCompression()
        {
            _iisOperation.RemoveRoleService("Web-Dyn-Compression");
            return this;
        }

        public IOfferIisInfrastructureOptions MngmntScriptsAndTools()
        {
            _iisOperation.RemoveRoleService("Web-Scripting-Tools");
            return this;
        }

        public IOfferIisInfrastructureOptions MgmtService()
        {
            _iisOperation.RemoveRoleService("Web-Mgmt-Service");
            return this;
        }
    }
}