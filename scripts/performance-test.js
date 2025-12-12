import http from 'k6/http'
import { check, sleep } from 'k6'

// Performance test configuration
export const options = {
  stages: [
    { duration: '30s', target: 20 }, // Ramp up to 20 users
    { duration: '1m', target: 50 }, // Stay at 50 users for 1 minute
    { duration: '10s', target: 0 }, // Ramp down to 0
  ],
  thresholds: {
    http_req_duration: ['p(95)<500'], // 95% of requests must complete below 500ms
    http_req_failed: ['rate<0.01'], // Less than 1% of requests can fail
  },
}

export default function () {
  const BASE_URL = __ENV.K6_BASE_URL || 'http://localhost:5000'

  // Generate random valid blood pressure readings
  const systolic = Math.floor(Math.random() * (190 - 70 + 1)) + 70
  let diastolic = Math.floor(Math.random() * (100 - 40 + 1)) + 40

  // Ensure systolic > diastolic
  if (systolic <= diastolic) {
    diastolic = systolic - 10
  }

  // Test the main page load
  const homeResponse = http.get(BASE_URL)
  check(homeResponse, {
    'homepage loads': (r) => r.status === 200,
    'contains Blood Pressure': (r) =>
      r.body && r.body.includes('Blood Pressure'),
  })

  sleep(1)

  // Test form submission
  const formData = {
    Systolic: systolic.toString(),
    Diastolic: diastolic.toString(),
  }

  const submitResponse = http.post(BASE_URL, formData)
  check(submitResponse, {
    'form submission successful': (r) => r.status === 200,
    'category displayed': (r) =>
      r.body &&
      (r.body.includes('Low') ||
        r.body.includes('Ideal') ||
        r.body.includes('High')),
  })

  sleep(1)
}
