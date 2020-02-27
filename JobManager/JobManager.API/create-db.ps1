
param (
	[switch]$SkipDbCreation
	)
	
. $PSScriptRoot\..\..\..\..\scripts\registry-utils.ps1

#------------------------
# Determine where PostgreSQL is installed, so we don't rely on the Path
$postgresPath = GetPostgreSQL
if ($postgresPath -eq $null) {
	exit 1
}
#------------------------

Write-Host Create job_management Database

$scriptsPath = Join-Path -Path $PSScriptRoot -ChildPath "DataAccess\Scripts"

$dropDbPath = Join-Path -Path $scriptsPath -ChildPath "service-drop-db.sql"
$createDbPath = Join-Path -Path $scriptsPath -ChildPath "service-create-db.sql"

if ($SkipDbCreation -eq $false) {
	& "$($postgresPath)psql" -U postgres -d postgres -f $dropDbPath
	& "$($postgresPath)psql" -U postgres -d postgres -f $createDbPath
}

<#
Keeping the SPs in code

$createProcScripts = Get-ChildItem $scriptsPath\* -Include *-create-procs.sql
foreach($script in $createProcScripts)
{
	& "$($postgresPath)psql" -U postgres -d postgres -f $script
}
#>
