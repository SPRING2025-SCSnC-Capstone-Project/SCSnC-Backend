# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY *.sln . 
COPY ./src/Api/*.csproj ./src/Api/
COPY ./src/Application/*.csproj ./src/Application/
COPY ./src/Domain/*.csproj ./src/Domain/
COPY ./src/Infrastructure/*.csproj ./src/Infrastructure/
RUN dotnet restore ./src/Api/*.csproj

COPY . .
RUN dotnet build

# Publish stage
FROM build AS publish
WORKDIR /app/src/Api
RUN dotnet publish -c Release --no-restore -o /app/publish

# Final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
EXPOSE 8080
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Api.dll"]