function Stop-NSBService($serviceName) {
    $service = Get-Service $serviceName -ErrorAction Stop

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

function Remove-NSBService($serviceName) {
    Stop-NSBService $serviceName
    C:\\WINDOWS\\system32\\sc.exe delete $serviceName
}

function Start-NSBService($serviceName) {
    $service = Get-Service $serviceName -ErrorAction Stop 
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