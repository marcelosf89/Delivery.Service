#-------------------------------------------------------
# Base
#--------------------------------------------------------
FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT=Local \
    ASPNETCORE_URLS=http://+:80

EXPOSE 80

#-------------------------------------------------------
# Dependences
#--------------------------------------------------------
FROM microsoft/dotnet:2.1-sdk AS dependences

WORKDIR /source

COPY DeliveryService.sln .
COPY src/01-Presentation/DeliveryService.Api/DeliveryService.Api.csproj src/01-Presentation/DeliveryService.Api/
COPY src/04-Domain/DeliveryService.Domain.Model/DeliveryService.Domain.Model.csproj src/04-Domain/DeliveryService.Domain.Model/
COPY src/03-Infrastructure/DeliveryService.Infrastructure.Data/DeliveryService.Infrastructure.Data.csproj src/03-Infrastructure/DeliveryService.Infrastructure.Data/
COPY src/02-Application/DeliveryService.Application.Query/DeliveryService.Application.Query.csproj src/02-Application/DeliveryService.Application.Query/
COPY src/02-Application/DeliveryService.Application.Command/DeliveryService.Application.Command.csproj src/02-Application/DeliveryService.Application.Command/
COPY src/03-Infrastructure/DeliveryService.Infrastructure.Cassandra/DeliveryService.Infrastructure.Cassandra.csproj src/03-Infrastructure/DeliveryService.Infrastructure.Cassandra/
COPY src/99-Crosscutting/DeliveryService.Crosscutting/DeliveryService.Crosscutting.csproj src/99-Crosscutting/DeliveryService.Crosscutting/

#Test's project
COPY test/DeliveryService.Application.Command.Tests/DeliveryService.Application.Command.Tests.csproj test/DeliveryService.Application.Command.Tests/
COPY test/DeliveryService.Application.Query.Tests/DeliveryService.Application.Query.Tests.csproj test/DeliveryService.Application.Query.Tests/
COPY test/DeliveryService.Performance.Tests/DeliveryService.Performance.Tests.csproj test/DeliveryService.Performance.Tests/

RUN dotnet restore

#-------------------------------------------------------
# Build
#--------------------------------------------------------
FROM dependences AS build

WORKDIR /source

COPY . .

RUN dotnet build -c Release -o /app

#-------------------------------------------------------
# Test
#--------------------------------------------------------
FROM build AS test

WORKDIR /source/test

RUN dotnet test --no-restore

#-------------------------------------------------------
# Test Performance
#--------------------------------------------------------
FROM build AS test-performance

WORKDIR /source/test/DeliveryService.Performance.Tests

RUN dotnet run -c Release --no-restore -- path=/performance

#-------------------------------------------------------
# Publish
#--------------------------------------------------------
FROM build AS publish

WORKDIR /source

RUN dotnet publish src/01-Presentation/DeliveryService.Api --no-restore -c Release -o /app 
#/p:Version=$VERSION

#-------------------------------------------------------
# Main
#--------------------------------------------------------
FROM base AS main

WORKDIR /app

COPY --from=publish /app .

ENTRYPOINT ["dotnet", "DeliveryService.Api.dll"]
