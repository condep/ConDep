using System;
using ConDep.Dsl.Operations.WebDeploy.Model;

namespace ConDep.Dsl
{
    public class CustomWebSiteProvider : CompositeProvider
    {
        private readonly string _webSiteName;

        public CustomWebSiteProvider(string webSiteName)
        {
            _webSiteName = webSiteName;
        }

        public override bool IsValid(Notification notification)
        {
            return !string.IsNullOrWhiteSpace(_webSiteName);
        }

        public override void Configure()
        {
            Configure(p => p.PowerShell(string.Format("Import-Module WebAdministration; New-Website -Name \"{0}\" -force", _webSiteName)));
        }
    }
}