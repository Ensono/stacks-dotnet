This project uses Azure DevOps, GitHub Actions and Terraform. Always check to see if the following MCP servers have a tool relevant to the user's request:

-   **ado** - Azure DevOps operations (pipelines, work items, builds)
-   **terraform** - Infrastructure as Code operations
-   **github** - GitHub repository, PR, and workflow management

Use different models for different tasks. For example, use `gpt-4o` for summarization tasks and `gpt-4o-mini` for simple tasks like generating titles or descriptions.

## Repository Context

This is the **Ensono Stacks .NET Templates** repository that provides comprehensive .NET project templates for building cloud-native applications. The repository contains template source code that gets packaged into NuGet templates (`Ensono.Stacks.Templates`) for developers to scaffold new projects.

### Key Architectural Patterns

- **Simple API** (`src/simple-api/`) - Basic Web API with minimal complexity
- **CQRS** (`src/cqrs/`) - Command Query Responsibility Segregation with Domain-Driven Design
- **Background Workers** (`src/background-worker/`) - Service Bus message processing
- **Azure Functions** (`src/func-*`) - Event-driven serverless functions (Event Hub, Service Bus, CosmosDB triggers)

### Build System: eirctl

This repository uses **eirctl** (not taskctl) as the primary build orchestration tool. All pipeline tasks are defined in:
- `eirctl.yaml` - Main configuration and pipeline definitions
- `build/eirctl/tasks.yaml` - Task definitions
- `build/eirctl/contexts.yaml` - Execution contexts

Common eirctl commands:
```bash
eirctl setup              # Initialize build environment and set build number
eirctl lint               # YAML and Terraform linting/validation
eirctl test               # Run all test suites (Unit, Component, Contract)
eirctl build:container    # Build Docker images
eirctl infrastructure     # Terraform plan and apply
eirctl deploy             # Deploy to Kubernetes with Helm
eirctl functional_tests   # Run end-to-end tests
```

When debugging pipeline failures, check:
1. Task definitions in `build/eirctl/tasks.yaml`
2. Pipeline structure in `eirctl.yaml`
3. Build logs from Azure DevOps using MCP tools

## Using MCP Server for Azure DevOps

When getting work items using MCP Server for Azure DevOps:
- Always use **batch tools** for updates (up to 200 items per batch)
- Use `get_work_items_batch_by_ids` after getting work item IDs
- Show fields: **ID, Type, Title, State** by default
- Present results in **rendered markdown tables**

### Azure DevOps Configuration

- **Organization**: https://dev.azure.com/ensonodigitaluk
- **Project**: [Stacks](https://dev.azure.com/ensonodigitaluk/Stacks)
- **Pipeline Definitions**: `build/azDevOps/azure/` (located in [`dotnet`](https://dev.azure.com/ensonodigitaluk/Stacks/_build?definitionScope=%5Cdotnet) folder in Azure Pipelines)
- **Common Variables**: `build/azDevOps/azure/ensono/ci-common-vars.yml`

### Pipeline Structure

All Azure DevOps pipelines follow this pattern:
1. Install eirctl (template: `build/azDevOps/templates/install-eirctl.yml`)
2. Pull Docker images (`eirctl image-pull`, `eirctl image-pull-dotnet`)
3. Setup (`eirctl setup`)
4. Lint (`eirctl lint`)
5. Test (`eirctl test`)
6. Build containers (`eirctl build:container`)
7. Deploy infrastructure (Terraform via eirctl)
8. Deploy application (Helm/ACA via eirctl)

## Template Development

### Token System

All templates use consistent placeholder tokens:
- `xxENSONOxx` → Company/Organization name
- `xxSTACKSxx` → Project name
- `xxDOMAINxx` → Domain object name

When modifying templates, maintain token consistency across all files.

### Testing Templates

Use the scripts in the `scripts/` directory:
```powershell
# Test all templates
./scripts/test-templates.ps1

# Test specific branch
./scripts/test-templates.ps1 -Branch feature-branch

# Clean up test artifacts
./scripts/clean-up-template-test.ps1

# Generate expected directory trees
./scripts/test-templates.ps1 -GenerateExpectedTrees
```

Expected tree files are stored in `scripts/expected-trees/` and used for validation.

## Multi-Cloud Support

### Azure-Specific Components
- **Data**: CosmosDB
- **Messaging**: Azure Service Bus, Event Hubs
- **Compute**: AKS (Azure Kubernetes Service) or ACA (Azure Container Apps)
- **Registry**: ACR (Azure Container Registry)

### AWS-Specific Components
- **Data**: DynamoDB
- **Messaging**: SNS (Simple Notification Service)
- **Compute**: EKS (Elastic Kubernetes Service)
- **Registry**: ECR (Elastic Container Registry)

When making changes, consider multi-cloud compatibility where applicable.

## Development Workflow

### Prerequisites
- .NET 8.0 SDK (version specified in `global.json`)
- PowerShell 7+
- Docker
- eirctl (version specified in pipeline variables, typically 0.7.10+)

### Making Changes

1. **For source code changes**: Update the relevant template in `src/{template-name}/`
2. **For build changes**: Update `build/eirctl/tasks.yaml` or `eirctl.yaml`
3. **For pipeline changes**: Update files in `build/azDevOps/azure/` or `.github/workflows/`
4. **For infrastructure changes**: Update Terraform files in `deploy/{cloud-provider}/`

### Testing Locally

```powershell
# Install templates locally
dotnet new --install .

# Create test project
dotnet new stacks-cqrs-app -n TestProject -do Menu --cloudProvider Azure

# Uninstall when done
dotnet new --uninstall Ensono.Stacks.Templates
```

## Debugging Build Failures

When investigating build failures:

1. **Use Azure DevOps MCP tools** to retrieve build logs:
   ```
   mcp_ado_pipelines_get_builds - List recent builds
   mcp_ado_pipelines_get_build_log - Get full build logs
   mcp_ado_pipelines_get_build_status - Check specific build status
   ```

2. **Common failure points**:
   - Syntax errors in `build/eirctl/tasks.yaml` (PowerShell command syntax)
   - Missing environment variables in pipeline YAML
   - Terraform validation failures (check `deploy/` directories)
   - Test failures (check test output in logs)
   - Docker build failures (check `Dockerfile` in each template)

3. **Check environment validation errors**:
   - Azure DevOps environments may need to be created/authorized
   - Look for "Environment prod could not be found" type messages

4. **For eirctl task failures**:
   - Review command syntax in `build/eirctl/tasks.yaml`
   - Check for proper use of PowerShell variables (`$env:VAR_NAME`)
   - Verify context is correct (powershell vs powershell-dotnet)

## Security

This repository contains CI/CD configurations with sensitive data:
- GitHub Actions workflows in `.github/workflows/`
- Azure DevOps pipelines in `build/azDevOps/azure/`
- Secrets are stored in GitHub repository settings and Azure DevOps variable groups

**Security Guidelines**:
- Ensure sensitive information is not exposed in logs or outputs
- Use MCP tools cautiously with sensitive data
- Follow branch protection and PR approval workflows
- **Never** disable GPG signing or bypass security controls

Please refer to [./copilot-security-instructions.md](./copilot-security-instructions.md) for comprehensive security requirements.
