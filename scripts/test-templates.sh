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
cd ../../cqrs/src/api

dotnet restore
dotnet test

# Change directory to the root to generate the templates
cd ../../../..

dotnet new --uninstall .
dotnet new --install .

# Change directory create the test-templates directory
cd ..
mkdir test-templates
cd test-templates

# Generate the base templates
dotnet new stacks-webapi -n Simple.WebAPI -do Menu
dotnet new stacks-cqrs-app -n CQRS.Project -do Menu
dotnet new stacks-az-func-cosmosdb-worker -n Cosmos.Worker.Project
dotnet new stacks-az-func-aeh-listener -n EventHub.Listener.Project -do Menu
dotnet new stacks-az-func-asb-listener -n ServiceBus.Listener.Project -do Menu
dotnet new stacks-asb-worker -n ServiceBus.Worker.Project -do Menu

# Generate the additional template
dotnet new stacks-webapi -n CQRS.Added.Project -do Menu
cd CQRS.Added.Project/simple-api/src/api
dotnet new stacks-add-cqrs -n CQRS.Project -do Menu

# Test each generated projects unit tests
cd ../../../..

# Test the Simple.WebAPI project
cd Simple.WebAPI/simple-api/src/api
dotnet restore
dotnet test
cd ../../../..

# Test the CQRS.Project project
cd CQRS.Project/cqrs/src/api
dotnet restore
dotnet test
cd ../../../..

# Test the Cosmos.Worker.Project project
cd Cosmos.Worker.Project/func-cosmosdb-worker/src/functions
dotnet restore
dotnet test
cd ../../../..

# Test the EventHub.Listener.Project project
cd EventHub.Listener.Project/func-aeh-listener/src/functions
dotnet restore
dotnet test
cd ../../../..

# Test the ServiceBus.Listener.Project project
cd ServiceBus.Listener.Project/func-asb-listener/src/functions
dotnet restore
dotnet test
cd ../../../..

# Test the ServiceBus.Worker.Project project
cd ServiceBus.Worker.Project/background-worker/src/worker
dotnet restore
dotnet test
cd ../../../..

# Test the CQRS.Added.Project project
cd CQRS.Added.Project/simple-api/src/api
dotnet restore
dotnet test
cd ../../..

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