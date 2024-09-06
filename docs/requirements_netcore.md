------------------------------------------------------------------------

id: requirements\_netcore
title: Requirements
linkTitle: Requirements
weight: 2
keywords:
- .net core
- rest api
- cqrs
- example
- template
- azure
- application insights
- cosmos db
- build
- run
- application
- configure
- docker
- tests
- github
- scaffolding
- cli
---

Windows

### Requirements for running the API locally

#### Mandatory

-   [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) and Runtime .0.\* or superior (for .NET 8 templates)

-   [CosmosDB Emulator 2.4.5+](https://aka.ms/cosmosdb-emulator)

#### Optional

-   [Visual Studio 2022](https://visualstudio.microsoft.com/vs/)

-   [Visual Studio Code](https://code.visualstudio.com/) 1.35+ with C# Extension from Microsoft (C# for Visual Studio Code (powered by OmniSharp))

### Additional requirements for running the API in docker containers

#### Mandatory

-   [Docker Desktop(for Windows)](https://desktop.docker.com/win/stable/Docker%20Desktop%20Installer.exe): Version 2.1.0.1 (37199) or superior

    -   Enable:

        -   Kubernetes 1.14+

        -   Linux container (Not windows containers)

    -   Docker Engine 19.03.1+ (provided with docker)

    -   WSL (Windows Subsystem for Linux: recommended v1, v2 is still in preview and has not been tested) (provided with docker)

        -   For running build, test and deployment scripts targeting Linux environment

    -   kubectl v1.14+ (provided with docker)

        -   Also [downloaded from k8s.io](https://kubernetes.io/docs/tasks/tools/install-kubectl/#install-kubectl-on-windows)

### Additional requirements for running the API in a kubernetes cluster

-   NGINX ingress controller

    -   Install the ingress controller in you local cluster.

    -   Make sure you follow the process for Bare Metal deployment described here.

macOS

### Requirements for running the API locally

-   homebrew

-   azure-cli: brew install azure-cli

-   [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) 8.0.\* or superior: brew cask install dotnet-sdk

-   [CosmosDB Emulator 2.4.5+](https://aka.ms/cosmosdb-emulator): Currently the Cosmos emulator can only be run on Windows. If you have an Azure subscription, you are able to use the Azure version instead

### Additional requirements for running the API in docker containers

-   [Docker Desktop for Mac](https://desktop.docker.com/mac/stable/Docker.dmg)

-   kubectl: docker run --name kubectl bitnami/kubectl:latest

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
<td class="content">The current version of Ensono Stacks are templates for .NET 8 (Current LTS, recommended).</td>
</tr>
</tbody>
</table>
