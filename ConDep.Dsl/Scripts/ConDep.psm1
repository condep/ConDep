Set-StrictMode -Version 2
Get-ChildItem -Path $PSScriptRoot\*.ps1 | Foreach-Object{ . $_.FullName }
Export-ModuleMember -Function *-*
