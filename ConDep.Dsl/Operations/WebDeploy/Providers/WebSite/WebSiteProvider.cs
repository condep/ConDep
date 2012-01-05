using System;
using ConDep.Dsl.Operations.WebDeploy.Model;
using Microsoft.Web.Deployment;

namespace ConDep.Dsl
{
    public class WebSiteProvider : Provider
    {
        //private readonly string _sourceWebsiteName;
        //private readonly string _destWebSiteName;

        public WebSiteProvider(string sourceWebsiteName, string destWebSiteName)
        {
            DestinationPath = destWebSiteName;
            SourcePath = sourceWebsiteName;
            //_sourceWebsiteName = sourceWebsiteName;
            //_destWebSiteName = destWebSiteName;
        }

        public override string Name
        {
            get { return DeploymentWellKnownProvider.AppHostConfig.ToString(); }
        }

        public bool ExcludeAppPools { get; set; }

        public bool ExcludeCertificates { get; set; }

        public bool ExcludeContent { get; set; }

        public bool ExcludeFrameworkConfig { get; set; }

        public bool ExcludeHttpCertConfig { get; set; }

        public override DeploymentProviderOptions GetWebDeployDestinationObject()
        {
            return new DeploymentProviderOptions(Name) { Path = DestinationPath };
        }

        public override DeploymentObject GetWebDeploySourceObject(DeploymentBaseOptions sourceBaseOptions)
        {
            return DeploymentManager.CreateObject(Name, SourcePath, sourceBaseOptions);
        }

        public override bool IsValid(Notification notification)
        {
            return !string.IsNullOrWhiteSpace(SourcePath) && !string.IsNullOrWhiteSpace(DestinationPath);
        }
    }
}