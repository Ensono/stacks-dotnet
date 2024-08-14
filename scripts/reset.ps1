# From the Output folder

rm * -force -recurse; dotnet new stacks-webapi --name SimpleApi; code .
start devenv .\SimpleApi\simple-api\src\api\SimpleApi.API.sln

rm * -force -recurse; dotnet new stacks-az-func-cosmosdb-worker --name MyCode; code .
start devenv .\MyCode\func-cosmosdb-worker\src\functions\MyCode.Worker.sln

rm * -force -recurse; dotnet new stacks-az-func-aeh-listener --name MyCode --domain Dom; code .
start devenv .\MyCode\func-aeh-listener\src\functions\MyCode.Listener.sln