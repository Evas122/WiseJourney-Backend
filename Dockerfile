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
COPY tests/WiseJourneyBackend.Application.Tests/WiseJourneyBackend.Application.Tests.csproj ./tests/WiseJourneyBackend.Application.Tests/
COPY tests/WiseJourneyBackend.Domain.Tests/WiseJourneyBackend.Domain.Tests.csproj ./tests/WiseJourneyBackend.Domain.Tests/
COPY tests/WiseJourneyBackend.Infrastructure.Tests/WiseJourneyBackend.Infrastructure.Tests.csproj ./tests/WiseJourneyBackend.Infrastructure.Tests/

RUN dotnet restore


COPY . .
WORKDIR /app/src/WiseJourneyBackend.Api
RUN dotnet publish -c Release -o out

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/src/WiseJourneyBackend.Api/out ./ 
ENTRYPOINT ["dotnet", "WiseJourneyBackend.Api.dll"]