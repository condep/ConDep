properties {
	$pwd = Split-Path $psake.build_script_file	
	$build_directory  = "$pwd\Build"
	$tools_directory  = "$pwd\tools\"
	$nuget_directory  = "$pwd\.nuget"
	$nuget  = "$nuget_directory\nuget.exe"
}

task default -depends BuildNugetPackage

task BuildNugetPackage { 
	Exec { & "$nuget" pack $pwd\ConDep.Dsl.nuspec -verbose }
}
