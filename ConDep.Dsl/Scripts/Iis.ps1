Import-Module WebAdministration; 

function New-ConDepIisWebSite {
	[CmdletBinding()]
	param (
		[Parameter(Mandatory=$true)] [string] $Name, 
		[Parameter(Mandatory=$true)] [int] $Id,
		[hashtable[]] $Bindings = @(),
		[string] $Path = "", 
		[string] $AppPool = "")
		
	$defaultPath = "$env:SystemDrive\inetpub\$Name"
	$physicalPath = if($Path) { $Path } else { $defaultPath }

	if(!$Bindings) {
		$Bindings = @(@{protocol='http';bindingInformation=':80:'})
	}
	RemoveWebSite $Id
	CreateWebSiteDir $physicalPath

	$webSite = new-item -force IIS:\Sites\$Name -Id $Id -Bindings $bindings -PhysicalPath $physicalPath -ApplicationPool $AppPool
	
	$Bindings | Where-Object {$_.protocol -eq "https"} | AssociateCertificateWithBinding
	StartWebSite $webSite
}

function New-ConDepAppPool {
	param (
		[Parameter(Mandatory=$true)] [string] $AppPool, 
		[hashtable] $AppPoolOptions
	)
	try {
		Remove-WebAppPool $AppPool 
	}
	catch {}

	$newAppPool = New-WebAppPool $AppPool -Force
	write-host "$($newAppPool.GetType())"
	#$newAppPool
	if($AppPoolOptions) {
		if($AppPoolOptions.Enable32Bit) { $newAppPool.enable32BitAppOnWin64 = $AppPoolOptions.Enable32Bit }
		if($AppPoolOptions.IdentityUsername) { 
			$newAppPool.processModel.identityType = 'SpecificUser'
			$newAppPool.processModel.username = $AppPoolOptions.IdentityUsername
			$newAppPool.processModel.password = $AppPoolOptions.IdentityPassword
		}
		
		if($AppPoolOptions.IdleTimeoutInMinutes) { $newAppPool.processModel.idleTimeout = [TimeSpan]::FromMinutes($AppPoolOptions.IdleTimeoutInMinutes) }
		if($AppPoolOptions.LoadUserProfile) { $newAppPool.processModel.loadUserProfile =  $AppPoolOptions.LoadUserProfile }
		if($AppPoolOptions.ManagedPipeline) { $newAppPool.managedPipelineMode = $AppPoolOptions.ManagedPipeline }
		if($AppPoolOptions.NetFrameworkVersion) { $newAppPool.managedRuntimeVersion = $AppPoolOptions.NetFrameworkVersion }
		if($AppPoolOptions.RecycleTimeInMinutes -ne $null) { if($AppPoolOptions.RecycleTimeInMinutes -eq 0) { $newAppPool.recycling.periodicrestart.time = [TimeSpan]::Zero } else { $newAppPool.recycling.periodicrestart.time = [TimeSpan]::FromMinutes($AppPoolOptions.RecycleTimeInMinutes)} }
		
		$newAppPool | set-item
	}
	$newAppPool
}

function New-ConDepWebApp {
	param (
		[Parameter(Mandatory=$true)] [string] $Name,
		[Parameter(Mandatory=$true)] [string] $WebSite,
		[string] $PhysicalPath,
		[string] $AppPool
	)
	
	$existingWebSite = Get-WebSite | where-object { $_.Name -eq $WebSite }
	
	if(!$existingWebSite) {
		throw "Web Site with name [$WebSite] not found!"
	}
	
	if(!$PhysicalPath) {
		$webSitePath = [System.Environment]::ExpandEnvironmentVariables($existingWebSite.physicalPath)
		$PhysicalPath = "$webSitePath\$Name"
		if(!(Test-Path -path ($PhysicalPath))) { 
			New-Item ($PhysicalPath) -type Directory 
		}
	}
	
	if(!$AppPool) {
		$AppPool = $existingWebSite.applicationPool
	}

	if(!(Test-Path -path $PhysicalPath)) { New-Item $PhysicalPath -type Directory }; 
	
	New-WebApplication -Name $Name -Site $WebSite -PhysicalPath $PhysicalPath -ApplicationPool $AppPool -force; 
}

function New-ConDepIisHttpBinding {
	param (
		[Parameter(Mandatory=$true)] [string] $WebSiteName, 
		[string] $Port = "", 
		[string] $Ip = "", 
		[string] $HostHeader = ""
	)

#	try {
		$cmd = GetNewWebBindingCommand $WebSiteName $Port $Ip $HostHeader $false
		Invoke-Expression $cmd

		#StartWebSite $webSite
#	}
#	catch {
#		Write-Error "ConDep Error: " $Error.Count
#	}
}

function New-ConDepIisHttpsBinding {
	param (
		[Parameter(Mandatory=$true)] [string] $WebSiteName, 
		[Parameter(Mandatory=$true)] [string] $SslCertFindType, 
		[Parameter(Mandatory=$true)] [string] $SslCertFindValue, 
		[string] $Port = "", 
		[string] $Ip = "", 
		[string] $HostHeader = "")

	$cmd = GetNewWebBindingCommand $WebSiteName $Port $Ip $HostHeader $true
	Invoke-Expression $cmd
	
	AssociateCertificateWithBinding $Port $SslCertFindType $SslCertFindValue $Ip
	#StartWebSite $webSite
}

function AssociateCertificateWithBinding {
	 [CmdletBinding()]
	 param(
		[Parameter(Mandatory=$True,ValueFromPipeline=$True,ValueFromPipelinebyPropertyName=$True)] 
		[hashtable[]] $Bindings
	)
	PROCESS {
		foreach($Binding in $Bindings) {
			$bindingDetails = $Binding.bindingInformation.Split(":")
			$bindingIp = $bindingDetails[0]
			$Port = $bindingDetails[1]
			
			if(!$bindingIp) {
				Write-Host "No binding ip, setting ip to [0.0.0.0]. Port is [$port]."
				$bindingIp = "0.0.0.0"
			}

			$certsInStore = Get-ChildItem cert:\\LocalMachine\\MY
			$certFinder = new-object System.Security.Cryptography.X509Certificates.X509Certificate2Collection(,$certsInStore)
			$findResult = $certFinder.Find($Binding.FindType, $Binding.FindValue, $false)

			if(!$findResult) { 
				throw "No Certificate found when looking for [$findType] with value [$findValue] found."
			}

			if($findResult.Count -gt 1) {
				throw "Certificates with $findValue returned more than 1 result."
			}

			$webSiteCert = $findResult | Select-Object -First 1
			Remove-Item -Path "IIS:\\SslBindings\$bindingIp!$port" -ErrorAction SilentlyContinue
		    New-Item -Path "IIS:\\SslBindings\$bindingIp!$port" -Value $webSiteCert
		}
	}
}

function GetNewWebBindingCommand ([string] $WebSiteName, [string] $Port = "", [string] $Ip = "", [string] $HostHeader = "", [bool] $Ssl) {
	if(!$Port -and !$Ip -and !$HostHeader) {
		throw "Either port, ip or host header must be specified to create a new binding in IIS."
	}

	if(!$Port) {
		$Port = "80"
	}

	$newWebBindingCmd = "New-WebBinding -force -Name $webSiteName -Port $port "
	
	if($Ssl) {
		$newWebBindingCmd += "-Protocol https "
	}
	
	if($Ip) {
		$newWebBindingCmd += "-IPAddress $ip "
	}
			
	if($hostHeader) {
		$newWebBindingCmd += "-HostHeader $HostHeader "
	}
		
	Write-Host "Command to execute: $newWebBindingCmd"
	return $newWebBindingCmd
}

function RemoveWebSite($webSiteId) {
	get-website | where-object { $_.ID -eq $webSiteId } | Remove-Website
}

function CreateWebSiteDir($dirPath) {
	if(!$dirPath) {
		return
	}
	
	if(!(Test-Path -path $dirPath)) {
		New-Item -Path $dirPath -Type Directory
	}
}

#function AssociateCertificateWithBinding([string] $port, [string] $certCommonName, [string] $bindingIp = "0.0.0.0") {
#	$matchString = "CN=*$certCommonName*"
#    $webSiteCerts = Get-ChildItem cert:\\LocalMachine\\MY | Where-Object {$_.Subject -match $matchString}
#	
#	if(!$bindingIp) {
#		Write-Host "No binding ip, setting ip to 0.0.0.0."
#		$bindingIp = "0.0.0.0"
#	}
#	
#	if($webSiteCerts.Count -gt 1) {
#		throw "Certificates with $matchString returned more than 1 result."
#	}
#
#	if(!$webSiteCerts) { 
#		throw "No Certificate with $matchString found."
#	}
#
#	$webSiteCert = $webSiteCerts | Select-Object -First 1
#
#	Remove-Item -Path "IIS:\\SslBindings\$bindingIp!$port" -ErrorAction SilentlyContinue
#   New-Item -Path "IIS:\\SslBindings\$bindingIp!$port" -Value $webSiteCert
#}

function StartWebSite($webSite) {
	if($webSite.State -eq 'Stopped') 
	{
		try {
			Write-Host "Web site is stopped. Trying to start..."
			$webSite.Start()
		}
		catch {
			throw "Unable to start web site."
		}
		Write-Host "Web site started"
	} 
}

function GetNewWebSiteCommand([string]$name, [int]$id, [string]$path = "", [string]$port = "", [string]$ip = "", [string]$hostHeader = "", [string] $appPool = "", [bool] $ssl) {
	$newWebSiteCmd = "New-Item -force IIS:\Sites\$name -id $id -bindings @{}"
	
	if($path) {
		$newWebSiteCmd += "-PhysicalPath $path "
	}
	
	if($port) {
		$newWebSiteCmd += "-Port $port "
	}
	
	if($ip) {
		$newWebSiteCmd += "-IPAddress $ip "	
	}
	
	if($hostHeader) {
		$newWebSiteCmd += "-HostHeader $hostHeader "
	}
	
	if($appPool) {
		$newWebSiteCmd += "-ApplicationPool $appPool "
	}
	
	if($ssl) {
		$newWebSiteCmd += "-Ssl "
	}
	
	return $newWebSiteCmd
}
