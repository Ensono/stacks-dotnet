FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ./ ./
RUN dotnet restore

# Copy everything else and build
COPY ./ ./
RUN dotnet publish -c Release -o out xxAMIDOxx.xxSTACKSxx.API
#list files copied to output
RUN du -ch xxAMIDOxx.xxSTACKSxx.API/out


# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
EXPOSE 80
WORKDIR /app
COPY --from=build-env /app/xxAMIDOxx.xxSTACKSxx.API/out .
ENTRYPOINT ["dotnet", "xxAMIDOxx.xxSTACKSxx.API.dll"]
