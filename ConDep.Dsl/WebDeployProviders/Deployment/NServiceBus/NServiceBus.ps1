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
    Stop-NSBService $serviceName $true
    C:\\WINDOWS\\system32\\sc.exe delete $serviceName
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