This project uses Azure DevOps, GitHub Actions and Terraform. Always check to see if the the following MCP servers have a tool relevant to the user's request:

-   ado
-   terraform
-   github

Use different models for different tasks. For example, use `gpt-4o` for summarization tasks and `gpt-4o-mini` for simple tasks like generating titles or descriptions.

## Using MCP Server for Azure DevOps

When getting work items using MCP Server for Azure DevOps, always try to use batch tools for updates instead of many individual single updates. For updates, try and update up to 200 updates in a single batch. When getting work items, once you get the list of IDs, use the tool `get_work_items_batch_by_ids` to get the work item details. By default, show fields ID, Type, Title, State. Show work item results in a rendered markdown table.

This repository uses the following Azure DevOps project: https://dev.azure.com/ensonodigitaluk/Stacks, and the Azure DevOps pipeline definitions are in `build\azDevOps\azure` in this repository and located within the [`dotnet`](https://dev.azure.com/ensonodigitaluk/Stacks/_build?definitionScope=%5Cdotnet) folder in Azure Pipelines.

## Security

This repository contains GitHub Actions workflows in the `.github/workflows` folder. These workflows may use secrets stored in the GitHub repository settings. Ensure that any sensitive information is handled securely and not exposed in logs or outputs.

When using MCP Server for tasks involving sensitive data, ensure that the appropriate security measures are in place to protect this data. Avoid sharing sensitive information in prompts or responses.

Please refer to ./copilot-instructions-security.md for more details.
