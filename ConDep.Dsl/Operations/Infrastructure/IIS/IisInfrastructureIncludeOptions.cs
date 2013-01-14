using System;

namespace ConDep.Dsl.Operations.Infrastructure
{
    public class IisInfrastructureIncludeOptions
    {
        private readonly IisInfrastructureOperation _iisOperation;

        public IisInfrastructureIncludeOptions(IisInfrastructureOperation iisOperation)
        {
            _iisOperation = iisOperation;
        }

        public IisInfrastructureIncludeOptions HttpRedirect()
        {
            _iisOperation.AddRoleService("Web-Http-Redirect");
            return this;
        }

        public IisInfrastructureIncludeOptions DAVPublishing()
        {
            _iisOperation.AddRoleService("Web-DAV-Publishing");
            return this;
        }

        public IisInfrastructureIncludeOptions AspNet()
        {
            _iisOperation.AddRoleService("Web-Asp-Net");
            return this;
        }

        public IisInfrastructureIncludeOptions ASP()
        {
            _iisOperation.AddRoleService("Web-ASP");
            return this;
        }

        public IisInfrastructureIncludeOptions CGI()
        {
            _iisOperation.AddRoleService("Web-CGI");
            return this;
        }

        public IisInfrastructureIncludeOptions ServerSideIncludes()
        {
            _iisOperation.AddRoleService("Web-Includes");
            return this;
        }

        public IisInfrastructureIncludeOptions LogLibraries()
        {
            _iisOperation.AddRoleService("Web-Log-Libraries");
            return this;
        }

        public IisInfrastructureIncludeOptions HttpTracing()
        {
            _iisOperation.AddRoleService("Web-Http-Tracing");
            return this;
        }

        public IisInfrastructureIncludeOptions CustomLogging()
        {
            _iisOperation.AddRoleService("Web-Custom-Logging");
            return this;
        }

        public IisInfrastructureIncludeOptions ODBCLogging()
        {
            _iisOperation.AddRoleService("Web-ODBC-Logging");
            return this;
        }

        public IisInfrastructureIncludeOptions BasicAuth()
        {
            _iisOperation.AddRoleService("Web-Basic-Auth");
            return this;
        }

        public IisInfrastructureIncludeOptions WindowsAuth()
        {
            _iisOperation.AddRoleService("Web-Windows-Auth");
            return this;
        }

        public IisInfrastructureIncludeOptions DigestAuth()
        {
            _iisOperation.AddRoleService("Web-Digest-Auth");
            return this;
        }

        public IisInfrastructureIncludeOptions ClientAuth()
        {
            _iisOperation.AddRoleService("Web-Client-Auth");
            return this;
        }

        public IisInfrastructureIncludeOptions CertAuth()
        {
            _iisOperation.AddRoleService("Web-Cert-Auth");
            return this;
        }

        public IisInfrastructureIncludeOptions UrlAuth()
        {
            _iisOperation.AddRoleService("Web-Url-Auth");
            return this;
        }

        public IisInfrastructureIncludeOptions IPSecurity()
        {
            _iisOperation.AddRoleService("Web-IP-Security");
            return this;
        }

        public IisInfrastructureIncludeOptions DynamicCompression()
        {
            _iisOperation.AddRoleService("Web-Dyn-Compression");
            return this;
        }

        public IisInfrastructureIncludeOptions ScriptingTools()
        {
            _iisOperation.AddRoleService("Web-Scripting-Tools");
            return this;
        }

        public IisInfrastructureIncludeOptions MgmtService()
        {
            _iisOperation.AddRoleService("Web-Mgmt-Service");
            return this;
        }
    }
}