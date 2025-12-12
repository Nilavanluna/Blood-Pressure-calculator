#!/bin/bash

echo "Switching PRODUCTION domain from BLUE → GREEN"

# 1️⃣ Remove domain from BLUE
curl -X DELETE \
  -H "Authorization: Bearer $RENDER_API_KEY" \
  "https://api.render.com/v1/services/$RENDER_BLUE_SERVICE_ID/custom-domains/$CUSTOM_DOMAIN"

echo "Removed domain from BLUE."

# 2️⃣ Add domain to GREEN
curl -X POST \
  -H "Authorization: Bearer $RENDER_API_KEY" \
  -H "Content-Type: application/json" \
  -d "{\"name\":\"$CUSTOM_DOMAIN\"}" \
  "https://api.render.com/v1/services/$RENDER_GREEN_SERVICE_ID/custom-domains"

echo "Added domain to GREEN."

echo "Blue-Green switch COMPLETE! Traffic now going → GREEN"
