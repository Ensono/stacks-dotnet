FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG nuget_url="https://replace_me"
ARG nuget_token="replace_me"

ENV STACKS_NUGET=${nuget_url}
ENV STACKS_NUGET_TOKEN=${nuget_token}

WORKDIR /app

# Copy everything else and build
COPY ./ ./
RUN dotnet publish -c Release -o out xxAMIDOxx.xxSTACKSxx.API
#list files copied to output
RUN find
RUN du -ch out/

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "xxAMIDOxx.xxSTACKSxx.API.dll"]
