FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG nuget_url="https://replace_me"
ARG nuget_token="replace_me"
ARG dotnet_arch="amd64"
ARG dotnet_os="linux"
ARG DOTNET_ASPIRE_CONTAINER_RUNTIME=docker

ENV STACKS_NUGET=${nuget_url}
ENV STACKS_NUGET_TOKEN=${nuget_token}
ENV DOTNET_ASPIRE_CONTAINER_RUNTIME=${DOTNET_ASPIRE_CONTAINER_RUNTIME}
ENV DOTNET_ARCH=${dotnet_arch}
ENV DOTNET_OS=${dotnet_os}

WORKDIR /app

# Copy everything else and build
COPY ./ ./
# RUN dotnet --os ${DOTNET_OS} --arch ${DOTNET_ARCH} --version
# RUN dotnet workload --os ${DOTNET_OS} --arch ${DOTNET_ARCH} update && dotnet workload install --os ${DOTNET_OS} --arch ${DOTNET_ARCH} aspire
RUN dotnet publish --os ${DOTNET_OS} --arch ${DOTNET_ARCH} -c Release -o out xxENSONOxx.xxSTACKSxx.API 

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "xxENSONOxx.xxSTACKSxx.API.dll"]
