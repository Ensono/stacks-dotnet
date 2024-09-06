------------------------------------------------------------------------

id: pipeline\_netcore
title: .NET Core - Azure DevOps Pipeline
linkTitle: Pipeline
weight: 7
keywords:
- .net core
- azure devops
- configure
- pipeline
- setting up
- template
- build
---

The pipeline will automate provisioning and updating the .NET Core REST API with CQRS infrastructure in Azure.

Where possible, we are creating reusable steps [stacks-pipeline-templates](https://github.com/Ensono/stacks-pipeline-templates) that can be pulled into any base pipeline. Reusable steps can include tasks to deploy, build, test and more.

![.NET Core REST API - Azure DevOps Pipeline](../../../../../images/azure_netcore_azure_devops_pipeline.png)

Figure 1. Pipeline Diagram

## Setting up Azure DevOps

### Variable group

A variable group will need creating for storing variables to be used for testing steps. Instructions for creating a variable group can be found [here](https://docs.microsoft.com/en-us/azure/devops/pipelines/library/variable-groups?view=azure-devops&tabs=classic#create-a-variable-group). Give the variable group a name and description and make sure the **Allow access to all pipelines** option is checked.

Add the following variables:

<table class="tableblock frame-all grid-all stretch">
<colgroup>
<col style="width: 33%" />
<col style="width: 33%" />
<col style="width: 33%" />
</colgroup>
<thead>
<tr class="header">
<th class="tableblock halign-left valign-top">Variable Name</th>
<th class="tableblock halign-left valign-top">Required for</th>
<th class="tableblock halign-left valign-top">Note</th>
</tr>
</thead>
<tbody>
<tr class="odd">
<td class="tableblock halign-left valign-top"><p>COSMOSDB_KEY_DEV</p></td>
<td class="tableblock halign-left valign-top"><p>Integration Test</p></td>
<td class="tableblock halign-left valign-top"><p>Output from infrastructure deployment. Should be enabled after first pipeline run</p></td>
</tr>
<tr class="even">
<td class="tableblock halign-left valign-top"><p>COSMOSDB_NAME_DEV</p></td>
<td class="tableblock halign-left valign-top"><p>Integration Test</p></td>
<td class="tableblock halign-left valign-top"><p>Output from infrastructure deployment. Should be enabled after first pipeline run</p></td>
</tr>
<tr class="odd">
<td class="tableblock halign-left valign-top"><p>COSMOSDB_ACCOUNT_URI_DEV</p></td>
<td class="tableblock halign-left valign-top"><p>Integration Test</p></td>
<td class="tableblock halign-left valign-top"><p>Output from infrastructure deployment. Should be enabled after first pipeline run</p></td>
</tr>
<tr class="even">
<td class="tableblock halign-left valign-top"><p>SONAR_ORGANIZATION</p></td>
<td class="tableblock halign-left valign-top"><p>Static Code Analysis</p></td>
<td class="tableblock halign-left valign-top"><p>from <a href="https://sonarcloud.io/">sonarcloud</a></p></td>
</tr>
<tr class="odd">
<td class="tableblock halign-left valign-top"><p>SONAR_PROJECT_KEY</p></td>
<td class="tableblock halign-left valign-top"><p>Static Code Analysis</p></td>
<td class="tableblock halign-left valign-top"><p>from <a href="https://sonarcloud.io/">sonarcloud</a></p></td>
</tr>
<tr class="even">
<td class="tableblock halign-left valign-top"><p>SONAR_PROJECT_NAME</p></td>
<td class="tableblock halign-left valign-top"><p>Static Code Analysis</p></td>
<td class="tableblock halign-left valign-top"><p>from <a href="https://sonarcloud.io/">sonarcloud</a></p></td>
</tr>
<tr class="odd">
<td class="tableblock halign-left valign-top"><p>SONAR_TOKEN</p></td>
<td class="tableblock halign-left valign-top"><p>Static Code Analysis</p></td>
<td class="tableblock halign-left valign-top"><p>from <a href="https://sonarcloud.io/">sonarcloud</a></p></td>
</tr>
<tr class="even">
<td class="tableblock halign-left valign-top"><p>PACT_BEARER_TOKEN</p></td>
<td class="tableblock halign-left valign-top"><p>Contract Test</p></td>
<td class="tableblock halign-left valign-top"><p>from <a href="https://docs.pact.io/">pact</a></p></td>
</tr>
<tr class="odd">
<td class="tableblock halign-left valign-top"><p>PACT_BROKER</p></td>
<td class="tableblock halign-left valign-top"><p>Contract Test</p></td>
<td class="tableblock halign-left valign-top"><p>from <a href="https://docs.pact.io/">pact</a></p></td>
</tr>
</tbody>
</table>

![Azure .NET Core Variable Group](../../../../../images/azure_netcore_variable_group.png)

### Update pipeline template placeholders

Where possible, the scaffolding CLI will have populated the correct values in the pipeline template file `build/azDevops/azure/api-pipeline.yml`. The values that need to be manually configured, such as the variable group name setup previously, will have placeholders using the prefix `%REPLACE_ME_FOR`. We very much recommend that you go through the whole template to make sure that values are correct for your project. Once you are happy with the template, commit the changes to your repository.

### Create the pipeline

Follow the steps below to create the pipeline and trigger the initial run.

1.  In the pipelines section of Azure DevOps, select **New Pipeline**.

2.  Select your repository.

3.  Select the **Existing Azure Pipelines YAML files** option and enter the path `build/azDevops/azure/api-pipeline.yml`.

4.  Click run and wait for the pipeline to complete.

5.  Update the API variable group with the Cosmos DB details.

6.  Enable Integration Tests in `build/azDevops/azure/api-pipeline.yml`.

7.  Commit changes to trigger the pipeline again.
