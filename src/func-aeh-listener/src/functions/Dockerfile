# Dockefile to create an image for either of the functions
#
# To build it, the function path needs to be passed in as an argument to the build.
# For example:
#
# docker build -t stacks-aeh-listener -f src/func-aeh-listener/src/functions/Dockerfile --build-arg self_repo_src=src/func-aeh-listener/src/functions .
ARG outdir=buildout

FROM mcr.microsoft.com/dotnet/sdk:8.0 as build

ARG outdir
ARG self_repo_src="src/functions"

WORKDIR /app

# Copy the worker project and build
COPY ./ ./
RUN dotnet publish -c Release -o ${outdir} $self_repo_src/xxENSONOxx.xxSTACKSxx.Listener

# Build the runtime image
FROM mcr.microsoft.com/azure-functions/dotnet-isolated:4-dotnet-isolated8.0 as runtime

ARG outdir

EXPOSE 80
WORKDIR /home/site/wwwroot
COPY --from=build /app/${outdir} .
