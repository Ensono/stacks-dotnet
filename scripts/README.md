# Template Test Scripts

These scripts have been created to simplify the testing of each template within the Stacks.Dotnet.Templates.

## Scripts

`test-templates.sh` clones the `stacks-dotnet` repository and checksout to the specified branch inserted as parameter 1(`$1`). 

`clean-up-template-test.sh` simply deletes anything produced by the `test-templates.sh`. 
> This was separated into it's own script as there may be a requirement to manually test the output of `test-templates.sh` and therefore these would need to be cleaned up at a later date. 

## How to use the scripts

### Test-Templates

1. Change your terminal directory to run from the `scrips` folder.
2. Run the `test-templates.sh` script from here:
    - `sh test-templates.sh <insert-branch-name-here>`
    - E.g `sh test-templates.sh master`
3. This will: 
    - Create a new directory called `STACKS-TESTING` at the same level as the `stacks-dotnet` repository.
    - Clone the `stacks-dotnet` repository.
    - Restore the repository.
    - Test the repository.
    - Uninstall the templates (just in case you have them already).
    - Install the templates (to ensure you have the latest versions).
    - Create a new directory within `STACKS-TESTING`called `test-templates`.
    - Create new templates for each existing within `stacks-dotnet`:
        - Simple.WebAPI - `stacks-webapi`
        - CQRS.Project - `stacks-cqrs-app`
        - CQRS.Added.Project - `stacks-add-cqrs`
        - Cosmos.Worker.Project - `stacks-az-func-cosmosdb-worker`
        - EventHub.Listener.Project - `stacks-az-func-aeh-listener`
        - ServiceBus.Listener.Project - `stacks-az-func-asb-listener`
        - ServiceBus.Worker.Project - `stacks-asb-worker`
    - Test each new created template project by restoring and running the unit tests. 

Output directory:

```
STACKS-TESTING
├── stacks-dotnet
└── test-templates
    ├── CQRS.Added.Project
    ├── CQRS.Project
    ├── Cosmos.Worker.Project
    ├── EventHub.Listener.Project
    ├── ServiceBus.Listener.Project
    ├── ServiceBus.Worker.Project
    └── Simple.WebAPI
```

### Clean-Up-Template-Test

1. Ensure you are within the `scripts` directory.
2. Run the `clean-up-template-test.sh` script from here:
    - `sh clean-up-template-test.sh`
3. This will:
    - Remove the `STACKS-TESTING` directory.


