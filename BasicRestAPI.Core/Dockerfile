# Build image
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app
COPY . ./
RUN dotnet publish -c Release -o out
# Normally you don't want to do this

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
# Copy over the xml file
COPY --from=build-env /app/BasicRestAPI.xml .
ENTRYPOINT ["dotnet", "./BasicRestAPI.dll"]
