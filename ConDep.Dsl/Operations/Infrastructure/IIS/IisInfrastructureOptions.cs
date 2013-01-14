using System;

namespace ConDep.Dsl.Operations.Infrastructure
{
    public class IisInfrastructureOptions
    {
        private readonly IisInfrastructureOperation _operation;

        public IisInfrastructureOptions(IisInfrastructureOperation operation)
        {
            _operation = operation;
        }

        public IisInfrastructureIncludeOptions Include
        {
            get { return new IisInfrastructureIncludeOptions(_operation); }
        }

        public IisInfrastructureExcludeOptions RemoveIfPresent
        {
            get { return new IisInfrastructureExcludeOptions(_operation); }
        }
    }

    public class IisInfrastructureExcludeOptions
    {
        private readonly IisInfrastructureOperation _iisOperation;

        public IisInfrastructureExcludeOptions(IisInfrastructureOperation iisOperation)
        {
            _iisOperation = iisOperation;
        }

        public IisInfrastructureExcludeOptions HttpRedirect()
        {
            _iisOperation.RemoveRoleService("Web-Http-Redirect");
            return this;
        }

        public IisInfrastructureExcludeOptions DAVPublishing()
        {
            _iisOperation.RemoveRoleService("Web-DAV-Publishing");
            return this;
        }

        public IisInfrastructureExcludeOptions AspNet()
        {
            _iisOperation.RemoveRoleService("Web-Asp-Net");
            return this;
        }

        public IisInfrastructureExcludeOptions ASP()
        {
            _iisOperation.RemoveRoleService("Web-ASP");
            return this;
        }

        public IisInfrastructureExcludeOptions CGI()
        {
            _iisOperation.RemoveRoleService("Web-CGI");
            return this;
        }

        public IisInfrastructureExcludeOptions ServerSideIncludes()
        {
            _iisOperation.RemoveRoleService("Web-Includes");
            return this;
        }

        public IisInfrastructureExcludeOptions LogLibraries()
        {
            _iisOperation.RemoveRoleService("Web-Log-Libraries");
            return this;
        }

        public IisInfrastructureExcludeOptions HttpTracing()
        {
            _iisOperation.RemoveRoleService("Web-Http-Tracing");
            return this;
        }

        public IisInfrastructureExcludeOptions CustomLogging()
        {
            _iisOperation.RemoveRoleService("Web-Custom-Logging");
            return this;
        }

        public IisInfrastructureExcludeOptions ODBCLogging()
        {
            _iisOperation.RemoveRoleService("Web-ODBC-Logging");
            return this;
        }

        public IisInfrastructureExcludeOptions BasicAuth()
        {
            _iisOperation.RemoveRoleService("Web-Basic-Auth");
            return this;
        }

        public IisInfrastructureExcludeOptions WindowsAuth()
        {
            _iisOperation.RemoveRoleService("Web-Windows-Auth");
            return this;
        }

        public IisInfrastructureExcludeOptions DigestAuth()
        {
            _iisOperation.RemoveRoleService("Web-Digest-Auth");
            return this;
        }

        public IisInfrastructureExcludeOptions ClientAuth()
        {
            _iisOperation.RemoveRoleService("Web-Client-Auth");
            return this;
        }

        public IisInfrastructureExcludeOptions CertAuth()
        {
            _iisOperation.RemoveRoleService("Web-Cert-Auth");
            return this;
        }

        public IisInfrastructureExcludeOptions UrlAuth()
        {
            _iisOperation.RemoveRoleService("Web-Url-Auth");
            return this;
        }

        public IisInfrastructureExcludeOptions IPSecurity()
        {
            _iisOperation.RemoveRoleService("Web-IP-Security");
            return this;
        }

        public IisInfrastructureExcludeOptions DynamicCompression()
        {
            _iisOperation.RemoveRoleService("Web-Dyn-Compression");
            return this;
        }

        public IisInfrastructureExcludeOptions ScriptingTools()
        {
            _iisOperation.RemoveRoleService("Web-Scripting-Tools");
            return this;
        }

        public IisInfrastructureExcludeOptions MgmtService()
        {
            _iisOperation.RemoveRoleService("Web-Mgmt-Service");
            return this;
        }
    }
}