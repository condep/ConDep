properties {
	$pwd = Split-Path $psake.build_script_file	
	$build_directory  = "$pwd\Build"
	$tools_directory  = "$pwd\tools\"
	$nuget_directory  = "$pwd\.nuget"
	$nuget  = "$nuget_directory\nuget.exe"
	$package_name = ""
}

task default -depends BuildNugetPackage

task BuildNugetPackage { 
	$nuspecFiles = get-childitem $build_directory -name -include *.nuspec
	
	$nuspecFiles | foreach {
		Write-Host "Creating nuget package with $_"
		Exec { & "$nuget" pack $build_directory\$_ -verbose }
	}
}
