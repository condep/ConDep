using System.Collections.Generic;
using System.Linq;
using ConDep.Dsl;
using Microsoft.Web.Deployment;
using DeploymentProviderOptions = Microsoft.Web.Deployment.DeploymentProviderOptions;

namespace ConDep.Dsl
{
    public class WebSiteDeploymentProvider : WebDeployProviderBase
    {
        private readonly string _destFilePath;
        private readonly Dictionary<string, bool> _linkExtensions = new Dictionary<string, bool>();

        public WebSiteDeploymentProvider(string sourceWebsiteName, string destWebSiteName)
        {
            DestinationPath = destWebSiteName;
            SourcePath = sourceWebsiteName;
        }

        public WebSiteDeploymentProvider(string sourceWebsiteName, string destWebSiteName, string destFilePath)
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
                                from excludedLink in _linkExtensions
                                where excludedLink.Value && link.Name == excludedLink.Key
                                select link).ToList();

            excludedLinks.ForEach(x => x.Enabled = false);

            var includedLinks = (from link in sourceBaseOptions.LinkExtensions
                                 from excludedLink in _linkExtensions
                                 where !excludedLink.Value && link.Name == excludedLink.Key
                                 select link).ToList();

            includedLinks.ForEach(x => x.Enabled = true);

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
            return _linkExtensions.ContainsKey(extensionExcludeName) ? _linkExtensions[extensionExcludeName] : false;
        }

        private void SetExcludeValue(string extensionExcludeName, bool value)
        {
            if (_linkExtensions.ContainsKey(extensionExcludeName))
            {
                _linkExtensions[extensionExcludeName] = value;
            }
            else
            {
                _linkExtensions.Add(extensionExcludeName, value);
            }
        }

    }
}