using System;
using System.Collections.Generic;
using ConDep.Dsl.Operations.WebDeploy.Model;

namespace ConDep.Dsl
{
    public class CustomWebSiteProvider : CompositeProvider
    {
        private readonly string _webSiteName;
        private readonly int _id;
        private readonly IList<IisBinding> _bindings = new List<IisBinding>();

        public CustomWebSiteProvider(string webSiteName, int id)
        {
            _webSiteName = webSiteName;
            _id = id;
        }

        public IList<IisBinding> Bindings { get { return _bindings; } }

        public string PhysicalPath { get; set; }

        public override bool IsValid(Notification notification)
        {
            return !string.IsNullOrWhiteSpace(_webSiteName);
        }

        public override void Configure()
        {
            string psCommand = GetRemoveExistingWebSiteCommand(_id);
            psCommand += GetCreateWebSiteDirCommand(PhysicalPath);
            psCommand += GetCreateWebSiteCommand(_webSiteName);
            psCommand += GetCreateBindings();
            psCommand += GetCertificateCommand();
            Configure(p => p.PowerShell("Import-Module WebAdministration; " + psCommand, o => o.WaitIntervalInSeconds(10)));
        }

        private string GetCreateWebSiteDirCommand(string webSiteDir)
        {
            return string.IsNullOrWhiteSpace(webSiteDir) ? "" : string.Format("if((Test-Path -path {0}) -ne $True) {{ New-Item {0} -type Directory }}; ", webSiteDir);
        }

        private string GetRemoveExistingWebSiteCommand(int webSiteId)
        {
            return string.Format("get-website | where-object {{ $_.ID -match '{0}' }} | Remove-Website; ", webSiteId);
        }

        private string GetCertificateCommand()
        {
            string command = "";
            foreach(var binding in Bindings)
            {
                if(binding.BindingType == BindingType.https)
                {
                    var bindingIp = string.IsNullOrWhiteSpace(binding.Ip) ? "0.0.0.0" : binding.Ip;
                    command += string.Format("Set-Location IIS:\\SslBindings; Remove-Item {0}!{1} -ErrorAction SilentlyContinue; ", bindingIp, binding.Port);
                    command += string.Format("Get-ChildItem cert:\\LocalMachine\\MY | Where-Object {{$_.Subject -match 'CN=*{0}*'}} | Select-Object -First 1 | New-Item {1}!{2}; ", binding.CertificateCommonName,  bindingIp, binding.Port);
                }
            }
            return command;
        }

        private string GetCreateBindings()
        {
            string bindingCommands = "";
            for (int index = 1; index < Bindings.Count; index++)
            {
                var binding = Bindings[index];
                var currentBinding = binding;
                var ipAddress = string.IsNullOrWhiteSpace(currentBinding.Ip)
                                    ? ""
                                    : "-IPAddress \"" + currentBinding.Ip + "\"";

                var hostHeader = string.IsNullOrWhiteSpace(currentBinding.HostHeader)
                                     ? ""
                                     : "-HostHeader \"" + currentBinding.HostHeader + "\"";

                bindingCommands +=
                    string.Format("New-WebBinding -Name \"{0}\" -Protocol \"{1}\" -Port {2} {3} {4} -force; ",
                                  _webSiteName, currentBinding.BindingType, currentBinding.Port, ipAddress, hostHeader);
            }

            return bindingCommands;
        }

        private string GetCreateWebSiteCommand(string webSiteName)
        {
            string bindingString = "";
            if(Bindings.Count > 0)
            {
                var binding = Bindings[0];
                bindingString += "-Port " + binding.Port + " ";
                bindingString += binding.BindingType == BindingType.https ? "-Ssl " : "";
                bindingString += !string.IsNullOrWhiteSpace(binding.Ip) ? "-IPAddress \"" + binding.Ip +"\" " : "";
                bindingString += !string.IsNullOrWhiteSpace(binding.HostHeader) ? "-HostHeader \"" + binding.HostHeader + "\" " : "";
            } 

            var physicalPath = string.IsNullOrWhiteSpace(PhysicalPath) ? "" : string.Format("-PhysicalPath \"{0}\" ", PhysicalPath);
            return string.Format("New-Website -Name \"{0}\" -Id {1} {2}{3}-force; ", webSiteName, _id, physicalPath, bindingString);
        }
    }
}