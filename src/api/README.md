# PROJECT_NAME 

Sample API project in netcore

current: 
 - dotnet core 3.1

<!-- ... more stuff here ... -->

USAGE:
---
Creates a sample CRUD for CosmosDB
<!-- TODO: flesh this out -->

Locally you may follow the below run instructions, from a CI/CD solution you will need to ensure the `STACKS_NUGET` and `STACKS_NUGET_TOKEN` are injected into the environment.

RUN Locally 
---

Pre-requisites for running locally are:

  - STACKS_NUGET for Amido Stacks nuget repo = `https://amido-dev.pkgs.visualstudio.com/_packaging/Stacks/nuget/v3/index.json`
  - STACKS_NUGET_TOKEN - PAT token you need to obtain from one of the team
  - COSMOSDB_KEY - primary key to use in authentication against the CosmosDB
  - appsettings.Development.json - you will need to place your own CosmosDBURI

        "DatabaseAccountUri": "https://EXCHANGEMEFORCOSMOS.documents.azure.com:443/"


Once obtained
```bash
cd src/api
export STACKS_NUGET=https://amido-dev.pkgs.visualstudio.com/_packaging/Stacks/nuget/v3/index.json
export STACKS_NUGET_TOKEN=${PRIVATE_PKG_REPO_TOKEN} 
export COSMOSDB_KEY=${COSMOSDB_KEY}

dotnet restore
dotnet clean
dotnet build
dotnet run --project xxAMIDOxx.xxSTACKSxx.API/xxAMIDOxx.xxSTACKSxx.API.csproj
```

docker build and run locally
```bash
docker build --build-arg nuget_url=${STACKS_NUGET} \
--build-arg nuget_token=${STACKS_NUGET_TOKEN} -t dotnet-api .
docker run -p 5000:80 dotnet-api:latest
```

