# Dockefile to create an image for either of the functions
#
# To build it, the function path needs to be passed in as an argument to the build.
# For example:
#
# docker build -t stacks-cosmosdb-worker -f src/func-cosmosdb-worker/src/functions/Dockerfile --build-arg self_repo_src=src/func-cosmosdb-worker/src/functions .

ARG outdir=buildout

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 as build

ARG TARGETARCH
ARG outdir
ARG self_repo_src="src/functions"

WORKDIR /app

# Copy the worker project and build
COPY ./ ./
RUN dotnet publish -c Release -a $TARGETARCH -o ${outdir} $self_repo_src/xxENSONOxx.xxSTACKSxx.Worker

# Build the runtime image
FROM mcr.microsoft.com/azure-functions/dotnet-isolated:4-dotnet-isolated8.0 as runtime

ARG outdir

EXPOSE 80
WORKDIR /home/site/wwwroot
COPY --from=build /app/${outdir} .
