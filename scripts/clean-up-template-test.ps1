$TempDirectory = [System.IO.Path]::GetTempPath()
$TestingDirectory = "$TempDirectory/stacks-dotnet-testing"

if (Test-Path $TestingDirectory) {
    Remove-Item -Recurse -Force $TestingDirectory
}