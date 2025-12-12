#!/bin/bash

echo "Deploying to GREEN environment on Render..."

curl -X POST \
  -H "Accept: application/json" \
  -H "Authorization: Bearer $RENDER_API_KEY" \
  -d '{}' \
  https://api.render.com/v1/services/$RENDER_GREEN_SERVICE_ID/deploys

echo "Triggered GREEN deployment successfully."
