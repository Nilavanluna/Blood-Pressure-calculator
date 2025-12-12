#!/bin/bash

# This script receives the deployment URL as the first command-line argument ($1)
URL_TO_TEST="$1" 
K6_SCRIPT="scripts/performance-test.js"

if [ -z "$URL_TO_TEST" ]; then
    echo "ERROR: Deployment URL is missing. Cannot run performance tests."
    exit 1
fi

echo "Running k6 performance tests against $URL_TO_TEST"

# Use --env to set the K6_BASE_URL environment variable inside the k6 runtime.
# This environment variable is read by the performance-test.js script.
k6 run \
  --env K6_BASE_URL="$URL_TO_TEST" \
  "$K6_SCRIPT"

if [ $? -ne 0 ]; then
    echo "Performance tests failed! Check k6 output above."
    exit 1
fi

echo "Performance tests passed."