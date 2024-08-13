# From the Output folder

rm * -force -recurse; dotnet new stacks-webapi --name SimpleApi; code .
start devenv .\SimpleApi\simple-api\src\api\SimpleApi.API.sln