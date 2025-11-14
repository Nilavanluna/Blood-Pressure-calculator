#!/bin/bash
URL=$1
echo "Running health check on $URL..."

STATUS=$(curl -s -o /dev/null -w "%{http_code}" $URL)

if [ $STATUS -ne 200 ]; then
  echo "Health Check Failed! Status: $STATUS"
  exit 1
else
  echo "Health Check Passed!"
fi
