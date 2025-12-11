# =========================
# Build stage
# =========================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy csproj and restore dependencies
COPY BPCalculator/*.csproj ./BPCalculator/
RUN dotnet restore ./BPCalculator/BPCalculator.csproj

# Copy all source code and publish
COPY BPCalculator/. ./BPCalculator/
WORKDIR /app/BPCalculator
RUN dotnet publish -c Release -o out

# =========================
# Runtime stage
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Copy published output from build stage
COPY --from=build /app/BPCalculator/out .

# Set environment variables
ENV ASPNETCORE_URLS=http://0.0.0.0:10000

# Expose the port Render will use
EXPOSE 10000

# Start the application
ENTRYPOINT ["dotnet", "BPCalculator.dll"]
