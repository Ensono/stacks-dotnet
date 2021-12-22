function Invoke-YamlLint() {
    [CmdletBinding()]
    param (
        [Alias("a")]
        [string]
        # Config File
        $ConfigFile = "yamllint.conf",

        [Alias("b")]
        [string]
        # Base path to search
        $BasePath = "."
    )

    # Check that arguments have been supplied
    if ([string]::IsNullOrEmpty($ConfigFile)) {
        Write-Error -Message "-a, -configfile: Missing path to configuration file"
        exit 1
    }

    if ([string]::IsNullOrEmpty($BasePath)) {
        Write-Error -Message "-b, -basepath: Missing base path to scan"
        exit 2
    }

    # Check that the config file can be located
    if (!(Test-Path -Path $ConfigFile)) {
        Write-Error -Message ("ConfigFile cannot be located: {0}" -f $ConfigFile)
        exit 4
    }

    # Find the path to python
    # Look for python3, if that fails look for python but then check the version
    $python = Get-Command -Name "python3" -ErrorAction SilentlyContinue
    if ([string]::IsNullOrEmpty($python)) {
        Write-Debug -Message "Cannot find 'python3'. Looking for 'python' and checking version"

        $python = Get-Command -Name "python" -ErrorAction SilentlyContinue

        if ([string]::IsNullOrEmpty($python)) {
            Write-Error -Message "Python3 cannot be found, please ensure it is installed"
            exit 3
        }

        # Check the version of pythong
        $cmd = "{0} -V" -f $python
        $result = Invoke-External -Command $cmd

        if (!$result.StartsWith("Python 3")) {
            Write-Error -Message "Python3 cannot be found, please ensure it is installed"
            exit 4        
        }
    }

    # Create the command that needs to be run to perform the lint function
    $cmd = "{0} -m yamllint -sc {1} {2} {3}" -f $python.Source, $ConfigFile, $BasePath, $ConfigFile
    Invoke-External -Command $cmd

    exit $LASTEXITCODE
}