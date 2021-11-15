[CmdletBinding()]
param (

    [string]
    # Pattern to use to find the projects which need to run
    $pattern = $env:pattern,

    [string]
    # Directory to perform build from
    $path = $PWD,

    [string]
    # Additional arguments to be passed to the test
    $arguments = $env:arguments
)

# Import helper functions
$parentDir = Split-Path -Path ($MyInvocation.MyCommand.Path) -Parent
$functions = Get-ChildItem -Path ([System.IO.Path]::Combine($parentDir, "functions")) -Filter '*.ps1'
$functions | Foreach-Object { . $_ }

# Change to the workingdirectory if one has been set
if (![String]::IsNullOrEmpty($path) -and (Test-Path -Path $path)) {
    Set-Location -Path $path
}
Write-Host ("Pattern: {0}" -f $pattern)
# Get a list of all the UnitTest projects using the pattern
$projects = Find-Projects -Path $path -Pattern $pattern -Directory

if ($projects.count -eq 0) {
    Write-Warning -Message ("No tests matching the pattern '{0}' can be found" -f $pattern)
    exit 0
}

# Create array that the exit code from the previoust test can be stored
# This is so that the tests can all be run, but the ones that fail will fail the
# entire cmdlet
$exitcodes = @()

# Iterate around each of the tests that have been found and run them in
# that directory
foreach ($project in $projects) {

    # create command to use to test the current proiject
    $cmd = "dotnet test {0} {1}" -f $project.Fullname, $arguments

    Invoke-External -Command $cmd

    $exitcodes += $LASTEXITCODE
}

# If there is non-zero exit code in the exitcodes exit with non-zero
$nonzero = $exitcodes -gt 0
if ($nonzero) {
    exit 1
} else {
    exit 0
}

