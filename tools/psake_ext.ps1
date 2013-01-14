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
	[string]$id,
	[string]$title,
	[string]$licenseUrl,
	[string]$projectUrl,
	[string]$description,
	[string]$iconUrl,
	[string]$releaseNotes,
	[string]$tags,
	[array]$dependencies,
	[array]$frameworkAssemblies,
	[array]$files,
	[string]$file = $(throw "file is a required parameter.")
)
	$preString = if($pre_release) { "-rc" } else { "" }	
	$nuspec = "<?xml version=""1.0""?>
<package>
  <metadata>
    <id>$id</id>
    <version>$version$preString</version>
    <title>$title</title>
    <authors>Jon Arild Torresdal</authors>
    <owners>Jon Arild Torresdal</owners>
    <licenseUrl>$licenseUrl</licenseUrl>
    <projectUrl>$projectUrl</projectUrl>
    <requireLicenseAcceptance>false</requireLicenseAcceptance>
    <description>$description</description>
    <iconUrl>$iconUrl</iconUrl>
    <releaseNotes>$releaseNotes</releaseNotes>
    <copyright>Copyright 2012</copyright>
    <tags>$tags</tags>
    "
  if($dependencies -ne $null) {
    $nuspec += "    <dependencies>
"
    	$dependencies | foreach {
      $nuspec += "<dependency id=""" + $_.Name + """ version=""" + $_.Version + """  />
      "
      }
    $nuspec += "</dependencies>"
    }

  if($frameworkAssemblies -ne $null) {
    $nuspec += "    <frameworkAssemblies>
"
    	$frameworkAssemblies | foreach {
      $nuspec += "<frameworkAssembly assemblyName=""" + $_.Name + """ targetFramework=""" + $_.Target + """  />
      "
      }
    $nuspec += "</frameworkAssemblies>"
    }


  $nuspec += "</metadata>
  "
  if($files -ne $null) {
    $nuspec += "  <files>
    "
  	$files | foreach {
  	$exclude = ""
	if($_.Exclude -ne $null) { $exclude = " exclude=""" + $_.Exclude + """"}
    $nuspec += "<file src=""" + $_.Path + """" + $exclude + " target=""" + $_.Target + """ />"
    }
  $nuspec += "
  </files>
  "
  }

$nuspec += "</package>"

	$dir = [System.IO.Path]::GetDirectoryName($file)
	if ([System.IO.Directory]::Exists($dir) -eq $false)
	{
		Write-Host "Creating directory $dir"
		[System.IO.Directory]::CreateDirectory($dir)
	}
	Write-Host "Generating Nuspec file: $file"
	Write-Output $nuspec > $file
}