[CmdletBinding()]
param (

    [string]
    # Directory to perform build from
    $workingDirectory
)

# Import helper functions
$parentDir = Split-Path -Path ($MyInvocation.MyCommand.Path) -Parent
$functions = Get-ChildItem -Path ([System.IO.Path]::Combine($parentDir, "functions")) -Filter '*.ps1'
$functions | Foreach-Object { . $_ }

# Change to the workingdirectory if one has been set
if (![String]::IsNullOrEmpty($workingDirectory) -and (Test-Path -Path $workingDirectory)) {
    Set-Location -Path $workingDirectory
}

Write-Information -MessageData ("Working directory: {0}" -f $workingDirectory)

# Look for the dotnet command
$dotnet = Find-Command -Name "dotnet"

$cmd = "{0} build" -f $dotnet

Invoke-External -Command $cmd

$LASTEXITCODE