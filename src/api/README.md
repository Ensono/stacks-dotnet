# PROJECT_NAME 

Sample API project in netcore

current: 
 - dotnet core 3.1

<!-- ... more stuff here ... -->

USAGE:
---
Creates a sample CRUD for CosmosDB
<!-- TODO: flesh this out -->

Locally you may follow the below run instructions to get up and running, if you used the `amido-scaffolding-cli` follow instructions [here](https://amido.github.io/stacks/docs/getting_started_demo) for any advanced setup you may require.

RUN Locally 
---

Pre-requisites for running locally are:
  - COSMOSDB_KEY - primary key to use in authentication against the CosmosDB
  - appsettings.Development.json - you will need to place your own CosmosDBURI
    - "DatabaseAccountUri": "https://EXCHANGEMEFORCOSMOS.documents.azure.com:443/"


Once obtained
```bash
cd src/api
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

