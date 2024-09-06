------------------------------------------------------------------------

id: infrastructure\_netcore
title: .NET Core - Azure Infrastructure
linkTitle: Infrastructure
weight: 6
keywords:
- .net core
- rest api
- cqrs
- pipeline
- azure
- azure container registry
- azure kubernetes service
- application insights
- cosmos db
- deployment
---

## Overview

This page presents the high-level design of the reference implementation of the .NET Core Ensono Stacks REST API with CQRS.

**This page assumes that the core infrastructure has already been provisioned. Instructions and additional information on the core infrastructure can be found [here](../../../../infrastructure/azure/core_infrastructure.md).**

Both the diagram and resource list below are for a single environment.

![Azure .NET API Infrastructure](../../../../../images/azure_rest_api_infrastructure.png)

### Resource List

<table class="tableblock frame-all grid-all stretch">
<colgroup>
<col style="width: 50%" />
<col style="width: 50%" />
</colgroup>
<thead>
<tr class="header">
<th class="tableblock halign-left valign-top">Resource</th>
<th class="tableblock halign-left valign-top">Description</th>
</tr>
</thead>
<tbody>
<tr class="odd">
<td class="tableblock halign-left valign-top"><p>Resource Group - Core*</p></td>
<td class="tableblock halign-left valign-top"><p>Used to logically group core infrastructure</p></td>
</tr>
<tr class="even">
<td class="tableblock halign-left valign-top"><p>AKS*</p></td>
<td class="tableblock halign-left valign-top"><p>Azure Kubernetes Service</p></td>
</tr>
<tr class="odd">
<td class="tableblock halign-left valign-top"><p>Public DNS Record</p></td>
<td class="tableblock halign-left valign-top"><p>DNS record pointing Ingress (via application gateway* and internal load balancer*)</p></td>
</tr>
<tr class="even">
<td class="tableblock halign-left valign-top"><p>ACR*</p></td>
<td class="tableblock halign-left valign-top"><p>Azure Container Registry</p></td>
</tr>
<tr class="odd">
<td class="tableblock halign-left valign-top"><p>Namespace: {env}-api</p></td>
<td class="tableblock halign-left valign-top"><p>Kubernetes namespace for the environment</p></td>
</tr>
<tr class="even">
<td class="tableblock halign-left valign-top"><p>Ingress</p></td>
<td class="tableblock halign-left valign-top"><p>Kubernetes Ingress to handle routing to Service</p></td>
</tr>
<tr class="odd">
<td class="tableblock halign-left valign-top"><p>Service</p></td>
<td class="tableblock halign-left valign-top"><p>Kubernetes Service to handle routing to Deployment Pods</p></td>
</tr>
<tr class="even">
<td class="tableblock halign-left valign-top"><p>Deployment</p></td>
<td class="tableblock halign-left valign-top"><p>Kubernetes Deployment for managing Pods</p></td>
</tr>
<tr class="odd">
<td class="tableblock halign-left valign-top"><p>Resource Group - API</p></td>
<td class="tableblock halign-left valign-top"><p>Used to logically group API-specific infrastructure</p></td>
</tr>
<tr class="even">
<td class="tableblock halign-left valign-top"><p>Cosmos DB</p></td>
<td class="tableblock halign-left valign-top"><p>Azure managed NoSQL database</p></td>
</tr>
</tbody>
</table>

\* **Resource is created by the core infrastructure deployment.**

## Deploying

### Pipelines

The following pipelines are currently supported for automating the deployment:

-   [Azure DevOps](./pipeline_netcore.md)

### Running Locally

Currently, `vars.tf` and provider configuration is not automatically updated. Future iterations will include this.

The safest way to run and maintain this locally is to rely on Docker and environment variables as that is the way the pipeline will trigger the executions of Terraform.

Sample commands with example environment vars:

    # Navigate to the infra directory
    cd ${YOUR_DIRECTORY_PATH}/deploy/azure/app

    # Run Ensono Terraform Docker container
    docker run -v $(pwd):/usr/data --rm -it amidostacks/ci-tf:0.0.4 /bin/bash

    ###########################################################################
    # All commands from this point should be executed in the Docker container #
    ###########################################################################

    # Navigate to /usr/data directory
    cd /usr/data

    # Export Azure Credentials. Replace the example values.
    export ARM_CLIENT_ID=1111-2222-3333-444 \
    ARM_CLIENT_SECRET=1111-2222-3333-4444 \
    ARM_SUBSCRIPTION_ID=1111-2222-3333-444 \
    ARM_TENANT_ID=1111-2222-3333-444

    # Export Terraform variables. Replace the example values.
    export TF_VAR_name_company=ensono \
    TF_VAR_name_project=example \
    TF_VAR_name_domain=menu-api \
    TF_VAR_name_environment=dev \
    TF_VAR_core_resource_group=ensono-example-nonprod-uks-core \
    TF_VAR_resource_group_location=uksouth \
    TF_VAR_app_gateway_frontend_ip_name=ensono-example-nonprod-uks-core \
    TF_VAR_create_dns_record=true \
    TF_VAR_dns_zone_name=nonprod.ensonostacks.com \
    TF_VAR_dns_record=dev-menu-api \
    TF_VAR_internal_dns_zone_name=nonprod.ensonostacks.internal \
    TF_VAR_create_cosmosdb=true \
    TF_VAR_cosmosdb_sql_container=Menu \
    TF_VAR_cosmosdb_sql_container_partition_key=/id \
    TF_VAR_cosmosdb_kind=GlobalDocumentDB \
    TF_VAR_cosmosdb_offer_type=Standard \
    TF_VAR_app_insights_name=ensono-example-nonprod-uks-core

    # Initial Terraform. Replace the example values.
    terraform init \
    -backend-config="resource_group_name=ensono-stacks-terraform" \
    -backend-config="storage_account_name=ensonostacksterraform" \
    -backend-config="container_name=tfstate" \
    -backend-config="key=example-menu-api"

    # Select or create the "dev" workspace.
    terraform workspace select dev || terraform workspace new dev

    # Check the plan matches your expected changes.
    terraform plan
