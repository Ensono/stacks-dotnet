
<#

.SYNOPSIS
Connect to azure using environment variables for the parameters

#>

function Connect-Azure() {

    [CmdletBinding()]
    param (

        [Alias("clientid")]
        [string]
        # ID of the service principal
        $id = $env:AZURE_CLIENT_ID,

        [string]
        # Secret for the service principal
        $secret = $env:AZURE_CLIENT_SECRET,

        [string]
        # Subscription ID 
        $subscriptionId = $env:AZURE_SUBSCRIPTION_ID,

        [string]
        # Tenant ID
        $tenantId = $env:AZURE_TENANT_ID

    )

    # ensure that all the values have been set
    $required = @("id", "secret", "subscriptionId", "tenantId")
    $missing = @()
    foreach ($req in $required) {

        # Get the value of the variable and make sure it is not null
        $var = Get-Variable -Name $req -ErrorAction SilentlyContinue

        if ([string]::IsNullOrEmpty($var)) {
            $missing += $var
        }
    }

    # check that the missing vars is empty
    if ($missing.Count -gt 0) {
        Write-Error -Message ("Missing required values: {0}" -f ($missing -Join ", "))
        exit 1
    }

    # Create a secret to be used with the credential
    $secret = ConvertTo-SecureString -String $secret -AsPlainText -Force

    # Create the credential to log in
    $credential = New-Object -TypeName "System.Management.Automation.PSCredential" -ArgumentList $id, $secret

    Connect-AzAccount -Credential $credential -Tenant $tenantId -Subscription $subscriptionId -ServicePrincipal
}