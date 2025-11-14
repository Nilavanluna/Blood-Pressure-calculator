#!/bin/bash
echo "Running Smoke Tests..."
dotnet test BPCalculator.Tests/BPCalculator.Tests.csproj --filter "Category=Smoke"
echo "Smoke Tests Completed"
