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
	Write-Host "Executing from: $pwd"
	Exec { & "$nuget" pack $build_directory\$package_name.nuspec -verbose }
}
