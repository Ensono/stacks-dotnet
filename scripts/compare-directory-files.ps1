param (
    [string]$InputFile,
    [string]$Directory
)

if (-Not (Test-Path $InputFile)) {
    Write-Error "Input file does not exist."
    exit 1
}

if (-Not (Test-Path $Directory)) {
    Write-Error "Directory does not exist."
    exit 1
}

$inputStructure = Get-Content -Path $InputFile
$generatedStructure = Push-Location -Path $Directory && Tree -a -I 'bin|obj' && Pop-Location

# Compare the two structures
$differences = Compare-Object -ReferenceObject $inputStructure -DifferenceObject $generatedStructure

if ($differences) {
    Write-Output "Differences found: $($differences.Count)"
    
    # Output the differences and then exit with an error code
    $differences | ForEach-Object {
        Write-Output $_
    }
    exit 1
} else {
    Write-Output "The directory structures match."
}