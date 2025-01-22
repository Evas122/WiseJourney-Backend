# Use the official .NET SDK image as the base image for build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app
EXPOSE 5217

# Copy the project files
COPY WiseJourneyBackend.sln . 
COPY src/WiseJourneyBackend.Api/WiseJourneyBackend.Api.csproj ./src/WiseJourneyBackend.Api/
COPY src/WiseJourneyBackend.Application/WiseJourneyBackend.Application.csproj ./src/WiseJourneyBackend.Application/
COPY src/WiseJourneyBackend.Domain/WiseJourneyBackend.Domain.csproj ./src/WiseJourneyBackend.Domain/
COPY src/WiseJourneyBackend.Infrastructure/WiseJourneyBackend.Infrastructure.csproj ./src/WiseJourneyBackend.Infrastructure/

RUN dotnet restore


COPY . .
WORKDIR /app/src/WiseJourneyBackend.Api
RUN dotnet publish -c Release -o out

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/src/WiseJourneyBackend.Api/out ./ 
ENTRYPOINT ["dotnet", "WiseJourneyBackend.Api.dll"]