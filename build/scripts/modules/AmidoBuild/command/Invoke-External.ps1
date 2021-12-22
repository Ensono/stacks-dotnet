<#

.SYNOPSIS
Runs external commands with the specified options

.DESCRIPTION
This is a helper function that executes external binaries. All cmdlets and functions that require
executables to be run should use this function. This is so that the Pester tests can mock the function
and Unit tests are possible on all scripts

#>

function Invoke-External {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory=$true)]
        [string]
        # Command and arguments to be run
        $command
    )

    Write-Debug -Message $command

    Invoke-Expression -Command $command
}