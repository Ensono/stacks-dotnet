This project uses Azure DevOps, GitHub Actions and Terraform. Always check to see if the the following MCP servers have a tool relevant to the user's request:

-   ado
-   terraform
-   github

Use different models for different tasks. For example, use `gpt-4o` for summarization tasks and `gpt-4o-mini` for simple tasks like generating titles or descriptions.

## Using MCP Server for Azure DevOps

When getting work items using MCP Server for Azure DevOps, always try to use batch tools for updates instead of many individual single updates. For updates, try and update up to 200 updates in a single batch. When getting work items, once you get the list of IDs, use the tool `get_work_items_batch_by_ids` to get the work item details. By default, show fields ID, Type, Title, State. Show work item results in a rendered markdown table.
