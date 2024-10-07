FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG nuget_url="https://replace_me"
ARG nuget_token="replace_me"
ARG NET_ARCH="linux-amd64"
ARG DOTNET_ASPIRE_CONTAINER_RUNTIME=docker

ENV STACKS_NUGET=${nuget_url}
ENV STACKS_NUGET_TOKEN=${nuget_token}
ENV DOTNET_ASPIRE_CONTAINER_RUNTIME=${DOTNET_ASPIRE_CONTAINER_RUNTIME}

WORKDIR /app

# Copy everything else and build
COPY ./ ./
RUN dotnet --version
RUN dotnet workload update && dotnet workload install aspire
RUN dotnet publish --runtime ${NET_ARCH} -c Release -o out xxENSONOxx.xxSTACKSxx.API

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "xxENSONOxx.xxSTACKSxx.API.dll"]
