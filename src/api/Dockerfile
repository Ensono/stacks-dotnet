FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build
WORKDIR /app

# Copy everything else and build
COPY ./ ./
RUN dotnet publish -c Release -o out xxAMIDOxx.xxSTACKSxx.API 
#list files copied to output
RUN find 
RUN du -ch out/


# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0 AS runtime
EXPOSE 80
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "xxAMIDOxx.xxSTACKSxx.API.dll"]
