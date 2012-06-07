using System;
using System.Collections.Generic;
using ConDep.Dsl.Operations.WebDeploy.Model;
using ConDep.Dsl.Operations.WebDeploy.Options;

namespace ConDep.Dsl
{
    public class CustomWebSiteProvider : CompositeProvider
    {
        private readonly string _webSiteName;
        private readonly int _id;
        private readonly string _physicalDir;
        private readonly IList<IisBinding> _bindings = new List<IisBinding>();
        private readonly ApplicationPool _applicationPool = new ApplicationPool();

        public CustomWebSiteProvider(string webSiteName, int id, string physicalDir)
        {
            _webSiteName = webSiteName;
            _id = id;
            _physicalDir = physicalDir;
        }

        public string WebSiteName { get { return _webSiteName; } }

        public IList<IisBinding> Bindings { get { return _bindings; } }

        public ApplicationPool ApplicationPool
        {
            get {
                return _applicationPool;
            }
        }

        public override bool IsValid(Notification notification)
        {
            return !string.IsNullOrWhiteSpace(_webSiteName);
        }

        public override void Configure()
        {
            string psCommand = GetRemoveExistingWebSiteCommand(_id);
            psCommand += GetCreateAppPoolCommand();
            psCommand += GetCreateWebSiteDirCommand(_physicalDir);
            psCommand += GetCreateWebSiteCommand(_webSiteName);
            psCommand += GetCreateBindings();
            psCommand += GetCertificateCommand();
            Configure(p => p.PowerShell("Import-Module WebAdministration; " + psCommand, o => o.WaitIntervalInSeconds(2).RetryAttempts(20)));
        }

        private string GetCreateAppPoolCommand()
        {
            if (!string.IsNullOrWhiteSpace(_applicationPool.Name))
            {
                var psCommand = _applicationPool.Name != null ? string.Format("Set-Location IIS:\\AppPools; try {{ Remove-WebAppPool '{0}' }} catch {{ }}; $newAppPool = New-WebAppPool '{0}'; ", _applicationPool.Name) : "";

                psCommand += _applicationPool.Enable32Bit != null ? string.Format("$newAppPool.enable32BitAppOnWin64 = {0}; ", _applicationPool.Enable32Bit.Value ? "$true" : "$false") : "";
                psCommand += _applicationPool.IdentityUsername != null ? string.Format("$newAppPool.processModel.identityType = 'SpecificUser'; $newAppPool.processModel.username = '{0}'; $newAppPool.processModel.password = '{1}'; ", _applicationPool.IdentityUsername, _applicationPool.IdentityPassword) : "";
                psCommand += _applicationPool.IdleTimeoutInMinutes != null ? string.Format("$newAppPool.processModel.idleTimeout = [TimeSpan]::FromMinutes({0}); ", _applicationPool.IdleTimeoutInMinutes) : "";
                psCommand += _applicationPool.LoadUserProfile != null ? string.Format("$newAppPool.processModel.loadUserProfile = {0}; ", _applicationPool.LoadUserProfile.Value ? "$true" : "$false") : "";
                psCommand += _applicationPool.ManagedPipeline != null ? string.Format("$newAppPool.managedPipelineMode = '{0}'; ", _applicationPool.ManagedPipeline) : "";
                psCommand += _applicationPool.NetFrameworkVersion != null ? string.Format("$newAppPool.managedRuntimeVersion = '{0}'; ", ExtractNetFrameworkVersion()) : "";
                psCommand += _applicationPool.RecycleTimeInMinutes != null ? string.Format("$newAppPool.recycling.periodicrestart.time = [TimeSpan]::FromMinutes({0}); ", _applicationPool.RecycleTimeInMinutes) : "";

                psCommand += "$newAppPool | set-item;";
                return psCommand;
            }
            return "";
        }

        private string ExtractNetFrameworkVersion()
        {
            switch (_applicationPool.NetFrameworkVersion)
            {
                case NetFrameworkVersion.Net1_0:
                    return "v1.0";
                case NetFrameworkVersion.Net1_1:
                    return "v1.1";
                case NetFrameworkVersion.Net2_0:
                    return "v2.0";
                case NetFrameworkVersion.Net4_0:
                    return "v4.0";
                case NetFrameworkVersion.Net5_0:
                    return "v5.0";
                default:
                    throw new Exception("Framework version unknown to ConDep.");
            } 

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
                    command += string.Format("$webSiteCert = Get-ChildItem cert:\\LocalMachine\\MY | Where-Object {{$_.Subject -match 'CN=*{0}*'}} | Select-Object -First 1; ", binding.CertificateCommonName);
                    command += string.Format("if($webSiteCert -eq $null) {{ throw 'No Certificate with CN=''*{0}*'' found.' }}; ", binding.CertificateCommonName);
                    command += string.Format("$webSiteCert | New-Item {0}!{1}; ", bindingIp, binding.Port);
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
                bindingString += _applicationPool.Name != null ? string.Format("-ApplicationPool '{0}' ", _applicationPool.Name) : "";
            } 

            var physicalPath = string.IsNullOrWhiteSpace(_physicalDir) ? "" : string.Format("-PhysicalPath \"{0}\" ", _physicalDir);
            return string.Format("New-Website -Name \"{0}\" -Id {1} {2}{3}-force; ", webSiteName, _id, physicalPath, bindingString);
        }
    }
}