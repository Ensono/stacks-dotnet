# This script is used to generate the appsettings.json files for the functional tests in the pipeline.
@(
  @{
    displayName = "FunctionalTests"
    template = "tests/xxENSONOxx.xxSTACKSxx.API.FunctionalTests/appsettings.json"
    vars = @{}
  }
)
