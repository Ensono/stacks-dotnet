------------------------------------------------------------------------

id: build\_and\_run\_project\_netcore
title: Build & Run REST API with CQRS
linkTitle: Build & Run REST API with CQRS
weight: 3
keywords:
- .net core
- rest api
- cqrs
- azure
- application insights
- cosmos db
- aws sns
- build
- run
- application
---

The API generated consists of configuration to be run locally or on a docker container.

Build and run locally on Windows

Move to the `<PROJECT-NAME>/cqrs/src/api` folder and run the next commands in **Command Prompt** or **Powershell**

    dotnet build

    # Note that the template engine will rename your paths, so change the command accordingly
    dotnet run --project xxENSONOxx.xxSTACKSxx.API/xxENSONOxx.xxSTACKSxx.API.csproj

<table>
<colgroup>
<col style="width: 50%" />
<col style="width: 50%" />
</colgroup>
<tbody>
<tr class="odd">
<td class="icon"><div class="title">
Note
</div></td>
<td class="content">Potential issue on some Windows installations
Depending on how deep your folder structure is you might encounter a problem where you cannot build the project. This happens because of our dependency on <a href="https://docs.pact.io/">Pact</a> for our contract tests.</td>
</tr>
</tbody>
</table>

The error looks something like this

    Error MSB3491 Path: File exceeds the OS max path limit. The fully qualified file name must be less than 260 characters.

There are two fixes possible so far:

-   You must enable long file paths on Windows by following the instructions [here](https://docs.microsoft.com/en-us/windows/win32/fileio/maximum-file-path-limitation?tabs=powershell#enable-long-paths-in-windows-10-version-1607-and-later)

-   Create your folder on an upper level where the paths won’t exceed 260 characters

Build and run locally on Linux

Move to the `<PROJECT-NAME>/cqrs/src/api` folder and run the next commands in **terminal**.

    export COSMOSDB_KEY=<COSMOSDB_KEY value here>
    export SERVICEBUS_CONNECTIONSTRING=<Your Service Bus connection string here>
    export EVENTHUB_CONNECTIONSTRING=<Your Event Hub connection string here>
    export STORAGE_CONNECTIONSTRING=<Your Event Hub storage connection string here>

    dotnet build

    # Note that the template engine will rename your paths, so change the command accordingly
    dotnet run --project xxENSONOxx.xxSTACKSxx.API/xxENSONOxx.xxSTACKSxx.API.csproj

Build and run in docker container

From the `<PROJECT-NAME>/cqrs/src/api` folder, build a Docker image using e.g. the command below:

Build docker image

    docker build -t dotnet-api .

This uses the **Dockerfile** in this folder to generate the Docker image.

After the creation of the Docker image, the Docker container can be run using the command below:

Run docker container

    docker run -p 5000:80 --mount type=bind,source=/path/to/PROJECT-NAME/cqrs/src/api/xxENSONOxx.xxSTACKSxx.API/appsettings.json,target=/app/config/appsettings.json -e COSMOSDB_KEY=your-key -e SERVICEBUS_CONNECTIONSTRING=your-asb-connection-string -e EVENTHUB_CONNECTIONSTRING=your-aeh-connection-string -e STORAGE_CONNECTIONSTRING=your-aeh-storage-connection-string  dotnet-api:latest`

<table>
<colgroup>
<col style="width: 50%" />
<col style="width: 50%" />
</colgroup>
<tbody>
<tr class="odd">
<td class="icon"><div class="title">
Note
</div></td>
<td class="content">The <strong>COSMOSDB_KEY</strong> described in the command above has to be passed when running the container. <strong>SERVICEBUS_CONNECTIONSTRING</strong> OR <strong>EVENTHUB_CONNECTIONSTRING</strong> and <strong>STORAGE_CONNECTIONSTRING</strong> are needed based on the configuration and message service you’ll be using. Note that the <strong>appsettings.json</strong> value is mounted here for running locally,
but not if the full project is deployed to Azure, where the build process will perform the substitution.</td>
</tr>
</tbody>
</table>

Verify that the application has started

<table>
<colgroup>
<col style="width: 50%" />
<col style="width: 50%" />
</colgroup>
<tbody>
<tr class="odd">
<td class="icon"><div class="title">
Note
</div></td>
<td class="content"><div class="title">
Relationship between domain and path
</div>
<div class="paragraph">
<p>Keep in mind that if you’ve changed the domain (original being <code>Menu</code>), the path will reflect that. If your domain is <code>Foo</code>. Then the path will be <code>../v1/foo</code> instead of <code>..v1/menu</code> etc.</p>
</div></td>
</tr>
</tbody>
</table>

Browse to <http://localhost:5000/v1/menu>. This should return a valid JSON response.

The application configuration uses Swagger/OAS3 to represent the API endpoints. The Swagger UI can be viewed by directing your
browser to <http://localhost:5000/swagger/index.html>.
