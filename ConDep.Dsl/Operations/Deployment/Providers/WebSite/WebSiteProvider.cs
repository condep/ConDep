using System;
using System.Collections.Generic;
using System.Linq;
using ConDep.Dsl.Operations.WebDeploy.Model;
using Microsoft.Web.Deployment;

namespace ConDep.Dsl
{
    public class WebSiteProvider : ExistingServerProvider
    {
        private readonly string _destFilePath;
        private readonly Dictionary<string, bool> _disabledLinkExtensions = new Dictionary<string, bool>();

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
            DestinationPath = destWebSiteName;
            SourcePath = sourceWebsiteName;
        }

        public override string Name
        {
            get { return DeploymentWellKnownProvider.AppHostConfig.ToString(); }
        }

        public bool ExcludeAppPools
        {
            get { return GetExcludeValue("AppPoolExtension"); }
            set { SetExcludeValue("AppPoolExtension", value); }
        }

        public bool ExcludeCertificates
        {
            get { return GetExcludeValue("CertificateExtension"); }
            set { SetExcludeValue("CertificateExtension", value); }
        }

        public bool ExcludeContent
        {
            get { return GetExcludeValue("ContentExtension"); }
            set { SetExcludeValue("ContentExtension", value); }
        }

        public bool ExcludeFrameworkConfig
        {
            get { return GetExcludeValue("FrameworkConfigExtension"); }
            set { SetExcludeValue("FrameworkConfigExtension", value); }
        }

        public bool ExcludeHttpCertConfig
        {
            get { return GetExcludeValue("HttpCertConfigExtension"); }
            set { SetExcludeValue("HttpCertConfigExtension", value); }
        }

        public override DeploymentProviderOptions GetWebDeployDestinationObject()
        {
            return new DeploymentProviderOptions(Name) { Path = DestinationPath };
        }

        public override DeploymentObject GetWebDeploySourceObject(DeploymentBaseOptions sourceBaseOptions)
        {
            var excludedLinks = (from link in sourceBaseOptions.LinkExtensions
                                from excludedLink in _disabledLinkExtensions
                                where excludedLink.Value && link.Name == excludedLink.Key
                                select link).ToList();

            excludedLinks.ForEach(x => x.Enabled = false);

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

        private bool GetExcludeValue(string extensionExcludeName)
        {
            return _disabledLinkExtensions.ContainsKey(extensionExcludeName) ? _disabledLinkExtensions[extensionExcludeName] : false;
        }

        private void SetExcludeValue(string extensionExcludeName, bool value)
        {
            if (_disabledLinkExtensions.ContainsKey(extensionExcludeName))
            {
                _disabledLinkExtensions[extensionExcludeName] = value;
            }
            else
            {
                _disabledLinkExtensions.Add(extensionExcludeName, value);
            }
        }

    }
}