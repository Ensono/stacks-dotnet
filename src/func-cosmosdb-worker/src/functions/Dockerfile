# Dockefile to create an image for either of the functions
#
# To build it, a function name needs to be passed in as an argument to the build.
# For example:
#
# docker build -t stacks-dotnet-cqrs --build-arg function=xxENSONOxx.xxSTACKSxx.Listener .\src\functions
ARG outdir=buildout

FROM mcr.microsoft.com/dotnet/sdk:8.0 as build

ARG self_repo_src="src/functions"

WORKDIR /app

# Copy the worker project and build
COPY ./ ./
RUN mkdir -p /home/site/wwwroot && \
dotnet publish -c Release --output /home/site/wwwroot $self_repo_src/xxENSONOxx.xxSTACKSxx.Worker

# Build the runtime image
FROM mcr.microsoft.com/azure-functions/dotnet:4-dotnet8.0 as runtime
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true

COPY --from=build ["/home/site/wwwroot", "/home/site/wwwroot"]
