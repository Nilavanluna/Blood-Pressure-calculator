#!/bin/bash

# You will need to install k6 on your CI/CD runner.
# The URL to test needs to be an environment variable set in your pipeline config (e.g., $STAGING_URL)

K6_SCRIPT="scripts/performance-test.js"
URL_TO_TEST="http://your-deployed-url.com" # Replace with actual dynamic variable

echo "Running k6 performance tests against $URL_TO_TEST"

# Replace the URL in the k6 script and run the test
# Note: You need to pass the URL to the k6 script as an environment variable or via command line arguments.

k6 run --vus 10 --duration 30s --base-url "$URL_TO_TEST" "$K6_SCRIPT"

if [ $? -ne 0 ]; then
    echo "Performance tests failed!"
    exit 1
fi

echo "Performance tests passed."