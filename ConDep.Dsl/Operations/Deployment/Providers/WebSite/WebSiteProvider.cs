using System;
using System.Collections.Generic;
using ConDep.Dsl.Operations.WebDeploy.Model;
using Microsoft.Web.Deployment;

namespace ConDep.Dsl
{
    public class WebSiteProvider : ExistingServerProvider
    {
        private readonly string _destFilePath;
        //private readonly string _sourceWebsiteName;
        //private readonly string _destWebSiteName;

        public WebSiteProvider(string sourceWebsiteName, string destWebSiteName)
        {
            DestinationPath = destWebSiteName;
            SourcePath = sourceWebsiteName;
            //_sourceWebsiteName = sourceWebsiteName;
            //_destWebSiteName = destWebSiteName;
        }

        public WebSiteProvider(string sourceWebsiteName, string destWebSiteName, string destFilePath)
        {
            _destFilePath = destFilePath;
            //-replace:objectName=virtualDirectory,targetAttributeName=physicalPath,match="C:\\WebDeployTemplateWebSites",replace="C:\Web"
            DestinationPath = destWebSiteName;
            SourcePath = sourceWebsiteName;
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

        public override IList<DeploymentRule> GetReplaceRules()
        {
            if (string.IsNullOrWhiteSpace(_destFilePath))
                return base.GetReplaceRules();

            return new List<DeploymentRule>
                       {
                           new DeploymentReplaceRule("ConDepReplaceRule", "virtualDirectory", "", "", "physicalPath", "", _destFilePath)
                       };
        }

        public override bool IsValid(Notification notification)
        {
            return !string.IsNullOrWhiteSpace(SourcePath) && !string.IsNullOrWhiteSpace(DestinationPath);
        }
    }
}