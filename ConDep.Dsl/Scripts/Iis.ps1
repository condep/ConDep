Import-Module WebAdministration; 

function New-ConDepIisWebSite {
	param (
		[Parameter(Mandatory=$true)] [string] $Name, 
		[Parameter(Mandatory=$true)] [int] $Id, 
		[string] $Path = "", 
		[string] $AppPool = "")
		
	$webSiteCmd = GetNewWebSiteCommand $Name $Id $Path $AppPool $Ssl

	RemoveWebSite $Id
	CreateWebSiteDir $Path
	
	$webSite = Invoke-Expression $webSiteCmd

	#StartWebSite $webSite
	Write-Host
}

function New-ConDepIisHttpBinding {
	param (
		[Parameter(Mandatory=$true)] [string] $WebSiteName, 
		[string] $Port = "", 
		[string] $Ip = "", 
		[string] $HostHeader = "")

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

function AssociateCertificateWithBinding([string] $port, [System.Security.Cryptography.X509Certificates.X509FindType] $findType, $findValue, [string] $bindingIp = "0.0.0.0") {
	#$matchString = "CN=*$certCommonName*"
    #$webSiteCerts = Get-ChildItem cert:\\LocalMachine\\MY | Where-Object {$_.Subject -match $matchString}
	
	if(!$bindingIp) {
		Write-Host "No binding ip, setting ip to [0.0.0.0]. Port is [$port]."
		$bindingIp = "0.0.0.0"
	}

	$certs = Get-ChildItem cert:\\LocalMachine\\MY
	$certsArray = @(0)
	$certsArray[0] = $certs
	$certFinder = new-object System.Security.Cryptography.X509Certificates.X509Certificate2Collection($certsArray)
	$findResult = $certFinder.Find($findType, $findValue, $false)

	if(!$findResult) { 
		throw "No Certificate found when looking for [$findType] with value [$findValue] found."
	}

	if($findResult.Count -gt 1) {
		throw "Certificates with $findValue returned more than 1 result."
	}

	$webSiteCert = $findResult | Select-Object -First 1
	Write-Host "Certificate found:"
	$webSiteCert
	Remove-Item -Path "IIS:\\SslBindings\$bindingIp!$port" -ErrorAction SilentlyContinue
	
	Write-Host "Command to execute: New-Item -Path IIS:\\SslBindings\$bindingIp!$port -Value $webSiteCert"
    New-Item -Path "IIS:\\SslBindings\$bindingIp!$port" -Value $webSiteCert
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
	get-website | where-object { $_.ID -match $webSiteId } | Remove-Website
}

function CreateWebSiteDir($dirPath) {
	if(!$dirPath) {
		return
	}
	
	if((Test-Path -path $dirPath) -ne $True) {
		New-Item $dirPath -type Directory 
	} 
	else {
		Write-Host "Web site directory $dirPath already exist."
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
	$newWebSiteCmd = "New-Website -force -Name $name -Id $id "
	
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
