param (
    [string]$Branch,
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
    Push-Location -Path "$StacksDotnetDirectory/src/simple-api/src/api" && dotnet restore && dotnet test && Pop-Location

    # Change directory to the cqrs project and run the tests
    Push-Location -Path "$StacksDotnetDirectory/src/cqrs/src/api" && dotnet restore && dotnet test && Pop-Location
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
    },
    @{ 
        Name = "Simple.Api.Azure";
        Template = "stacks-webapi";
        Arguments = "-do Menu --cloudProvider Azure --cicdProvider AZDO";
        Path = "$TestTemplatesDirectory/Simple.Api.Azure/src/simple-api/src/api"
    },
    @{ 
        Name = "Simple.Api.AWS";
        Template = "stacks-webapi";
        Arguments = "-do Menu --cloudProvider AWS --cicdProvider GHA";
        Path = "$TestTemplatesDirectory/Simple.Api.AWS/src/simple-api/src/api"
    },
    @{ 
        Name = "Cqrs";
        Template = "stacks-cqrs";
        Arguments = "-do Menu";
        Path = "$TestTemplatesDirectory/Cqrs/src/cqrs/src/api"
    },
    @{ 
        Name = "Cqrs.AllTheThings";
        Template = "stacks-cqrs";
        Arguments = "-do Menu --cloudProvider Azure --cicdProvider AZDO -db CosmosDb -e ServiceBus";
        Path = "$TestTemplatesDirectory/Cqrs.AllTheThings/src/cqrs/src/api"
    },
    @{ 
        Name = "Cqrs.ServiceBus";
        Template = "stacks-cqrs";
        Arguments = "-do Menu --cloudProvider Azure --cicdProvider AZDO -e ServiceBus";
        Path = "$TestTemplatesDirectory/Cqrs.ServiceBus/src/cqrs/src/api"
    },
    @{ 
        Name = "Cqrs.Dynamo";
        Template = "stacks-cqrs";
        Arguments = "-do Menu --cloudProvider AWS --cicdProvider GHA -db DynamoDb";
        Path = "$TestTemplatesDirectory/Cqrs.Dynamo/src/cqrs/src/api"
    },
    @{ 
        Name = "Cqrs.Sns";
        Template = "stacks-cqrs";
        Arguments = "-do Menu --cloudProvider AWS --cicdProvider GHA -e AwsSns";
        Path = "$TestTemplatesDirectory/Cqrs.Sns/src/cqrs/src/api"
    },
    @{ 
        Name = "Cosmos.Worker";
        Template = "stacks-az-func-cosmosdb-worker";
        Path = "$TestTemplatesDirectory/Cosmos.Worker/src/func-cosmosdb-worker/src/functions"
    },
    @{ 
        Name = "EventHub.Listener";
        Template = "stacks-az-func-aeh-listener";
        Arguments = "-do Menu";
        Path = "$TestTemplatesDirectory/EventHub.Listener/src/func-aeh-listener/src/functions"
    },
    @{ 
        Name = "ServiceBus.Listener";
        Template = "stacks-az-func-asb-listener";
        Arguments = "-do Menu";
        Path = "$TestTemplatesDirectory/ServiceBus.Listener/src/func-asb-listener/src/functions"
    }
)

Write-Output "Creating test projects"
Push-Location -Path $TestTemplatesDirectory
$projects | ForEach-Object {
    $project = $_
    dotnet new ($project.Template) -n ($project.Name) ($null -eq $project.Arguments ? $null : $project.Arguments.split(' '))
    if ($GenerateExpectedTrees) {
        Write-Output "Generating expected directory tree for $($project.Name)"
        Push-Location -Path ($project.Name)
        tree -a > $PSScriptRoot/expected-trees/$($project.Name).tree
        Pop-Location
    }
}
Pop-Location

if (!$GenerateExpectedTrees) {
    # Test each project
    foreach ($project in $projects) {
        Write-Output "Testing $($project.Name)"
        & "$PSScriptRoot/compare-directory-files.ps1" -InputFile "$StacksDotnetDirectory/scripts/expected-trees/$($project.Name).tree" -Directory $TestTemplatesDirectory/$($project.Name)
        if ($LASTEXITCODE -ne 0) {
            Write-Error "Directory comparison failed for $($project.Name)"
            exit $LASTEXITCODE
        }
        Push-Location -Path $project.Path
        dotnet restore
        dotnet build
        dotnet test
        if ($LASTEXITCODE -ne 0) {
            Pop-Location
            Write-Error "Tests failed for $($project.Name)"
            exit $LASTEXITCODE
        }
        Pop-Location
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
