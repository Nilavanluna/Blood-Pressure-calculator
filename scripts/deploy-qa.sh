#!/bin/bash
echo "Deploying to QA environment..."
dotnet publish BPCalculator/BPCalculator.csproj -c Release -o ./qa
echo "QA Deployment Complete"
