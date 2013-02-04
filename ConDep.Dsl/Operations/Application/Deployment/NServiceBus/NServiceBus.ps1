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
	$managedService = Get-Service $serviceName -ErrorAction SilentlyContinue

	if(!$managedService) {
		return
	}

	$processId = (get-wmiobject Win32_Service -filter "name='$serviceName'").ProcessID

	if ($managedService.Status -eq [System.ServiceProcess.ServiceControllerStatus]::Running) 
	{ 
		Write-Host "Stopping Windows Service [$($managedService.DisplayName)]."
		$managedService.Stop()
		$managedService.WaitForStatus([System.ServiceProcess.ServiceControllerStatus]::Stopped)
		Write-Host "Current service status for [$($managedService.DisplayName)] is [$($managedService.Status)]."
	} 
	
	$service = Get-WmiObject -Class Win32_Service -Filter "Name='$serviceName'"
	Write-Host "Removing Windows Service [$($service.DisplayName)]."
	$result = $service.Delete().ReturnValue
	if($result -ne 0) {
		throw 'Unable to delete service [$serviceName]. Return code [$result].'
	}
	Write-Host "Windows Service [$($service.DisplayName)] removed."

	if($processId) {
		$process = Get-Process -Id $processId -ErrorAction SilentlyContinue
		
		if($process -and !$process.HasExited) {
			Write-Host "Waiting for process with id [$processId] to exit..."
			$process.WaitForExit()
			Write-Host "Process id [$processId] has now exited."
		}
	}
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