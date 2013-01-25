function Stop-NSBService($serviceName, $ignoreFailure) {
    $service = Get-Service $serviceName -ErrorAction Stop

	try {
		if ($service.Status -eq [System.ServiceProcess.ServiceControllerStatus]::Running) 
		{ 
			Write-Host "Stopping: $($service.DisplayName)"
			$service.Stop()
			$service.WaitForStatus([System.ServiceProcess.ServiceControllerStatus]::Stopped)
			Write-Host "Stopped: $($service.DisplayName)" 
		}
		else {
			Write-Host "$($service.DisplayName) is already stopped" 
		}	
	}
	catch {
		if($ignoreFailure) {
			return 0
		}
		else {
			throw
		}
	}
}

function Remove-NSBService($serviceName) {
	$result = 0
	$managedService = Get-Service $serviceName -ErrorAction Stop
	
	if ($managedService.Status -eq [System.ServiceProcess.ServiceControllerStatus]::Running) 
	{ 
		Write-Host "Stopping: $($managedService.DisplayName)"
		$managedService.Stop()
		$managedService.WaitForStatus([System.ServiceProcess.ServiceControllerStatus]::Stopped)
		Write-Host "Stopped: $($managedService.DisplayName)"
	} 

	#$service = Get-Service $serviceName -ErrorAction Stop
    #Stop-NSBService $serviceName $true
	
	$service = Get-WmiObject -Class Win32_Service -Filter "Name='$serviceName'"
	Write-Host "Removing: $($service.DisplayName)"
	$result = $service.Delete().ReturnValue
	if($result -ne 0) {
		throw 'Unable to delete service [$serviceName]. Return code [$result].'
	}

	$managedService.Refresh()
	
	if($managedService.Status) {
		while ($managedService.Status) {
			Write-Host "Waiting for service to be deleted..."
			Start-Sleep -s 2
			$managedService.Refresh()
		}
	}

	Write-Host "Removed: $($service.DisplayName)"
	
    #C:\\WINDOWS\\system32\\sc.exe delete $serviceName
}

function Start-NSBService($serviceName, $ignoreFailure) {
    $service = Get-Service $serviceName -ErrorAction Stop
	try { 
		if ($service.Status -eq [System.ServiceProcess.ServiceControllerStatus]::Stopped) 
		{ 
			Write-Host "Starting: $($service.DisplayName)"
			$service.Start()
			$service.WaitForStatus([System.ServiceProcess.ServiceControllerStatus]::Running)
			Write-Host "Started: $($service.DisplayName)"
		} 
		else 
		{ 
			Write-Host "$($service.DisplayName) is already running" 
		} 
	}
	catch {
		if($ignoreFailure) {
			return 0
		}
		else {
			throw
		}
	}
}