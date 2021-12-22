
function Find-Command {

    [CmdletBinding()]
    param (

        [string]
        # Name of the command to find
        $Name
    )

    # Find the path to the named command
    $command = Get-Command -Name $Name -ErrorAction SilentlyContinue
    if ([string]::IsNullOrEmpty($command)) {
        Write-Error -Message ("'{0}' command cannot be found in the path, please ensure it is installed" -f $Name)
        exit 1
    } else {
        Write-Host ("Tool found: {0}" -f $command.Source)
    }

    return $command
}