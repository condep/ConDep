using System;
using ConDep.Dsl.WebDeployProviders.Deployment.CopyDir;
using ConDep.Dsl.WebDeployProviders.Deployment.NServiceBus;

namespace ConDep.Dsl.Application.Deployment
{
    public class Deployment : IDeploy
    {
        private readonly IManageExecutionSequence _sequence;
        private readonly IDeploySslCertificate _sslCertDeployment;

        public Deployment(IManageExecutionSequence sequence, IDeploySslCertificate sslCertDeployment)
        {
            _sequence = sequence;
            _sslCertDeployment = sslCertDeployment;
        }

        public void Directory(string sourceDir, string destDir)
        {
            var copyDirProvider = new CopyDirProvider(sourceDir, destDir);
            _sequence.Add(copyDirProvider);
        }

        public void File(string sourceFile, string destFile)
        {
            throw new System.NotImplementedException();
        }

        public void IisWebApplication()
        {
            throw new System.NotImplementedException();
        }

        public void WindowsService()
        {
            throw new System.NotImplementedException();
        }

        public void NServiceBusEndpoint(string sourceDir, string destDir, string serviceName)
        {
            throw new NotImplementedException();
        }

        public void NServiceBusEndpoint(string sourceDir, string destDir, string serviceName, Action<NServiceBusOptions> nServiceBusOptions)
        {
            throw new NotImplementedException();
        }

        public IDeploySslCertificate SslCertificate { get { return _sslCertDeployment; } }
    }
}