param (
    [string]$Branch,
    [string]$Template,
    [switch]$GenerateExpectedTrees = $false
)

$TempDirectory = [System.IO.Path]::GetTempPath()
$TestingDirectory = "$TempDirectory/stacks-dotnet-testing"
$StacksDotnetDirectory = [string]::IsNullOrEmpty($Branch) ? "$(Split-Path $PSScriptRoot -Parent)" : "$TestingDirectory/stacks-dotnet"
$TestTemplatesDirectory = "$TestingDirectory/test-templates"

# Create the testing directory
if (-Not (Test-Path $TestingDirectory)) {
    New-Item -ItemType Directory -Path $TestingDirectory
}

# Clone the stacks-dotnet repository if branch parameter is set
if (!([string]::IsNullOrEmpty($Branch))) {
    if (Test-Path $StacksDotnetDirectory) {
        Remove-Item -Recurse -Force $StacksDotnetDirectory
    }
    git clone -b $Branch git@github.com:Ensono/stacks-dotnet.git $StacksDotnetDirectory
}

if (!$GenerateExpectedTrees) {
    # Change directory to the simple-api project and run the tests
    Push-Location -Path "$StacksDotnetDirectory/src/simple-api/src/api" && dotnet restore && dotnet test --filter TestType!=IntegrationTests && Pop-Location

    # Change directory to the cqrs project and run the tests
    Push-Location -Path "$StacksDotnetDirectory/src/cqrs/src/api" && dotnet restore && dotnet test  --filter TestType!=IntegrationTests && Pop-Location
}

# Install dotnet templates
dotnet new install $StacksDotnetDirectory --force

# Create testing directory
if (Test-Path $TestTemplatesDirectory) {
    Remove-Item -Recurse -Force $TestTemplatesDirectory
}
New-Item -ItemType Directory -Path $TestTemplatesDirectory

$projects = @(
    @{ 
        Name = "Simple.Api";
        Template = "stacks-webapi";
        Arguments = "-do Menu";
        Path = "$TestTemplatesDirectory/Simple.Api/src/simple-api/src/api"
    }
)

Write-Output "Creating test projects"
Push-Location -Path $TestTemplatesDirectory
$projects | ForEach-Object {
    $project = $_
    if ([string]::IsNullOrEmpty($Template) -or $($project.Name -eq $Template)) {
        dotnet new ($project.Template) -n ($project.Name) ($null -eq $project.Arguments ? $null : $project.Arguments.split(' '))
        if ($GenerateExpectedTrees) {
            Write-Output "Generating expected directory tree for $($project.Name)"
            Push-Location -Path ($project.Name)
            tree -a --noreport > $PSScriptRoot/expected-trees/$($project.Name).tree
            Pop-Location
        }
    }
}
Pop-Location

if (!$GenerateExpectedTrees) {
    # Test each project
    foreach ($project in $projects) {
        if ([string]::IsNullOrEmpty($Template) -or $($project.Name -eq $Template)) {
            Write-Output "Testing $($project.Name)"
            & "$PSScriptRoot/compare-directory-files.ps1" -InputFile "$StacksDotnetDirectory/scripts/expected-trees/$($project.Name).tree" -Directory $TestTemplatesDirectory/$($project.Name)
            if ($LASTEXITCODE -ne 0) {
                Write-Error "Directory comparison failed for $($project.Name)"
                exit $LASTEXITCODE
            }
            Push-Location -Path $project.Path
            dotnet restore
            dotnet build
            dotnet test --filter TestType!=IntegrationTests
            if ($LASTEXITCODE -ne 0) {
                Pop-Location
                Write-Error "Tests failed for $($project.Name)"
                exit $LASTEXITCODE
            }
            Pop-Location
        }
    }

    # Print success message
    Write-Output @"
_________ _______  _______ _________ _______    _______  _______  _______  _______  _______  ______  
\__   __/(  ____ \(  ____ \\__   __/(  ____ \  (  ____ )(  ___  )(  ____ \(  ____ \(  ____ \(  __  \ 
    ) (   | (    \/| (    \/   ) (   | (    \/  | (    )|| (   ) || (    \/| (    \/| (    \/| (  \  )
    | |   | (__    | (_____    | |   | (_____   | (____)|| (___) || (_____ | (_____ | (__    | |   ) |
    | |   |  __)   (_____  )   | |   (_____  )  |  _____)|  ___  |(_____  )(_____  )|  __)   | |   | |
    | |   | (            ) |   | |         ) |  | (      | (   ) |      ) |      ) || (      | |   ) |
    | |   | (____/\/\____) |   | |   /\____) |  | )      | )   ( |/\____) |/\____) || (____/\| (__/  )
    )_(   (_______/\_______)   )_(   \_______)  |/       |/     \|\_______)\_______)(_______/(______/
"@
}
