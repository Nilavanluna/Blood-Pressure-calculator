#!/bin/bash
echo "Deploying to Green Production environment..."
dotnet publish BPCalculator/BPCalculator.csproj -c Release -o ./production/green
echo "Green Deployment Complete"
