<#

.SYNOPSIS
Initialise terraform with the necssary backend and credentials

#>

function Invoke-TerraformInit() {
    [CmdletBinding()]
    param (

        [Alias("path")]
        [string]
        # Path to the terraform files
        $workingDirectory,

        [Alias("backend")]
        [string[]]
        # Backend properties to be used in the configuration
        # This is a list of properties to be set
        $properties = $env:BACKEND_PROPERTIES
    )

    # Check that necessary variables have been set
    if ([String]::IsNullOrEmpty($workingDirectory)) {
        Write-Error -Message "A directory containing the Terraform files must be specified"
        exit 1 
    }

    # Check that some backend properties have been set
    if ($properties.Count -eq 0) {
        Write-Error -Message "No properties have been specified for the backend"
        exit 2
    }

    # Find the path to terraform
    $terraform = Find-Command -Name "terraform"

    # Define array to hold the arguments for the terraform command
    $arguments = @()

    # Iterate around the properties and as arguments
    foreach ($prop in $properties) {
        $arguments += "-backend-config={0}" -f $prop
    }

    # Build up the command to pass
    $cmd = "{0} init {1}" -f $terraform, ($arguments -join (" "))

    Invoke-External -Command $cmd

    return $LASTEXITCODE
}