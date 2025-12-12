import http from 'k6/http'
import { check, sleep } from 'k6'

// Define the load test options: 10 Virtual Users for 30 seconds
export let options = {
  stages: [
    { duration: '10s', target: 10 },
    { duration: '20s', target: 10 },
    { duration: '5s', target: 0 },
  ],
  // Set a threshold to ensure performance is acceptable
  thresholds: {
    http_req_duration: ['p(90) < 400'],
    checks: ['rate>0.99'],
  },
}

// The default function defines the main test logic
export default function () {
  // FIX: Read the base URL from the environment variable set by the shell script
  const BASE_URL = __ENV.K6_BASE_URL

  // Construct the full URL for the blood pressure calculation endpoint
  // Assuming the calculation form is submitted to the root Index page (Razor Pages convention)
  const url = `${BASE_URL}/Index`

  // Simulate a random, valid blood pressure reading for variety
  const systolic = Math.floor(Math.random() * (190 - 70 + 1)) + 70 // 70-190
  const diastolic = Math.floor(Math.random() * (100 - 40 + 1)) + 40 // 40-100
  // Ensure systolic is higher than diastolic (a project constraint)
  const finalSystolic = systolic > diastolic ? systolic : diastolic + 1

  // The data payload, simulating the form fields
  const payload = {
    'BloodPressure.SystolicPressure': finalSystolic.toString(),
    'BloodPressure.DiastolicPressure': diastolic.toString(),
  }

  // The simulated form POST request
  const res = http.post(url, payload)

  // FIX: Add a null check (r.body) before attempting .includes() to prevent the TypeError
  check(res, {
    'is status 200': (r) => r.status === 200,
    'response body contains "Category"': (r) =>
      r.body && r.body.includes('Category'),
  })

  // Pause for 1 second between virtual user iterations
  sleep(1)
}
