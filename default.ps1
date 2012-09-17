properties {
	$pwd = Split-Path $psake.build_script_file	
	$build_directory  = "$pwd\Build"
	$tools_directory  = "$pwd\tools\"
	$version          = "1.0.0.0"
	$configuration = "Debug"
	$condep = "ConDep"
	$condep_dsl = "ConDep.Dsl"
	$condep_console = "ConDep.Console"
	$condep_dsl_lb_arr = "ConDep.Dsl.LoadBalancer.Arr"
}
 
include .\tools\psake_ext.ps1

task default -depends Build-All
task ci -depends Build-All

task Build-All -depends Init, Build-ConDep-Dsl, Build-ConDep-Console, Build-ConDep-Dsl-LB-ARR

task Build-ConDep-Dsl -depends Clean-ConDep-Dsl, Init { 
	Exec { msbuild "$pwd\$condep_dsl\$condep_dsl.csproj" /t:Build /p:Configuration=$configuration /p:OutDir=$build_directory\$condep_dsl }

	Generate-Nuspec-File `
		-file "$build_directory\$condep_dsl.nuspec" `
		-version $nugetVersion `
		-pre_release $true `
		-id "$condep_dsl" `
		-title "$condep_dsl" `
		-licenseUrl "https://github.com/torresdal/ConDep/blob/master/LICENSE" `
		-projectUrl "https://github.com/torresdal/ConDep/" `
		-description "Note: This package is for extending the ConDep.Dsl. If you're looking for ConDep to do deployment or infrastructure as code, please use the ConDep package. <br/><br/>ConDep is a highly extendable Domain Specific Language for Continuous Deployment, Continuous Delivery and Infrastructure as Code on Windows." `
		-iconUrl "https://raw.github.com/torresdal/ConDep/master/images/ConDepNugetLogo.png" `
		-releaseNotes "Initial pre-release." `
		-tags "Continuous Deployment Delivery Infrastructure WebDeploy Deploy" `
		-files @(@{ Path="$build_directory\$condep_dsl\$condep_dsl.dll"; Target="lib/net40"} ) `
		-frameworkAssemblies @(
			@{ Name="Microsoft.Web.Deployment"; Target="net40"}, 
			@{ Name="Microsoft.Web.Delegation"; Target="net40"} 
		)
}

task Build-ConDep-Console -depends Clean-ConDep-Console, Init { 
	Exec { msbuild "$pwd\$condep_console\$condep_console.csproj" /t:Build /p:Configuration=$configuration /p:OutDir=$build_directory\$condep_console }

	Generate-Nuspec-File `
		-file "$build_directory\$condep.nuspec" `
		-version $nugetVersion `
		-pre_release $true `
		-id "$condep" `
		-title "$condep" `
		-licenseUrl "https://github.com/torresdal/ConDep/blob/master/LICENSE" `
		-projectUrl "https://github.com/torresdal/ConDep/" `
		-description "ConDep is a highly extendable Domain Specific Language for Continuous Deployment, Continuous Delivery and Infrastructure as Code on Windows." `
		-iconUrl "https://raw.github.com/torresdal/ConDep/master/images/ConDepNugetLogo.png" `
		-releaseNotes "Initial pre-release." `
		-tags "Continuous Deployment Delivery Infrastructure WebDeploy Deploy" `
		-dependencies @(
			@{ Name="$condep_dsl"; Version="$nugetVersion"},
			@{ Name="Json.NET"; Version="4.5.9"},
			@{ Name="NDesk.Options"; Version="0.2.1"}
		) `
		-files @(
			@{ Path="$build_directory\$condep_console\$condep.exe"; Target="lib/net40"}, 
			@{ Path="$build_directory\$condep_console\ConDep.Dsl.Operations.TransformConfig.dll"; Target="lib/net40"} 
		) `
		-frameworkAssemblies @(
			@{ Name="Microsoft.Web.Publishing.Tasks"; Target="net40"} 
		)
}

task Build-ConDep-Dsl-LB-ARR -depends Clean-ConDep-Dsl-LB-ARR, Init { 
	Exec { msbuild "$pwd\$condep_dsl_lb_arr\$condep_dsl_lb_arr.csproj" /t:Build /p:Configuration=$configuration /p:OutDir=$build_directory\$condep_dsl_lb_arr }

	Generate-Nuspec-File `
		-file "$build_directory\$condep_dsl_lb_arr.nuspec" `
		-version $nugetVersion `
		-pre_release $true `
		-id "$condep_dsl_lb_arr" `
		-title "$condep_dsl_lb_arr" `
		-licenseUrl "https://github.com/torresdal/ConDep/blob/master/LICENSE" `
		-projectUrl "https://github.com/torresdal/ConDep/" `
		-description "ConDep is a highly extendable Domain Specific Language for Continuous Deployment, Continuous Delivery and Infrastructure as Code on Windows." `
		-iconUrl "https://raw.github.com/torresdal/ConDep/master/images/ConDepNugetLogo.png" `
		-releaseNotes "Initial pre-release." `
		-tags "Continuous Deployment Delivery Infrastructure WebDeploy Deploy" `
		-dependencies @(
			@{ Name="$condep_dsl"; Version="$nugetVersion"},
			@{ Name="ArrLoadBalancerCmdlet"; Version="1.0.2"}
		) `
		-files @(@{ Path="$build_directory\$condep_dsl_lb_arr\$condep_dsl_lb_arr.dll"; Target="lib/net40"} )       
}

task Init {  
	#Generate-Nuspec-File -file "$build_directory\$condep_dsl.nuspec" -version $nugetVersion -pre_release $true
	
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
