# WARP.md

This file provides guidance to WARP (warp.dev) when working with code in this repository.

## Repository Overview

This is the **Ensono Stacks .NET Templates** repository that provides comprehensive .NET project templates for building cloud-native applications with different architectural patterns. The repository contains template source code that gets packaged into NuGet templates for developers to scaffold new projects.

### Core Architecture Patterns

**Template-Based Architecture**: This repository follows a template-first approach where actual source code serves as templates that get tokenized with placeholders (e.g., `xxENSONOxx.xxSTACKSxx`). These templates are packaged into the `Ensono.Stacks.Templates` NuGet package.

**Multi-Pattern Support**: The repository supports multiple architectural patterns:
- **Simple API** (`src/simple-api/`): Basic Web API with minimal complexity
- **CQRS** (`src/cqrs/`): Command Query Responsibility Segregation pattern with Domain-Driven Design
- **Background Workers** (`src/background-worker/`): Service Bus message processing workers
- **Azure Functions** (`src/func-*`): Event-driven serverless functions for various triggers

**Cloud-Agnostic Design**: Templates support both AWS and Azure cloud providers with specific configurations for each, including different data stores (CosmosDB, DynamoDB) and messaging systems (Service Bus, SNS, Event Hubs).

### Key Components

- **Domain Layer**: Contains business logic and domain entities
- **Application Layer**: Implements CQRS patterns with separate command/query handlers
- **Infrastructure Layer**: Database access, messaging, and external service integrations
- **API Layer**: REST endpoints with Swagger documentation
- **Shared Components** (`src/shared/`): Common messaging and infrastructure packages

## Common Development Commands

### Template Development & Testing

```powershell
# Test all templates (run from scripts/ directory)
pwsh test-templates.ps1

# Test templates from a specific branch
pwsh test-templates.ps1 -Branch feature-branch

# Clean up test artifacts
pwsh clean-up-template-test.ps1

# Generate expected directory trees for validation
pwsh test-templates.ps1 -GenerateExpectedTrees
```

### Template Installation & Usage

```powershell
# Install templates from current repository
dotnet new --install .

# Create CQRS project for Azure with CosmosDB
dotnet new stacks-cqrs-app -n Company.Project -do DomainName -db CosmosDb --cloudProvider Azure

# Create simple API for AWS
dotnet new stacks-webapi -n Company.Project -do DomainName --cloudProvider AWS

# Create Azure Function with Service Bus trigger
dotnet new stacks-az-func-asb-listener -n Company.Project -do DomainName

# Uninstall templates
dotnet new --uninstall Ensono.Stacks.Templates
```

### Building & Testing

```powershell
# Build template package
dotnet build template.csproj

# Run tests using eirctl (main build tool)
eirctl lint          # YAML and Terraform linting
eirctl test           # Run all test suites with SonarCloud analysis
eirctl build:container    # Build Docker images
eirctl build:functional_tests    # Build functional tests
```

### Infrastructure & Deployment

```powershell
# Infrastructure operations
eirctl infrastructure    # Plan and apply Terraform
eirctl deploy           # Deploy to Kubernetes with Helm
eirctl functional_tests # Run end-to-end tests

# Deploy to specific environments
eirctl deploy           # With ENV_NAME=dev|prod
```

## Development Environment Setup

### Prerequisites

- **.NET 8.0 SDK** (specified in `global.json`)
- **PowerShell 7+** (required for eirctl tasks)
- **Docker** (for containerization)
- **eirctl** (build orchestration tool, version 0.7.10)

### Local Development Setup

For CQRS templates with external dependencies:

```powershell
# Set required environment variables
$env:COSMOSDB_KEY = "your-cosmosdb-key"
$env:SERVICEBUS_CONNECTIONSTRING = "your-service-bus-connection"
$env:EVENTHUB_CONNECTIONSTRING = "your-event-hub-connection"
$env:TOPIC_ARN = "your-sns-topic-arn"  # For AWS
```

### Testing Individual Components

```powershell
# Run specific test patterns
dotnet test **/*UnitTests*
dotnet test **/*ComponentTests*
dotnet test **/*ContractTests*
dotnet test **/*FunctionalTests*

# Run tests with coverage
dotnet test -p:CollectCoverage=true -p:CoverletOutputFormat=opencover
```

## Multi-Cloud & Template Considerations

### Template Token System

All templates use a consistent token replacement system:
- `xxENSONOxx` → Company/Organization name
- `xxSTACKSxx` → Project name  
- `xxDOMAINxx` → Domain object name

### Cloud Provider Variations

**Azure-Specific Components**:
- CosmosDB for data persistence
- Azure Service Bus for messaging
- Azure Event Hubs for streaming
- Azure Container Apps (ACA) or Azure Kubernetes Service (AKS) deployment

**AWS-Specific Components**:
- DynamoDB for data persistence  
- SNS for messaging
- EKS for Kubernetes deployment
- ECR for container registry

### Build System Architecture

The repository uses **eirctl** as the primary build orchestration tool with:
- **Context-based execution**: Different execution contexts (powershell, powershell-dotnet)
- **Pipeline composition**: Reusable tasks combined into pipelines
- **Multi-environment support**: Dev/prod deployment workflows
- **Integrated toolchain**: SonarCloud, Terraform, Helm, kubectl integration

### CI/CD Integration

- **GitHub Actions** workflows for both simple-api and cqrs templates
- **Azure DevOps** pipeline templates in `build/azDevOps/`
- **Environment-based deployments** with separate dev/prod configurations
- **Terraform state management** with backend configuration
- **Container image publishing** to AWS ECR or Azure ACR

## File Structure Patterns

When working with templates, understand these key directories:
- `/src/{template-type}/src/api/` - Web API implementations
- `/src/{template-type}/src/tests/` - Test projects (Unit, Component, Functional)
- `/build/` - Build scripts and pipeline definitions
- `/deploy/` - Infrastructure as Code (Terraform) and deployment configurations
- `/scripts/` - Development and testing automation scripts

## Template Best Practices

- Always test template generation using the scripts in `/scripts/`
- Maintain token consistency across all template files
- Update expected directory trees when adding/removing template files
- Ensure both Azure and AWS variants work for applicable templates
- Follow the established layered architecture patterns (Domain → Application → Infrastructure → API)
