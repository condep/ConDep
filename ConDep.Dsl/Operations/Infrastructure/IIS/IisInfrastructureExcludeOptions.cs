using ConDep.Dsl.Operations.Infrastructure.IIS;
using ConDep.Dsl.Operations.Windows;

namespace ConDep.Dsl.Operations.Infrastructure
{
    public class IisInfrastructureExcludeOptions : IOfferIisInfrastructureRoleOptions
    {
        private readonly IisInfrastructureOperation _iisOperation;

        public IisInfrastructureExcludeOptions(IisInfrastructureOperation iisOperation)
        {
            _iisOperation = iisOperation;
        }

        public IOfferIisInfrastructureRoleOptions HttpRedirect()
        {
            _iisOperation.RemoveRoleService("Web-Http-Redirect");
            return this;
        }

        public IOfferIisInfrastructureRoleOptions DavPublishing()
        {
            _iisOperation.RemoveRoleService("Web-DAV-Publishing");
            return this;
        }

        public IOfferIisInfrastructureRoleOptions AspNet()
        {
            _iisOperation.RemoveRoleService("Web-Asp-Net");
            return this;
        }

        public IOfferIisInfrastructureRoleOptions Asp()
        {
            _iisOperation.RemoveRoleService("Web-ASP");
            return this;
        }

        public IOfferIisInfrastructureRoleOptions Cgi()
        {
            _iisOperation.RemoveRoleService("Web-CGI");
            return this;
        }

        public IOfferIisInfrastructureRoleOptions ServerSideIncludes()
        {
            _iisOperation.RemoveRoleService("Web-Includes");
            return this;
        }

        public IOfferIisInfrastructureRoleOptions LoggingTools()
        {
            _iisOperation.RemoveRoleService("Web-Log-Libraries");
            return this;
        }

        public IOfferIisInfrastructureRoleOptions Tracing()
        {
            _iisOperation.RemoveRoleService("Web-Http-Tracing");
            return this;
        }

        public IOfferIisInfrastructureRoleOptions CustomLogging()
        {
            _iisOperation.RemoveRoleService("Web-Custom-Logging");
            return this;
        }

        public IOfferIisInfrastructureRoleOptions OdbcLogging()
        {
            _iisOperation.RemoveRoleService("Web-ODBC-Logging");
            return this;
        }

        public IOfferIisInfrastructureRoleOptions BasicAuth()
        {
            _iisOperation.RemoveRoleService("Web-Basic-Auth");
            return this;
        }

        public IOfferIisInfrastructureRoleOptions WindowsAuth()
        {
            _iisOperation.RemoveRoleService("Web-Windows-Auth");
            return this;
        }

        public IOfferIisInfrastructureRoleOptions DigestAuth()
        {
            _iisOperation.RemoveRoleService("Web-Digest-Auth");
            return this;
        }

        public IOfferIisInfrastructureRoleOptions ActiveDirectoryClientCertMappingAuth()
        {
            _iisOperation.RemoveRoleService("Web-Client-Auth");
            return this;
        }

        public IOfferIisInfrastructureRoleOptions IisClientCertMappingAuth()
        {
            _iisOperation.RemoveRoleService("Web-Cert-Auth");
            return this;
        }

        public IOfferIisInfrastructureRoleOptions UrlAuth()
        {
            _iisOperation.RemoveRoleService("Web-Url-Auth");
            return this;
        }

        public IOfferIisInfrastructureRoleOptions IpSecurity()
        {
            _iisOperation.RemoveRoleService("Web-IP-Security");
            return this;
        }

        public IOfferIisInfrastructureRoleOptions DynamicContentCompression()
        {
            _iisOperation.RemoveRoleService("Web-Dyn-Compression");
            return this;
        }

        public IOfferIisInfrastructureRoleOptions MngmntScriptsAndTools()
        {
            _iisOperation.RemoveRoleService("Web-Scripting-Tools");
            return this;
        }

        public IOfferIisInfrastructureRoleOptions MgmtService()
        {
            _iisOperation.RemoveRoleService("Web-Mgmt-Service");
            return this;
        }
    }
}