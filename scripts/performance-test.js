import http from 'k6/http'
import { check, sleep } from 'k6'

// Define the load test options: 10 Virtual Users for a duration of 30 seconds
export let options = {
  stages: [
    // Ramping up to 10 VUs over 10 seconds
    { duration: '10s', target: 10 },
    // Stay at 10 VUs for 20 seconds
    { duration: '20s', target: 10 },
    // Ramping down to 0 VUs over 5 seconds
    { duration: '5s', target: 0 },
  ],
  // Set a threshold to ensure performance is acceptable
  thresholds: {
    // 90% of requests must be finished within 400ms
    http_req_duration: ['p(90) < 400'],
    // Check that all responses were successful (HTTP 200)
    checks: ['rate>0.99'],
  },
}

// The default function defines the main test logic
export default function () {
  // NOTE: Replace 'http://localhost:5000' with the actual URL of your deployed application (e.g., QA or staging URL)
  const url = 'http://localhost:5000/Index'

  // Simulate a random, valid blood pressure reading for variety
  const systolic = Math.floor(Math.random() * (190 - 70 + 1)) + 70 // 70-190 [cite: 12]
  const diastolic = Math.floor(Math.random() * (100 - 40 + 1)) + 40 // 40-100 [cite: 12]

  // Ensure systolic is higher than diastolic (a project constraint) [cite: 12]
  const finalSystolic = systolic > diastolic ? systolic : diastolic + 1

  // The data payload, simulating the form fields from the BloodPressure.cs model (SystolicPressure, DiastolicPressure)
  const payload = {
    'BloodPressure.SystolicPressure': finalSystolic.toString(),
    'BloodPressure.DiastolicPressure': diastolic.toString(),
    // Note: Razor Pages POST often requires an anti-forgery token (__RequestVerificationToken)
    // which makes a simple k6 script fail. You may need to disable the token for performance endpoints
    // or perform a GET first to extract the token, which is more complex.
    // This example assumes for simplicity or that you'll disable anti-forgery for this test.
  }

  // The simulated form POST request
  const res = http.post(url, payload)

  // Check if the request was successful and the response contains the expected calculation result
  check(res, {
    'is status 200': (r) => r.status === 200,
    // Check for a specific string on the resulting page (e.g., the word 'Category' is present)
    'response body contains "Category"': (r) => r.body.includes('Category'),
  })

  // Pause for 1 second between virtual user iterations to simulate user think time
  sleep(1)
}
