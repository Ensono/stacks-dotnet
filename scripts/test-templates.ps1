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
    },
    @{ 
        Name = "Simple.Api.Azure";
        Template = "stacks-webapi";
        Arguments = "-do Menu --cloudProvider Azure --cicdProvider AZDO";
        Path = "$TestTemplatesDirectory/Simple.Api.Azure/src/simple-api/src/api"
    },
    @{ 
        Name = "Simple.Api.Azure.AKS";
        Template = "stacks-webapi";
        Arguments = "-do Menu --cloudProvider Azure --cicdProvider AZDO --deploymentMode AKS";
        Path = "$TestTemplatesDirectory/Simple.Api.Azure/src/simple-api/src/api"
    },
    @{ 
        Name = "Simple.Api.Azure.ACA";
        Template = "stacks-webapi";
        Arguments = "-do Menu --cloudProvider Azure --cicdProvider AZDO --deploymentMode ACA";
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
        Name = "Cqrs.AllTheThings.AKS";
        Template = "stacks-cqrs";
        Arguments = "-do Menu --cloudProvider Azure --cicdProvider AZDO -db CosmosDb -e ServiceBus --deploymentMode AKS";
        Path = "$TestTemplatesDirectory/Cqrs.AllTheThings/src/cqrs/src/api"
    },
    @{ 
        Name = "Cqrs.AllTheThings.ACA";
        Template = "stacks-cqrs";
        Arguments = "-do Menu --cloudProvider Azure --cicdProvider AZDO -db CosmosDb -e ServiceBus --deploymentMode ACA";
        Path = "$TestTemplatesDirectory/Cqrs.AllTheThings/src/cqrs/src/api"
    },
    @{ 
        Name = "Cqrs.ServiceBus";
        Template = "stacks-cqrs";
        Arguments = "-do Menu --cloudProvider Azure --cicdProvider AZDO -e ServiceBus";
        Path = "$TestTemplatesDirectory/Cqrs.ServiceBus/src/cqrs/src/api"
    },
    @{ 
        Name = "Cqrs.ServiceBus.AKS";
        Template = "stacks-cqrs";
        Arguments = "-do Menu --cloudProvider Azure --cicdProvider AZDO -e ServiceBus --deploymentMode AKS";
        Path = "$TestTemplatesDirectory/Cqrs.ServiceBus/src/cqrs/src/api"
    },
    @{ 
        Name = "Cqrs.ServiceBus.ACA";
        Template = "stacks-cqrs";
        Arguments = "-do Menu --cloudProvider Azure --cicdProvider AZDO -e ServiceBus --deploymentMode ACA";
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
    },
    @{ 
        Name = "Background.Worker";
        Template = "stacks-asb-worker";
        Arguments = "-do Menu";
        Path = "$TestTemplatesDirectory/Background.Worker/src/background-worker/src/worker"
    },
    @{ 
        Name = "Background.Worker.AKS";
        Template = "stacks-asb-worker";
        Arguments = "-do Menu --deploymentMode AKS";
        Path = "$TestTemplatesDirectory/Background.Worker/src/background-worker/src/worker"
    }
    @{ 
        Name = "Background.Worker.ACA";
        Template = "stacks-asb-worker";
        Arguments = "-do Menu --deploymentMode ACA";
        Path = "$TestTemplatesDirectory/Background.Worker/src/background-worker/src/worker"
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

            ############################
            # Comparing the generated directory tree with the expected tree files was introduced in Cycle 15
            # This is commented out as it proved cumbersome to maintain and was not providing much value
            # It is left here to be revisited in the future
            ############################
            # & "$PSScriptRoot/compare-directory-files.ps1" -InputFile "$StacksDotnetDirectory/scripts/expected-trees/$($project.Name).tree" -Directory $TestTemplatesDirectory/$($project.Name)
            # if ($LASTEXITCODE -ne 0) {
            #     Write-Error "Directory comparison failed for $($project.Name)"
            #     exit $LASTEXITCODE
            # }

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
