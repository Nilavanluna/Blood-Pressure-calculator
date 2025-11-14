#!/bin/bash
echo "Running E2E Tests..."
dotnet test BPCalculator.Tests/BPCalculator.Tests.csproj --filter "Category=E2E"
echo "E2E Tests Completed"
