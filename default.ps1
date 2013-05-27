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
	$condep_dsl_lb_ace = "ConDep.Dsl.LoadBalancer.Ace"
	$condep_dsl_tests = "ConDep.Dsl.Tests"
	$condep_webq_tests = "ConDep.WebQ.Tests"
	$condep_web_q_server = "ConDep.WebQ.Server"
	$condep_web_q_client = "ConDep.WebQ.Client"
	$condep_node = "ConDep.Node"
	$condep_node_client = "ConDep.Node.Client"
	$lib = "$pwd\lib"
	$preString = "-rc"
	$releaseNotes = "Pre-release"
}
 
include .\tools\psake_ext.ps1

task default -depends Build-All
task ci -depends Build-All

task Build-All -depends Init, Build-ConDep-Node, Build-ConDep-Node-Client, Build-ConDep-Dsl, Build-ConDep-Console, Build-ConDep-Dsl-LB-ARR, Build-ConDep-Dsl-LB-ACE, Build-Tests

task Build-ConDep-Node -depends Clean-ConDep-Node, Init { 
	Exec { msbuild "$pwd\$condep_node\$condep_node.csproj" /t:Build /p:Configuration=$configuration /p:OutDir=$build_directory\$condep_node\ }
	cmd /c $pwd\packages\ilmerge.2.13.0307\ILMerge.exe /wildcards /internalize /allowDup /out:$build_directory\$condep_node\ConDepNode.exe $build_directory\$condep_node\ConDep.Node.exe $build_directory\$condep_node\*.dll
	
	Generate-Nuspec-File `
		-file "$build_directory\$condep_node.nuspec" `
		-version $nugetVersion `
		-preString $preString `
		-id "$condep_node" `
		-title "$condep_node" `
		-licenseUrl "http://www.con-dep.net/license/" `
		-projectUrl "http://www.con-dep.net/" `
		-description "The ConDep Node is used by ConDep to communicate over HTTP with remote nodes (servers). Mostly for deploying files. If you're looking for ConDep to do deployment or infrastructure as code, please use the ConDep package. ConDep is a highly extendable Domain Specific Language for Continuous Deployment, Continuous Delivery and Infrastructure as Code on Windows." `
		-iconUrl "https://raw.github.com/torresdal/ConDep/master/images/ConDepNugetLogo.png" `
		-releaseNotes "$releaseNotes" `
		-tags "ConDep Continuous Deployment Delivery Infrastructure WebAPI Web API WebDeploy Deploy msdeploy remote" `
		-dependencies @(
			@{ Name="Microsoft.AspNet.WebApi.SelfHost"; Version="4.0.20918.0"}
		) `
		-files @(
			@{ Path="$condep_node\ConDepNode.exe"; Target="lib/net40"}
		)
}

task Build-ConDep-Node-Client -depends Clean-ConDep-Node-Client, Init { 
	Exec { msbuild "$pwd\$condep_node_client\$condep_node_client.csproj" /t:Build /p:Configuration=$configuration /p:OutDir=$build_directory\$condep_node_client\ }
	
	Generate-Nuspec-File `
		-file "$build_directory\$condep_node_client.nuspec" `
		-version $nugetVersion `
		-preString $preString `
		-id "$condep_node_client" `
		-title "$condep_node_client" `
		-licenseUrl "http://www.con-dep.net/license/" `
		-projectUrl "http://www.con-dep.net/" `
		-description "The ConDep Node Client is used by ConDep to communicate over HTTP with remote nodes (servers). Mostly for deploying files. If you're looking for ConDep to do deployment or infrastructure as code, please use the ConDep package. ConDep is a highly extendable Domain Specific Language for Continuous Deployment, Continuous Delivery and Infrastructure as Code on Windows." `
		-iconUrl "https://raw.github.com/torresdal/ConDep/master/images/ConDepNugetLogo.png" `
		-releaseNotes "$releaseNotes" `
		-tags "ConDep Continuous Deployment Delivery Infrastructure WebAPI Web API WebDeploy Deploy msdeploy remote" `
		-dependencies @(
			@{ Name="Microsoft.AspNet.WebApi.Client"; Version="4.0.20710.0"}
		) `
		-files @(
			@{ Path="$condep_node_client\$condep_node_client.dll"; Target="lib/net40"}
		)
}

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
		-releaseNotes "$releaseNotes" `
		-tags "Continuous Deployment Delivery Infrastructure WebDeploy Deploy msdeploy IIS automation powershell remote" `
		-dependencies @(
			@{ Name="log4net"; Version="2.0.0"},
			@{ Name="$condep_node_client"; Version="$nugetVersion$preString"}
		) `
		-files @(
			@{ Path="$condep_dsl\$condep_dsl.dll"; Target="lib/net40"}, 
			@{ Path="$condep_dsl\$condep_dsl.xml"; Target="lib/net40"}
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
		-releaseNotes "$releaseNotes" `
		-tags "Continuous Deployment Delivery Infrastructure WebDeploy Deploy msdeploy IIS automation powershell remote" `
		-dependencies @(
			@{ Name="$condep_dsl"; Version="$nugetVersion$preString"},
			@{ Name="NDesk.Options"; Version="0.2.1"},
			@{ Name="log4net"; Version="2.0.0"}
		) `
		-files @(
			@{ Path="$condep_console\$condep.exe"; Target="lib/net40"},
			@{ Path="$condep_node\ConDepNode.exe"; Target="lib/net40"},
			@{ Path="$condep_console\$condep_remote.dll"; Target="lib/net40"},
			@{ Path="$condep_console\$condep_web_q_client.dll"; Target="lib/net40"},
			@{ Path="$condep_console\ConDep.WebQ.Data.dll"; Target="lib/net40"},
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
		-releaseNotes "$releaseNotes" `
		-tags "Continuous Deployment Delivery Infrastructure WebDeploy Deploy msdeploy IIS automation powershell remote" `
		-dependencies @(
			@{ Name="$condep_dsl"; Version="$nugetVersion$preString"},
			@{ Name="ArrLoadBalancerCmdlet"; Version="1.0.2"}
		) `
		-files @(@{ Path="$condep_dsl_lb_arr\$condep_dsl_lb_arr.dll"; Target="lib/net40"} )       
}

task Build-ConDep-Dsl-LB-ACE -depends Clean-ConDep-Dsl-LB-ACE, Init { 
	Exec { msbuild "$pwd\$condep_dsl_lb_ace\$condep_dsl_lb_ace.csproj" /t:Build /p:Configuration=$configuration /p:OutDir=$build_directory\$condep_dsl_lb_ace\ }

	Generate-Nuspec-File `
		-file "$build_directory\$condep_dsl_lb_ace.nuspec" `
		-version $nugetVersion `
		-preString $preString `
		-id "$condep_dsl_lb_ace" `
		-title "$condep_dsl_lb_ace" `
		-licenseUrl "http://www.con-dep.net/license/" `
		-projectUrl "http://www.con-dep.net/" `
		-description "ConDep is a highly extendable Domain Specific Language for Continuous Deployment, Continuous Delivery and Infrastructure as Code on Windows." `
		-iconUrl "https://raw.github.com/torresdal/ConDep/master/images/ConDepNugetLogo.png" `
		-releaseNotes "$releaseNotes" `
		-tags "Continuous Deployment Delivery Infrastructure WebDeploy Deploy msdeploy IIS automation powershell remote" `
		-dependencies @(
			@{ Name="$condep_dsl"; Version="$nugetVersion$preString"}
		) `
		-files @(@{ Path="$condep_dsl_lb_ace\$condep_dsl_lb_ace.dll"; Target="lib/net40"} )       
}

task Build-Tests -depends Clean-Tests, Init {
	Exec { msbuild "$pwd\$condep_dsl_tests\$condep_dsl_tests.csproj" /t:Build /p:Configuration=$configuration /p:OutDir=$build_directory\$condep_dsl_tests\ }
	Exec { msbuild "$pwd\$condep_webq_tests\$condep_webq_tests.csproj" /t:Build /p:Configuration=$configuration /p:OutDir=$build_directory\$condep_webq_tests\ }
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

task Clean-ConDep-Node {
	Write-Host "Cleaning Build output"  -ForegroundColor Green
	Remove-Item $build_directory\$condep_node -Force -Recurse -ErrorAction SilentlyContinue
}

task Clean-ConDep-Node-Client {
	Write-Host "Cleaning Build output"  -ForegroundColor Green
	Remove-Item $build_directory\$condep_node_client -Force -Recurse -ErrorAction SilentlyContinue
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

task Clean-ConDep-Dsl-LB-ACE {
	Write-Host "Cleaning Build output"  -ForegroundColor Green
	Remove-Item $build_directory\$condep_dsl_lb_ace -Force -Recurse -ErrorAction SilentlyContinue
}

task Clean-Tests {
	Write-Host "Cleaning Build output"  -ForegroundColor Green
	Remove-Item $build_directory\$condep_dsl_tests -Force -Recurse -ErrorAction SilentlyContinue
	Remove-Item $build_directory\$condep_webq_tests -Force -Recurse -ErrorAction SilentlyContinue
}
