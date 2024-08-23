FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG nuget_url="https://replace_me"
ARG nuget_token="replace_me"
ARG self_repo_src="src/api"

ENV STACKS_NUGET=${nuget_url}
ENV STACKS_NUGET_TOKEN=${nuget_token}

WORKDIR /app

# Copy everything else and build
COPY ./ ./

RUN dotnet publish -c Release -o out $self_repo_src/xxENSONOxx.xxSTACKSxx.API
#list files copied to output
RUN find
RUN du -ch out/

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "xxENSONOxx.xxSTACKSxx.API.dll"]
