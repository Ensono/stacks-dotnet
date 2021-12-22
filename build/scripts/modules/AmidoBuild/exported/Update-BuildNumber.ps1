<#

.SYNOPSIS
Update the build number

.DESCRIPTION
Depending on the platform being run, update the build number accordingly

#>

function Update-BuildNumber() {
    [CmdletBinding()]
    param (

    )

    # If the TF_BUILD environment variable is defined, then running on an Azure Devops build agent
    if (Test-Path env:TF_BUILD) {
        Write-Output ("##vso[build.updatebuildnumber]{0}" -f $env:DOCKER_IMAGE_TAG)
    }
}