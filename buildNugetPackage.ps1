properties {
	$nuget  = ".nuget\nuget.exe"
}

task default -depends BuildNugetPackage

task BuildNugetPackage { 
	$nuspecFiles = get-childitem -name -include *.nuspec
	
	$nuspecFiles | foreach {
		Write-Host "Creating nuget package with $_"
		Exec { & "$nuget" pack $_ -verbose }
	}
}
