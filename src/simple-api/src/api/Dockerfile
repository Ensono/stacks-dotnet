FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG nuget_url="https://replace_me"
ARG nuget_token="replace_me"
ARG NET_ARCH="linux-amd64"

ENV STACKS_NUGET=${nuget_url}
ENV STACKS_NUGET_TOKEN=${nuget_token}

WORKDIR /app

# Copy everything else and build
COPY ./ ./
RUN dotnet workload install aspire --skip-manifest-update
RUN dotnet publish --runtime ${NET_ARCH} -c Release -o out xxENSONOxx.xxSTACKSxx.API

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "xxENSONOxx.xxSTACKSxx.API.dll"]
