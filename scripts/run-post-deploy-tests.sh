#!/bin/bash
echo "Running Post-Deployment Tests..."
dotnet test BPCalculator.Tests/BPCalculator.Tests.csproj --filter "Category=PostDeploy"
echo "Post-Deploy Tests Completed"
