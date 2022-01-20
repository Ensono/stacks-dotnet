
function Invoke-Templater() {

    <#
    
    .SYNOPSIS
        Reads all env vars and, optionally, an env file and replaces values in a template file
    #>

    [CmdletBinding()]
    param (

        [Parameter(
            Mandatory = $true,
            ParameterSetName = "path"
        )]
        [string]
        # path to the list of items
        $path,

        [Alias("tfdata")]
        [string]
        # JSON object representing the outputs from Terraform
        $tfoutputs,

        [string]
        # Base directory to use when paths are relative
        $baseDir = "/app"
    )

    # Get all the enviornment variables
    $envvars = [Environment]::GetEnvironmentVariables()

    # iterate around the variables and create local ones
    foreach ($envvar in $envvars.GetEnumerator()) {

        # Exclude the path env var so that the one that is already
        # set does not get overwritten
        if (@("path") -notcontains $envvar.Name) {
            Set-Variable -Name $envvar.Name -Value $envvar.Value
        }
    }

    # if any tfoutputs have been specified, iterate around the object
    # and set variables for the output keys and the associated value
    if (![string]::IsNullOrEmpty($tfoutputs)) {

        # determine if the tfoutputs is a path, if it is get the data from
        # the file
        if (Test-Path -Path $tfoutputs) {
            $tfoutputs = Get-Content -Path $tfoutputs -Raw
        }

        # convert the tfoutputs to a data object
        $data = $tfoutputs | ConvertFrom-JSON

        # iterate around the data and set local variables
        $data | Get-Member -MemberType NoteProperty | ForEach-Object {
            $name = $_.Name
            Set-Variable -Name $name -Value $data.$name.value
        }
    }

    # Check that the specified path exists
    if ($PSCmdlet.ParameterSetName -eq "path") {
        if (!(Test-Path -Path $path)) {
            Write-Error ("Unable to file list file: {0}" -f $path)
            return
        }

        # Ensure that the path specified is a file
        if ((Get-Item $path) -is [System.IO.DirectoryInfo]) {
            Write-Error -Message ("A file must be specified, directory provided: {0}" -f $path)
            return
        }

        try {
            # Get the list of items as an object
            $items = @()
            $items += Invoke-Expression -Command (Get-Content -Path $path -Raw)

            foreach ($item in $items) {
                Write-Information ("Template: {0}" -f $item.template)

                if (![IO.Path]::IsPathRooted($item.template)) {
                    $item.template = [IO.Path]::Combine($baseDir, $item.template)
                }

                Expand-Template -path $item.template -additional $item.vars
            }

        } catch {

            Write-Debug $_
            Write-Error -Message ("Unable to read specified list file as data. Please ensure that the file contains a valid PowerShell object")
        }
    }
}