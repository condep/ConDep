properties {
	$pwd = Split-Path $psake.build_script_file	
	$build_directory  = "$pwd\Build"
	$tools_directory  = "$pwd\tools"
	$version          = "1.0.0.0"
	$configuration = "Debug"
	$condep = "ConDep"
	$condep_dsl = "ConDep.Dsl"
	$condep_console = "ConDep.Console"
	$condep_remote = "ConDep.Remote"
	$condep_dsl_lb_arr = "ConDep.Dsl.LoadBalancer.Arr"
	$condep_tests = "ConDep.Dsl.Tests"
	$lib = "$pwd\lib"
	$preString = "-rc"
}
 
include .\tools\psake_ext.ps1

task default -depends Build-All
task ci -depends Build-All

task Build-All -depends Init, Build-ConDep-Dsl, Build-ConDep-Console, Build-ConDep-Dsl-LB-ARR, Build-Tests

task Build-ConDep-Dsl -depends Clean-ConDep-Dsl, Init { 
	Exec { msbuild "$pwd\$condep_dsl\$condep_dsl.csproj" /t:Build /p:Configuration=$configuration /p:OutDir=$build_directory\$condep_dsl\ }

	Generate-Nuspec-File `
		-file "$build_directory\$condep_dsl.nuspec" `
		-version $nugetVersion `
		-preString $preString `
		-id "$condep_dsl" `
		-title "$condep_dsl" `
		-licenseUrl "http://www.con-dep.net/license/" `
		-projectUrl "http://www.con-dep.net/" `
		-description "Note: This package is for extending the ConDep DSL. If you're looking for ConDep to do deployment or infrastructure as code, please use the ConDep package. ConDep is a highly extendable Domain Specific Language for Continuous Deployment, Continuous Delivery and Infrastructure as Code on Windows." `
		-iconUrl "https://raw.github.com/torresdal/ConDep/master/images/ConDepNugetLogo.png" `
		-releaseNotes "Initial pre-release." `
		-tags "Continuous Deployment Delivery Infrastructure WebDeploy Deploy msdeploy IIS automation powershell remote" `
		-dependencies @(
			@{ Name="log4net"; Version="2.0.0"}
		) `
		-files @(@{ Path="$condep_dsl\$condep_dsl.dll"; Target="lib/net40"} ) `
		-frameworkAssemblies @(
			@{ Name="Microsoft.Web.Deployment"; Target="net40"}, 
			@{ Name="Microsoft.Web.Delegation"; Target="net40"} 
		)
}

task Build-ConDep-Console -depends Clean-ConDep-Console, Init { 
	Exec { msbuild "$pwd\$condep_console\$condep_console.csproj" /t:Build /p:Configuration=$configuration /p:OutDir=$build_directory\$condep_console\ }

	Generate-Nuspec-File `
		-file "$build_directory\$condep.nuspec" `
		-version $nugetVersion `
		-preString $preString `
		-id "$condep" `
		-title "$condep" `
		-licenseUrl "http://www.con-dep.net/license/" `
		-projectUrl "http://www.con-dep.net/" `
		-description "ConDep is a highly extendable Domain Specific Language for Continuous Deployment, Continuous Delivery and Infrastructure as Code on Windows." `
		-iconUrl "https://raw.github.com/torresdal/ConDep/master/images/ConDepNugetLogo.png" `
		-releaseNotes "Initial pre-release." `
		-tags "Continuous Deployment Delivery Infrastructure WebDeploy Deploy msdeploy IIS automation powershell remote" `
		-dependencies @(
			@{ Name="$condep_dsl"; Version="$nugetVersion$preString"},
			@{ Name="NDesk.Options"; Version="0.2.1"}
			@{ Name="log4net"; Version="2.0.0"}
		) `
		-files @(
			@{ Path="$condep_console\$condep.exe"; Target="lib/net40"},
			@{ Path="$condep_console\$condep_remote.dll"; Target="lib/net40"},
			@{ Path="$lib\SlowCheetah\v2.4\SlowCheetah.Tasks.dll"; Target="lib/net40"}
		)
}

task Build-ConDep-Dsl-LB-ARR -depends Clean-ConDep-Dsl-LB-ARR, Init { 
	Exec { msbuild "$pwd\$condep_dsl_lb_arr\$condep_dsl_lb_arr.csproj" /t:Build /p:Configuration=$configuration /p:OutDir=$build_directory\$condep_dsl_lb_arr\ }

	Generate-Nuspec-File `
		-file "$build_directory\$condep_dsl_lb_arr.nuspec" `
		-version $nugetVersion `
		-preString $preString `
		-id "$condep_dsl_lb_arr" `
		-title "$condep_dsl_lb_arr" `
		-licenseUrl "http://www.con-dep.net/license/" `
		-projectUrl "http://www.con-dep.net/" `
		-description "ConDep is a highly extendable Domain Specific Language for Continuous Deployment, Continuous Delivery and Infrastructure as Code on Windows." `
		-iconUrl "https://raw.github.com/torresdal/ConDep/master/images/ConDepNugetLogo.png" `
		-releaseNotes "Initial pre-release." `
		-tags "Continuous Deployment Delivery Infrastructure WebDeploy Deploy msdeploy IIS automation powershell remote" `
		-dependencies @(
			@{ Name="$condep_dsl"; Version="$nugetVersion$preString"},
			@{ Name="ArrLoadBalancerCmdlet"; Version="1.0.2"}
		) `
		-files @(@{ Path="$condep_dsl_lb_arr\$condep_dsl_lb_arr.dll"; Target="lib/net40"} )       
}

task Build-Tests -depends Clean-Tests, Init {
	Exec { msbuild "$pwd\$condep_tests\$condep_tests.csproj" /t:Build /p:Configuration=$configuration /p:OutDir=$build_directory\$condep_tests\ }
}

task Init {
	$script:nugetVersion = $version.Substring(0, $version.LastIndexOf("."))

	Generate-Assembly-Info `
		-file "$pwd\AssemblyVersionInfo.cs" `
		-company "ConDep" `
		-product "ConDep $version" `
		-copyright "Copyright © ConDep 2012" `
		-version $version `
		-clsCompliant "true"
        
	if ((Test-Path $build_directory) -eq $false) {
		New-Item $build_directory -ItemType Directory
	}
}

task Clean-ConDep-Dsl {
	Write-Host "Cleaning Build output"  -ForegroundColor Green
	Remove-Item $build_directory\$condep_dsl -Force -Recurse -ErrorAction SilentlyContinue
}

task Clean-ConDep-Console {
	Write-Host "Cleaning Build output"  -ForegroundColor Green
	Remove-Item $build_directory\$condep_console -Force -Recurse -ErrorAction SilentlyContinue
}

task Clean-ConDep-Dsl-LB-ARR {
	Write-Host "Cleaning Build output"  -ForegroundColor Green
	Remove-Item $build_directory\$condep_dsl_lb_arr -Force -Recurse -ErrorAction SilentlyContinue
}

task Clean-Tests {
	Write-Host "Cleaning Build output"  -ForegroundColor Green
	Remove-Item $build_directory\$condep_tests -Force -Recurse -ErrorAction SilentlyContinue
}
