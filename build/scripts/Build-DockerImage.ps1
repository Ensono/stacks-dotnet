<#

.SYNOPSIS
Create a Docker image for the application

#>

[CmdletBinding()]
param (

    [string]
    # Arguments for docker build
    $buildargs = ".",

    [string]
    # Name of the docker image
    $name = $env:DOCKER_IMAGENAME,

    [string]
    # Image tag
    $tag = $env:DOCKER_IMAGETAG,

    [string]
    # Docker registry to push the image to
    $registry = $env:DOCKER_CONTAINERREGISTRYNAME,

    [switch]
    # Push the image to the specified registry
    $push

)

# Import helper functions
$parentDir = Split-Path -Path ($MyInvocation.MyCommand.Path) -Parent
$functions = Get-ChildItem -Path ([System.IO.Path]::Combine($parentDir, "functions")) -Filter '*.ps1'
$functions | Foreach-Object { . $_ }

# Create an array to store the arguments to pass to docker
$arguments = @()
$arguments += $buildArgs
$arguments += "-t {0}:{1}" -f $name, $tag

# if the registry name has been set, add t to the tasks
if (![String]::IsNullOrEmpty($registry)) {
    $arguments += "-t {0}/{1}:{2}" -f $registry, $name, $tag
    $arguments += "-t {0}/{1}:latest" -f $registry, $name
}

# Create the cmd to execute
$cmd = "docker build {0}" -f ($arguments -Join " ")

Invoke-External -Command $cmd

if ($LASTEXITCODE -ne 0) {
    exit $LASTEXITCODE
}

# Only proceed if the flag to push the image has been set
if (!$push.IsPresent) {
    exit 0
}

# Check the Get-AzContainerRegistryCredential cmdlet exists
$exists = Get-Command -Name "Get-AzContainerRegistryCredential" -ErrorAction SilentlyContinue
if ([string]::IsNullOrEmpty($exists)) {
    Write-Warning -Message "Az PowerShell module is not installed so cannot get container registry credentials"
    exit 0
}

# Proceed if a registry has been specified
if (![String]::IsNullOrEmpty($registry) -and $push.IsPresent) {

    # Get the credentials for the registry
    $creds = Get-AzContainerRegistryCredential -Name $registry

    # Run command to login to the docker registry to do the push
    # The Invoke-External function will need to be updated to obfruscate sensitive information
    $cmd = "docker login {0} -u {1} -p {2}" -f $registry, $creds.Username, $creds.Password
    Invoke-External -Command $cmd

    if ($LASTEXITCODE -ne 0) {
        exit $LASTEXITCODE
    }

    # Finally push the image
    $cmd = "docker push {0}/{1}:{2}" -f $registry, $name, $tag
    Invoke-External -Command $cmd

    $LASTEXITCODE

}