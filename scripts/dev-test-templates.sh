#!/bin/bash

# Move to the root directory and declare directory paths
cd ../..
ROOT_DIRECTORY=$(pwd)  
STACKS_TESTING_DIRECTORY="$ROOT_DIRECTORY/STACKS-TESTING"
STACKS_DOTNET_DIRECTORY="$STACKS_TESTING_DIRECTORY/stacks-dotnet"
TEST_TEMPLATES_DIRECTORY="$STACKS_TESTING_DIRECTORY/test-templates"

# Create directory for the repository and testing
mkdir -p $STACKS_TESTING_DIRECTORY
cd $STACKS_TESTING_DIRECTORY

# Clone repository and checkout to the specified branch
git clone git@github.com:Ensono/stacks-dotnet.git
cd $STACKS_DOTNET_DIRECTORY
git checkout $1

# Change directory to the simple-api project and run the tests
cd $STACKS_DOTNET_DIRECTORY/src/simple-api/src/api

dotnet restore
dotnet test

# Change directory to the cqrs project and run the tests
cd $STACKS_DOTNET_DIRECTORY/src/cqrs/src/api

dotnet restore
dotnet test

# Change directory and re-install the templates
cd $STACKS_DOTNET_DIRECTORY

dotnet new uninstall .
dotnet new install .

# Create and change to the test-templates directory
mkdir -p $TEST_TEMPLATES_DIRECTORY
cd $TEST_TEMPLATES_DIRECTORY

# Generate the generic base templates
dotnet new stacks-webapi -n Simple.Api -do Menu
dotnet new stacks-cqrs -n Cqrs -do Menu
dotnet new stacks-az-func-cosmosdb-worker -n Cosmos.Worker
dotnet new stacks-az-func-aeh-listener -n EventHub.Listener -do Menu
dotnet new stacks-az-func-asb-listener -n ServiceBus.Listener -do Menu
dotnet new stacks-asb-worker -n ServiceBus.Worker -do Menu

# Generate fleshed out CQRS templates
dotnet new stacks-cqrs -n Cqrs.AllTheThings -do Menu --cloudProvider Azure --cicdProvider AZDO -db CosmosDb -e ServiceBus
dotnet new stacks-cqrs -n Cqrs.ServiceBus -do Menu --cloudProvider Azure --cicdProvider AZDO -e ServiceBus
dotnet new stacks-cqrs -n Cqrs.Dynamo -do Menu --cloudProvider AWS -db DynamoDb  
dotnet new stacks-cqrs -n Cqrs.Sns -do Menu --cloudProvider AWS -e AwsSns
dotnet new stacks-cqrs -n Cqrs.EventHub -do Menu --cloudProvider GCP --cicdProvider GHA -db DynamoDb -e EventHub

# Test the Simple.Api project
cd $TEST_TEMPLATES_DIRECTORY/Simple.Api/src/simple-api/src/api
dotnet restore
dotnet build
dotnet test

# Test the Cqrs project
cd $TEST_TEMPLATES_DIRECTORY/Cqrs/src/cqrs/src/api
dotnet restore
dotnet build
dotnet test

# Test the Cosmos.Worker project
cd $TEST_TEMPLATES_DIRECTORY/Cosmos.Worker/src/func-cosmosdb-worker/src/functions
dotnet restore
dotnet build
dotnet test

# Test the EventHub.Listener project
cd $TEST_TEMPLATES_DIRECTORY/EventHub.Listener/func-aeh-listener/src/functions
dotnet restore
dotnet build
dotnet test

# Test the ServiceBus.Listener project
cd $TEST_TEMPLATES_DIRECTORY/ServiceBus.Listener/func-asb-listener/src/functions
dotnet restore
dotnet build
dotnet test

# Test the ServiceBus.Worker project
cd $TEST_TEMPLATES_DIRECTORY/ServiceBus.Worker/background-worker/src/worker
dotnet restore
dotnet build
dotnet test

# Test the Cqrs.AllTheThings project
cd $TEST_TEMPLATES_DIRECTORY/Cqrs.AllTheThings/src/cqrs/src/api
dotnet restore
dotnet build
dotnet test

# Test the Cqrs.ServiceBus project
cd $TEST_TEMPLATES_DIRECTORY/Cqrs.ServiceBus/src/cqrs/src/api
dotnet restore
dotnet build
dotnet test

# Test the Cqrs.Dynamo project
cd $TEST_TEMPLATES_DIRECTORY/Cqrs.Dynamo/src/cqrs/src/api
dotnet restore
dotnet build
dotnet test

# Test the Cqrs.Sns project
cd $TEST_TEMPLATES_DIRECTORY/Cqrs.Sns/src/cqrs/src/api
dotnet restore
dotnet build
dotnet test

# Print success message
echo " _________ _______  _______ _________ _______    _______  _______  _______  _______  _______  ______  "
echo " \__   __/(  ____ \(  ____ \\__   __/(  ____ \  (  ____ )(  ___  )(  ____ \(  ____ \(  ____ \(  __  \ "
echo "    ) (   | (    \/| (    \/   ) (   | (    \/  | (    )|| (   ) || (    \/| (    \/| (    \/| (  \  )"
echo "    | |   | (__    | (_____    | |   | (_____   | (____)|| (___) || (_____ | (_____ | (__    | |   ) |"
echo "    | |   |  __)   (_____  )   | |   (_____  )  |  _____)|  ___  |(_____  )(_____  )|  __)   | |   | |"
echo "    | |   | (            ) |   | |         ) |  | (      | (   ) |      ) |      ) || (      | |   ) |"
echo "    | |   | (____/\/\____) |   | |   /\____) |  | )      | )   ( |/\____) |/\____) || (____/\| (__/  )"
echo "    )_(   (_______/\_______)   )_(   \_______)  |/       |/     \|\_______)\_______)(_______/(______/ "

# Exit script
exit 0

# Error handling
trap 'echo "An error occurred. Exiting..."; exit 1;' ERR
