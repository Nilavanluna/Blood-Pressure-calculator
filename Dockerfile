# Use .NET 9 SDK for building
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy csproj and restore dependencies
COPY BPCalculator/*.csproj ./BPCalculator/
RUN dotnet restore ./BPCalculator/BPCalculator.csproj

# Copy everything else and build
COPY BPCalculator/. ./BPCalculator/
WORKDIR /app/BPCalculator
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/BPCalculator/out .

# Render uses port 10000
ENV ASPNETCORE_URLS=http://0.0.0.0:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "BPCalculator.dll"]