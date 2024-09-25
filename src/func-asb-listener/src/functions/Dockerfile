# Dockefile to create an image for either of the functions
#
# To build it, a function name needs to be passed in as an argument to the build.
# For example:
#
# docker build -t stacks-dotnet-cqrs --build-arg function=xxENSONOxx.xxSTACKSxx.Listener .\src\functions
ARG outdir=out

FROM mcr.microsoft.com/dotnet/sdk:8.0 as build

ARG outdir
ARG self_repo_src="src/functions"

WORKDIR /app

# Copy the worker project and build
COPY ./ ./
RUN dotnet publish -c Release -o ${outdir} $self_repo_src/xxENSONOxx.xxSTACKSxx.Listener

# Build the runtime image
FROM mcr.microsoft.com/azure-functions/dotnet:4 as runtime

ARG outdir

EXPOSE 80
WORKDIR /home/site/wwwroot
COPY --from=build /app/${outdir} .
