#!/bin/bash
echo "Deploying to Staging environment..."
dotnet publish BPCalculator/BPCalculator.csproj -c Release -o ./staging
echo "Staging Deployment Complete"