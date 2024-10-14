# Template Test Scripts

These scripts have been created to simplify the testing of each template within the Stacks.Dotnet.Templates.

## Scripts

`test-templates.ps1` can use your local `stacks-dotnet` or clone a specific branch of the `stacks-dotnet` repository by setting the Branch parameter (`-Branch some-feature-branch`). 

`clean-up-template-test.ps1` simply deletes anything produced by the `test-templates.ps1`. 
> This was separated into it's own script as there may be a requirement to manually test the output of `test-templates.ps1` and therefore these would need to be cleaned up at a later date. 

## How to use the scripts

### Test-Templates

1. Change your terminal directory to run from the `scripts` folder.
2. Run the `test-templates.ps1` script from here:
    - `pwsh test-templates.ps1` - Run tests using your local `stacks-dotnet`
    - `pwsh test-templates.ps1 -Branch <branch-name>` - Clone and test a specific branch
3. This will: 
    - Create a new directory called `stacks-dotnet-testing` in your temp directory.
    - Clone the `stacks-dotnet` repository if a branch parameter is provided.
    - Restore the repository.
    - Test the repository.
    - Install the templates (to ensure you have the latest versions).
    - Create a new directory within `stacks-dotnet-testing`called `test-templates`.
    - Create new templates for each existing within `test-templates`:
        - Simple.WebAPI - `stacks-webapi`
        - CQRS.Project - `stacks-cqrs`
        - Cosmos.Worker.Project - `stacks-az-func-cosmosdb-worker`
        - EventHub.Listener.Project - `stacks-az-func-aeh-listener`
        - ServiceBus.Listener.Project - `stacks-az-func-asb-listener`
        - ServiceBus.Worker.Project - `stacks-asb-worker`
    - Test each new created template project by restoring and running the unit tests. 

Output directory:

```
stacks-dotnet-testing
├── stacks-dotnet
└── test-templates
    ├── Cosmos.Worker
    ├── Cqrs
    ├── Cqrs.AllTheThings
    ├── Cqrs.Dynamo
    ├── Cqrs.ServiceBus
    ├── Cqrs.Sns
    ├── EventHub.Listener
    ├── ServiceBus.Listener
    ├── Simple.Api
    ├── Simple.Api.AWS
    └── Simple.Api.Azure
```

### Clean-Up-Template-Test

1. Ensure you are within the `scripts` directory.
2. Run the `clean-up-template-test.ps1` script from here:
    - `pwsh clean-up-template-test.ps1`
3. This will:
    - Remove the `stacks-dotnet-testing` directory.

### Generating the expected directory trees

The template outputs are validated during testing using the directory tree files in the `scripts/expected-trees` directory. When files are added or removed from the template the expected directory trees will need to be updated. This can be done using the `test-templates.ps1` script with the `-GenerateExpectedTrees` parameter (`pwsh test-templates.ps1 -GenerateExpectedTrees`)
