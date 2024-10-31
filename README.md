# stacks-dotnet

The full documentation on Ensono Stacks can be found [here](https://amido.github.io/stacks/).

Ensono Stacks targets different cloud providers.

[Azure](https://amido.github.io/stacks/docs/workloads/azure/backend/netcore/introduction_netcore)

## Folders of interest in this repository

```shell
stacks-dotnet
|   README.md
└─── src
|   └─── background-worker
|   |     └─── src
|   |         └─── worker
|   |              └─── xxENSONOxx.xxStacksxx.BackgroundWorker
|   └─── cqrs
|   |     └─── src
|   |         └─── api
|   |         |    └─── xxENSONOxx.xxStacksxx.API
|   |         |    └─── other API related projects
|   |         └─── tests
|   └─── func-aeh-listener
|   |     └─── src
|   |         └─── functions
|   |             └─── xxENSONOxx.xxStacksxx.Listener
|   └─── func-asb-listener
|   |     └─── src
|   |         └─── functions
|   |             └─── xxENSONOxx.xxStacksxx.Listener
|   └─── func-cosmosdb-worker
|   |     └─── src
|   |         └─── functions
|   |             └─── xxENSONOxx.xxStacksxx.Worker
|   └─── shared
|   └─── simple-api
|   |     └─── src
|   |         └─── api
|   |             └─── xxENSONOxx.xxStacksxx.API
|   |             └─── other API related projects
```

- The `background-worker` folder contains a background worker that listens to all event types from the ASB topic and shows example handlers for them and the use of the [Ensono.Stacks.Messaging.Azure.ServiceBus](https://github.com/amido/stacks-dotnet-packages-messaging-asb) package.
- The `cqrs` folder contains everything related to a CQRS API and is a standalone executable
- `func-aeh-listener` is an Azure Event Hub trigger that listens for `MenuCreatedEvent`
- `func-asb-listener` is an Azure Service Bus subscription (filtered) trigger that listens for `MenuCreatedEvent`
- `func-cosmosdb-worker` is a CosmosDB change feed trigger function that publishes a `CosmosDbChangeFeedEvent` when a new entity has been added or was changed to CosmosDB
- The `shared` folder contains shared code that is used across multiple projects
- The `simple-api` folder contains everything related to a basic API and is a standalone executable

The functions and workers are all stand-alone implementations that can be used together or separately in different projects.

### Templates

All templates from this repository come as part of the [Stacks Dotnet NuGet package](https://www.nuget.org/packages/Ensono.Stacks.Templates/). The list of templates inside the package are as follows:

- `stacks-webapi`. The full Web API template including source + build infrastructure.
- `stacks-cqrs-app`. The full CQRS template including source + build infrastructure.
- `stacks-asb-worker`. This template contains a background worker application that reads and handles messages from a ServiceBus subscription.
- `stacks-az-func-asb-listener`. Template containing an Azure Function project with a single function that has a Service Bus subscription trigger. The function receives the message and deserializes it.
- `stacks-az-func-aeh-listener`. Template containing an Azure Function project with a single function that has a Event Hub trigger. The function receives the message and deserializes it.
- `stacks-az-func-cosmosdb-worker`. Azure Function containing a CosmosDb change feed trigger. Upon a CosmosDb event, the worker reads it and publishes a message to Service Bus.

### Generating the expected directory trees

The template outputs are validated during testing using the directory tree files in the `scripts/expected-trees` directory. When files are added or removed from the template the expected directory trees will need to be updated. This can be done using the `test-templates.ps1` script with the `-GenerateExpectedTrees` parameter (`pwsh test-templates.ps1 -GenerateExpectedTrees`)

### Template usage

#### Template installation

For the latest template version, please consult the Nuget page [Ensono.Stacks.Templates](https://www.nuget.org/packages/Ensono.Stacks.Templates/). To install the latest version of the templates to your machine via the command line:

```shell
dotnet new --install Ensono.Stacks.Templates
```

The output you'll see will list all installed templates (not listed for brevity). In that list you'll see the just installed Ensono Stacks template `stacks-webapi`

| Template Name | Short Name                     | Language | Tags |
|---------------|--------------------------------|----------|------|
| Ensono Stacks Web API  | stacks-webapi                  | [C#] | Stacks/Infrastructure/WebAPI |
| Ensono Stacks CQRS Web API | stacks-cqrs-app                | [C#] | Stacks/Infrastructure/CQRS/WebAPI |
| Ensono Stacks Azure Function CosmosDb Worker | stacks-az-func-cosmosdb-worker | [C#] | Stacks/Azure Function/CosmosDb/Worker |
| Ensono Stacks Azure Function Service Bus Trigger | stacks-az-func-asb-listener    | [C#] | Stacks/Azure Function/Service Bus/Listener |
| Ensono Stacks Azure Function Event Hub Trigger | stacks-az-func-aeh-listener    | [C#] | Stacks/Azure Function/Event Hub/Listener |
| Ensono Stacks Service Bus Worker | stacks-asb-worker              | [C#] | Stacks/Service Bus/Worker |

```shell
Examples:
    dotnet new mvc --auth Individual
    dotnet new react --auth Individual
    dotnet new --help
    dotnet new stacks-az-func-asb-listener --help
```
#### Uninstalling a template

To uninstall the template pack you have to execute the following command

```shell
dotnet new --uninstall Ensono.Stacks.Templates
```

#### Important parameters

- **-n|--name**
  - Sets the project name
    - Omitting it will result in the project name being the same as the folder where the command has been ran from
- **-do|--domain**
  - Sets the name of the aggregate root object. It is also the name of the collection within CosmosDB instance.
- **-o|--output**
  - Sets the path to where the project is added
  - Omitting the parameter will result in the creation of a new folder
- **-cp|--cloudProvider**
  - Configures which cloud provider to be used
- **-cicd|--cicdProvider**
  - Configures which cicd provider templates to be used
- **-dm|--deploymentMode**
  - Allow you to deploy the container workload to ACA or AKS. The default is AKS. Only supported for Azure cloud provider

#### Creating a new WebAPI project with the template

Let's say you want to create a brand new WebAPI for your project.

It's entirely up to you where you want to generate the WebAPI. For example your company has the name structure `Foo.Bar` as a prefix to all your namespaces where `Foo` is the company name and `Bar` is the name of the project. If you want the WebAPI to be generated inside a folder called `Foo.Bar` you'll do the following:

```shell
% cd your-repo-folder

% dotnet new stacks-webapi -n Foo.Bar -do Menu
The template "Ensono Stacks Web Api" was created successfully.
```

Inspecting the folder will yield the following

```shell
% ls -la
total 0
drwxr-xr-x  3 ensono  staff   96 23 Aug 15:51 .
drwxr-xr-x  9 ensono  staff  288 16 Aug 14:06 ..
drwxr-xr-x  6 ensono  staff  192 23 Aug 15:51 Foo.Bar

% ls -la Foo.Bar
total 16
drwxr-xr-x  6 ensono  staff   192 27 Aug 15:51 .
drwxr-xr-x  3 ensono  staff    96 27 Aug 15:51 ..
-rw-r--r--  1 ensono  staff  1062 27 Aug 14:59 LICENSE
-rw-r--r--  1 ensono  staff   258 27 Aug 14:59 README.md
drwxr-xr-x  3 ensono  staff    96 27 Aug 14:59 build
drwxr-xr-x  4 ensono  staff   128 27 Aug 14:59 contracts
drwxr-xr-x  5 ensono  staff   160 27 Aug 14:59 deploy
drwxr-xr-x  4 ensono  staff   128 27 Aug 14:59 src
-rw-r--r--  1 ensono  staff   292 27 Aug 14:59 yamllint.confn
```

The `Foo.Bar` namespace prefix will be added to the class names and is reflected not only in folder/file names, but inside the codebase as well.

To generate the template with your own namespace, but in a different folder you'll have to pass the `-o` flag with your desired path/folder name

```shell
% dotnet new stacks-webapi -n Foo.Bar -d Car -o web-api-dir
The template "Ensono Stacks Web Api" was created successfully.

% ls -la
total 0
drwxr-xr-x  3 ensono  staff   96 23 Aug 15:58 .
drwxr-xr-x  9 ensono  staff  288 16 Aug 14:06 ..
drwxr-xr-x  6 ensono  staff  192 23 Aug 15:58 web-api-dir
```

Now you can build the solution located in the `web-api/src` folder and run/deploy it.

## Creating a new WebAPI + CQRS project from the template

Let's say you want to create a brand new WebAPI with CQRS for your project.

It's entirely up to you where you want to generate the WebAPI. For example your company has the name structure `Foo.Bar` as a prefix to all your namespaces where `Foo` is the company name and `Bar` is the name of the project. If you want the WebAPI to have a domain `Warehouse`, use `CosmosDb` and be generated inside a folder called `new-proj-folder` you'll execute the following command:

Deployed to an AKS cluster:
```shell
% dotnet new stacks-cqrs-app -n Foo.Bar -do Warehouse -db CosmosDb -o new-proj-folder --deploymentMode AKS
The template "Ensono Stacks Web Api" was created successfully.
```

Deployed to an ACA environment:
```shell
% dotnet new stacks-cqrs-app -n Foo.Bar -do Warehouse -db CosmosDb -o new-proj-folder --deploymentMode ACA
The template "Ensono Stacks Web Api" was created successfully.
```

## Creating a new WebAPI with CQRS event sourcing

Let's say you want to create a brand new WebAPI with CQRS and event sourcing for your project.

It's entirely up to you where you want to generate the WebAPI. For example your company has the name structure `Foo.Bar` as a prefix to all your namespaces where `Foo` is the company name and `Bar` is the name of the project. If you want the WebAPI to have a domain `Warehouse`, use `CosmosDb`, publish events to `ServiceBus` and be generated inside a folder called `new-proj-folder` you'll execute the following command:

Deployed to an AKS cluster:
```shell
% dotnet new stacks-cqrs-app -n Foo.Bar -do Warehouse -db CosmosDb -e ServiceBus -o new-proj-folder --deploymentMode AKS
The template "Ensono Stacks CQRS Web API" was created successfully.
```
Deployed to an ACA environment:
```shell
% dotnet new stacks-cqrs-app -n Foo.Bar -do Warehouse -db CosmosDb -e ServiceBus -o new-proj-folder --deploymentMode ACA
The template "Ensono Stacks CQRS Web API" was created successfully.
```

Alternatively, if you wanted to generate the WebAPI with structure `Bar.Baz` as a prefix to all your namespaces where `Bar` is the company name and `Baz` is the name of the project. And you want the WebAPI to have a domain `Warehouse`, use `DynamoDb`, publish events to `AwsSns` and be generated inside a folder called `new-proj-folder` you'll execute the following command:

```shell
% dotnet new stacks-cqrs-app -n Foo.Bar -do Warehouse -db DynamoDb -e AwsSns -o new-proj-folder
The template "Ensono Stacks CQRS Web API" was created successfully.
```

## Adding a function template to your project

Let's say you want to add either `stacks-az-func-cosmosdb-worker` or `stacks-az-func-asb-listener` function apps to your solution or project.

It's entirely up to you where you want to generate the function project. For example your project has the name structure `Foo.Bar` as a prefix to all your namespaces. If you want the function project to be generated inside a folder called `Foo.Bar` you'll do the following:

```shell
% cd functions

% dotnet new stacks-az-func-cosmosdb-worker -n Foo.Bar -do Menu
The template "Ensono Stacks Azure Function CosmosDb Worker" was created successfully.

% ls -la
total 0
drwxr-xr-x  3 ensono staff   96 23 Aug 15:51 .
drwxr-xr-x  9 ensono  staff  288 16 Aug 14:06 ..
drwxr-xr-x  6 ensono  staff  192 23 Aug 15:51 Foo.Bar

% ls -la Foo.Bar
total 16
drwxr-xr-x  6 ensono  staff   192 23 Aug 15:51 .
drwxr-xr-x  3 ensono  staff    96 23 Aug 15:51 ..
-rw-r--r--  1 ensono  staff   639 23 Aug 15:51 Dockerfile
drwxr-xr-x  9 ensono  staff   288 23 Aug 15:51 Foo.Bar.Worker
drwxr-xr-x  4 ensono  staff   128 23 Aug 15:51 Foo.Bar.Worker.UnitTests
-rw-r--r--  1 ensono  staff  1643 23 Aug 15:51 Foo.Bar.Worker.sln
```

As you can see your `Foo.Bar` namespace prefix got added to the class names and is reflected not only in the filename, but inside the codebase as well.

To generate the template with your own namespace, but in a different folder you'll have to pass the `-o` flag with your desired path.

```shell
% dotnet new stacks-az-func-cosmosdb-worker -n Foo.Bar -o cosmosdb-worker
The template "Ensono Stacks Azure Function CosmosDb Worker" was created successfully.

% ls -la
total 0
drwxr-xr-x  3 ensono  staff   96 23 Aug 15:58 .
drwxr-xr-x  9 ensono  staff  288 16 Aug 14:06 ..
drwxr-xr-x  6 ensono  staff  192 23 Aug 15:58 cosmosdb-worker

% ls -la cosmosdb-worker
total 16
drwxr-xr-x  6 ensono  staff   192 23 Aug 15:58 .
drwxr-xr-x  3 ensono  staff    96 23 Aug 15:58 ..
-rw-r--r--  1 ensono  staff   639 23 Aug 15:58 Dockerfile
drwxr-xr-x  9 ensono  staff   288 23 Aug 15:58 Foo.Bar.Worker
drwxr-xr-x  4 ensono  staff   128 23 Aug 15:58 Foo.Bar.Worker.UnitTests
-rw-r--r--  1 ensono  staff  1643 23 Aug 15:58 Foo.Bar.Worker.sln
```

Now you can build the solution and run/deploy it. If you want to add the existing projects to your own solution you can go to the folder where your `.sln` file lives and execute the following commands

```shell
% cd my-proj-folder

% ls -la
total 16
drwxr-xr-x  6 ensono  staff   192 23 Aug 15:58 .
drwxr-xr-x  3 ensono  staff    96 23 Aug 15:58 ..
-rw-r--r--  1 ensono  staff   639 23 Aug 15:58 src
-rw-r--r--  1 ensono  staff  1643 23 Aug 15:58 Foo.Bar.sln

% dotnet sln add path/to/function/Foo.Bar.Worker
% dotnet sln add path/to/function/Foo.Bar.Worker.UnitTests
```

Now both `Foo.Bar.Worker` and `Foo.Bar.Worker.UnitTests` projects are part of your `Foo.Bar` solution.

## DynamoDB Setup

You need a DynamoDB instance in order to use this library. You can follow the official instructions provided by AWS [here](https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/SettingUp.DynamoWebService.html).

It should be noted that the object(s) from your application that you want to store in DynamoDB have to conform to the [Object Persistence Model](https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/DotNetSDKHighLevel.html).

Relevant documentation pages that you can follow to set up your profile:

- [Configuration and credential file settings](https://docs.aws.amazon.com/cli/latest/userguide/cli-configure-files.html)

- [Named profiles](https://docs.aws.amazon.com/cli/latest/userguide/cli-configure-profiles.html)

This library assumes you'll use the `AWS CLI` tools and will have configured your access keys via the `aws configure` command.

## Running the API locally on MacOS

To run the API locally on MacOS there are a couple of prerequisites that you have to be aware of. You'll need a CosmosDB emulator/instance or an instance of DynamoDB on AWS. You also might need access to Azure/AWS for Azure Service Bus, Azure Event Hubs or AWS SNS.

### Docker CosmosDB emulator setup

1. Get Docker from here - <https://docs.docker.com/docker-for-mac/install/>
2. Follow the instructions outlined here - <https://docs.microsoft.com/en-us/azure/cosmos-db/linux-emulator?tabs=ssl-netstd21>
3. From the CosmosDB UI create a database called `Stacks`.
4. Inside the `Stacks` database create a container called `Menu`

### Azure Service Bus

You'll need an Azure Service Bus namespace and a topic with subscriber in order to be able to publish application events.

### Azure Event Hub

You'll will need an Azure Event Hub namespace and an Event Hub to publish application events. You will also need a blob container storage account.

### AWS SNS

You'll need an AWS SNS Topic setup with a defined TopicArn in order to be able to publish application events.

### Configuring CosmosDb, ServiceBus, EventHub, DynamoDb or SNS

Now that you have your cloud services all set, you can point the API project to it. In `appsettings.json` you can see the following sections

```json
"CosmosDb": {
    "DatabaseAccountUri": "https://localhost:8081/",
    "DatabaseName": "Stacks",
    "SecurityKeySecret": {
        "Identifier": "COSMOSDB_KEY",
        "Source": "Environment"
    }
},
"ServiceBusConfiguration": {
    "Sender": {
        "Topics": [
            {
                "Name": "servicebus-topic-lius",
                "ConnectionStringSecret": {
                    "Identifier": "SERVICEBUS_CONNECTIONSTRING",
                    "Source": "Environment"
                }
            }
        ]
    }
},
"EventHubConfiguration": {
    "Publisher": {
        "NamespaceConnectionString": {
            "Identifier": "EVENTHUB_CONNECTIONSTRING",
            "Source": "Environment"
        },
        "EventHubName": "stacks-event-hub"
    },
    "Consumer": {
        "NamespaceConnectionString": {
            "Identifier": "EVENTHUB_CONNECTIONSTRING",
            "Source": "Environment"
        },
        "EventHubName": "stacks-event-hub",
        "BlobStorageConnectionString": {
            "Identifier": "STORAGE_CONNECTIONSTRING",
            "Source": "Environment"
        },
        "BlobContainerName": "stacks-blob-container-name"
    }
}
"DynamoDb": {
    "TableName": "<Domain Object>",
    "TablePrefix": "<Any Environmental Prefix You Would Like>"
}
"AwsSnsConfiguration": {
    "TopicArn": {
            "Identifier": "TOPIC_ARN",
            "Source": "Environment"
        }
}
"AWS": {
    "Region": "eu-west-2"
}
"logConfiguration": {
    "logDriver": "awslogs",
    "options": {
        "awslogs-group": "${cloudwatch_log_group_name}",
        "awslogs-region": "${region}",
        "awslogs-stream-prefix": "${cloudwatch_log_prefix}"
    }
}
```

The `SecurityKeySecret` and `ConnectionStringSecret` sections are needed because of our use of references to secrets in configuration. `COSMOSDB_KEY`, `SERVICEBUS_CONNECTIONSTRING`, `EVENTHUB_CONNECTIONSTRING` or `TOPIC_ARN` have to be set before you can run the project. If you want to debug the solution with VSCode you usually have a `launch.json` file. In that file there's an `env` section where you can put environment variables for the current session.

```json
"env": {
    "ASPNETCORE_ENVIRONMENT": "Development",
    "COSMOSDB_KEY": "YOUR_COSMOSDB_PRIMARY_KEY",
    "SERVICEBUS_CONNECTIONSTRING": "YOUR_SERVICE_BUS_CONNECTION_STRING",
    "EVENTHUB_CONNECTIONSTRING": "YOUR_EVENT_HUB_CONNECTION_STRING",
    "TOPIC_ARN": "YOUR_TOPIC_ARN"
}
```

Alternatively, if you would like to run locally, you could also set the following environment variables in your `local.settings.json` file

```json
{
    "IsEncrypted": false,
    "Values": {
        "ASPNETCORE_ENVIRONMENT": "Development",
        "COSMOSDB_KEY": "YOUR_COSMOSDB_PRIMARY_KEY",
        "SERVICEBUS_CONNECTIONSTRING": "YOUR_SERVICE_BUS_CONNECTION_STRING",
        "EVENTHUB_CONNECTIONSTRING": "YOUR_EVENT_HUB_CONNECTION_STRING",
        "TOPIC_ARN": "YOUR_TOPIC_ARN"
    }
}
```

If you want to run the application without VSCode you'll have to set the `COSMOSDB_KEY`, `SERVICEBUS_CONNECTIONSTRING`, `EVENTHUB_CONNECTIONSTRING` or `TOPIC_ARN` environment variables through your terminal.

```shell
export COSMOSDB_KEY=YOUR_COSMOSDB_PRIMARY_KEY
export SERVICEBUS_CONNECTIONSTRING=YOUR_SERVICE_BUS_CONNECTION_STRING
export EVENTHUB_CONNECTIONSTRING=YOUR_EVENT_HUB_CONNECTION_STRING
export TOPIC_ARN=YOUR_TOPIC_ARN
```

This will set the environment variables only for the current session of your terminal.

To set the environment variables permanently on your system you'll have to edit your `bash_profile` or `.zshenv` depending on which shell are you using.

```shell
# Example for setting env variable in .zchenv
echo 'export COSMOSDB_KEY=YOUR_COSMOSDB_PRIMARY_KEY' >> ~/.zshenv
echo 'export SERVICEBUS_CONNECTIONSTRING=YOUR_SERVICE_BUS_CONNECTION_STRING' >> ~/.zshenv
echo 'export EVENTHUB_CONNECTIONSTRING=YOUR_EVENT_HUB_CONNECTION_STRING' >> ~/.zshenv
echo 'export TOPIC_ARN=YOUR_TOPIC_ARN' >> ~/.zshenv
```

If you wan to run the application using Visual Studio, you will need to set the environment variables in the `launchSettings.json` file contained in the Properties folder of the solution.

```json
"environmentVariables": {
    "ASPNETCORE_ENVIRONMENT": "Development",
    "COSMOSDB_KEY": "",
    "SERVICEBUS_CONNECTIONSTRING": "",
    "EVENTHUB_CONNECTIONSTRING": "=",
    "STORAGE_CONNECTIONSTRING": "",
    "OTLP_SERVICENAME": "",
    "OTLP_ENDPOINT": "",
    "TOPIC_ARN": "",
}
```

### Running the Worker ChangeFeed listener locally

Running the Worker function locally is pretty straightforward. You'll have to set the following environment variables in your `local.settings.json` file

```json
{
    "IsEncrypted": false,
    "Values": {
        "AzureWebJobsStorage": "UseDevelopmentStorage=true",
        "FUNCTIONS_WORKER_RUNTIME": "dotnet",
        "SERVICEBUS_CONNECTIONSTRING": "SERVICE_BUS_CONNECTION_STRING",
        "DatabaseName": "Stacks",
        "CollectionName": "Menu",
        "LeaseCollectionName": "Leases",
        "CosmosDbConnectionString": "COSMOS_DB_CONNECTION_STRING",
        "CreateLeaseCollectionIfNotExists": true
    }
}
```
