function Generate-Assembly-Info
{
param(
	[string]$clsCompliant = "true",
	[string]$company, 
	[string]$copyright, 
	[string]$version,
	[string]$file = $(throw "file is a required parameter.")
)
  $asmInfo = "using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyCompanyAttribute(""$company"")]
[assembly: AssemblyProductAttribute(""$product"")]
[assembly: AssemblyCopyrightAttribute(""$copyright"")]
[assembly: AssemblyVersionAttribute(""$version"")]
[assembly: AssemblyFileVersionAttribute(""$version"")]
[assembly: CLSCompliantAttribute($clsCompliant )]
"

	$dir = [System.IO.Path]::GetDirectoryName($file)
	if ([System.IO.Directory]::Exists($dir) -eq $false)
	{
		Write-Host "Creating directory $dir"
		[System.IO.Directory]::CreateDirectory($dir)
	}
	Write-Host "Generating assembly info file: $file"
	Write-Output $asmInfo > $file
}

function Generate-Nuspec-File
{
param(
	[string]$version,
	[bool]$pre_release,
	[string]$file = $(throw "file is a required parameter.")
)
	$preString = if($pre_release) { "-pre" } else { "" }	
	$nuspec = "<?xml version=""1.0""?>
<package>
  <metadata>
    <id>ConDep.Dsl</id>
    <version>$version$preString</version>
    <title>ConDep.Dsl</title>
    <authors>Jon Arild Torresdal</authors>
    <owners>Jon Arild Torresdal</owners>
    <licenseUrl>https://github.com/torresdal/ConDep/blob/master/LICENSE</licenseUrl>
    <projectUrl>https://github.com/torresdal/ConDep/</projectUrl>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>ConDep is a highly extendable Domain Specific Language for Continuous Deployment, Continuous Delivery and Infrastructure as Code on Windows</description>
    <iconUrl>https://raw.github.com/torresdal/ConDep/master/images/ConDepNugetLogo.png</iconUrl>
    <releaseNotes>Initial pre-release.</releaseNotes>
    <copyright>Copyright 2012</copyright>
    <tags>Continuous Deployment Delivery Infrastructure WebDeploy Deploy</tags>
    <dependencies>
      <dependency id=""NDesk.Options"" version=""0.2.1""  />
      <dependency id=""Newtonsoft.Json"" version=""4.5.8""  />
    </dependencies>
  </metadata>
  <files>
    <file src=""Build\*.dll"" target=""lib\net40"" />
    <file src=""Build\ConDep.exe*"" target=""tools\net40"" />
  </files>
</package>"

	$dir = [System.IO.Path]::GetDirectoryName($file)
	if ([System.IO.Directory]::Exists($dir) -eq $false)
	{
		Write-Host "Creating directory $dir"
		[System.IO.Directory]::CreateDirectory($dir)
	}
	Write-Host "Generating Nuspec file: $file"
	Write-Output $nuspec > $file
}