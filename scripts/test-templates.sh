#!/bin/bash

# Create directory for the repository and testing
cd ../..
mkdir STACKS-TESTING
cd STACKS-TESTING

# Clone repository and checkout to the specified branch
git clone git@github.com:Ensono/stacks-dotnet.git
cd stacks-dotnet
git checkout $1

# Change directory to the simple-api project and run the tests
cd src/simple-api/src/api

dotnet restore
dotnet test

# Change directory to the cqrs project and run the tests
cd ../../../cqrs/src/api

dotnet restore
dotnet test

# Change directory to the root to generate the templates
cd ../../../..

dotnet new uninstall .
dotnet new install .

# Change directory create the test-templates directory
cd ..
mkdir test-templates
cd test-templates

# Generate the base templates
dotnet new stacks-webapi -n Simple.WebAPI -do Menu
dotnet new stacks-cqrs -n Cqrs -do Menu
dotnet new stacks-az-func-cosmosdb-worker -n Cosmos.Worker
dotnet new stacks-az-func-aeh-listener -n EventHub.Listener -do Menu
dotnet new stacks-az-func-asb-listener -n ServiceBus.Listener -do Menu
dotnet new stacks-asb-worker -n ServiceBus.Worker -do Menu

# Generate the additional template
dotnet new stacks-webapi -n NonCqrs -do Menu
cd NonCqrs/src/simple-api/src/api
dotnet new stacks-add-cqrs -n NonCqrs.Cqrs -do Menu

# Test the Simple.WebAPI project
cd Simple.WebAPI/src/simple-api/src/api
dotnet restore
dotnet test
cd ../../../../..

# Test the Cqrs project
cd Cqrs/src/cqrs/src/api
dotnet restore
dotnet test
cd ../../../../..

# Test the Cosmos.Worker project
cd Cosmos.Worker/src/func-cosmosdb-worker/src/functions
dotnet restore
dotnet test
cd ../../../..

# Test the EventHub.Listener project
cd EventHub.Listener/src/func-aeh-listener/src/functions
dotnet restore
dotnet test
cd ../../../..

# Test the ServiceBus.Listener project
cd ServiceBus.Listener/src/func-asb-listener/src/functions
dotnet restore
dotnet test
cd ../../../..

# Test the ServiceBus.Worker project
cd ServiceBus.Worker/src/background-worker/src/worker
dotnet restore
dotnet test
cd ../../../..

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
