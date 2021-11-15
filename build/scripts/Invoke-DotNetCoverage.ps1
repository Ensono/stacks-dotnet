<#

.SYNOPSIS
Finds any unit test coverage report files can converts them into Cobertura

.DESCRIPTION
This script will install the dotnet-reportgenerator-globaltool and then find all the 
*.opencover.xml which it will convert to Cobertura format 

.PARAMETER path
Path that the script should look for opencover.xml files from. Default: .

.PARAMETER toolpath
Path that the tool should be installed into. Default: .

.PARAMETER target
Directory that should be used for the report files. Default: coverage

.PARAMETER types
The types of report that should be generated. Default: Cobertura

If multiple types are required, they should be separated by a ';'

#>

[CmdletBinding()]
param (

    [string]
    # Path that files should be looked for
    $path = ".",

    [string]
    # Path that the tool should be installed in
    $toolpath = ".",

    [string]
    # Directory that should be used for the resultant files
    $target = "coverage",

    [string]
    # Types of report that should be generated
    $types = "Cobertura"
)

# Import helper functions
$parentDir = Split-Path -Path ($MyInvocation.MyCommand.Path) -Parent
$functions = Get-ChildItem -Path ([System.IO.Path]::Combine($parentDir, "functions")) -Filter '*.ps1'
$functions | Foreach-Object { . $_ }

# Determine if the reportgenerator tool is installed
# The dotnet command will state that it is, but will return a non-zero exit code which will stop the pipeline
$cmdName = "reportgenerator"
if ($IsWindows) {
    $cmdName += ".exe"
}
$reportGeneratorPath = [IO.Path]::Combine($toolpath, $cmdName)

if (!(Test-Path -Path $reportGeneratorPath)) {
    Write-Information -MessageData "Installing ReportGenerator tool"

    # Build up command to install the report converter tool
    $cmd = "dotnet tool install dotnet-reportgenerator-globaltool --tool-path {0}" -f $toolpath
    write-host $cmd
    Invoke-External -Command $cmd

    # if there was problem installing the tool, exit
    if ($LASTEXITCODE -ne 0) {
        exit $LASTEXITCODE
    }    
}

# Find all the files that match the pattern 
$coverFiles = Find-Projects -Pattern "*.opencover.xml" -Path $path

# Iterate around all of the cover files and create an array of the paths that 
# can then be passed to the generator as a comma delimited list
if ($coverFiles.Count -eq 0) {
    Write-Error -Message "No coverage files can be found"
    exit 1
}

$list = @()
foreach ($coverFile in $coverFiles) {
    $list += $coverFile.Fullname
}

# Build up the command to run
$cmd = "{0} -reports:{1} -targetdir:{2} -reporttypes:{3}" -f $reportGeneratorPath,
    ($list -join ";"),
    $target,
    $types

Invoke-External -Command $cmd

$LASTEXITCODE