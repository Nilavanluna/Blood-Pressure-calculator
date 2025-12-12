import http from 'k6/http'
import { check, sleep } from 'k6'

// Load testing configuration
export let options = {
  stages: [
    { duration: '10s', target: 10 },
    { duration: '20s', target: 10 },
    { duration: '5s', target: 0 },
  ],
  thresholds: {
    http_req_duration: ['p(90) < 400'],
    checks: ['rate>0.99'],
  },
}

export default function () {
  const url = `${__ENV.TEST_URL}/Index`

  const systolic = Math.floor(Math.random() * (190 - 70 + 1)) + 70
  const diastolic = Math.floor(Math.random() * (100 - 40 + 1)) + 40
  const finalSystolic = systolic > diastolic ? systolic : diastolic + 1

  const payload = {
    'BloodPressure.SystolicPressure': finalSystolic,
    'BloodPressure.DiastolicPressure': diastolic,
  }

  const params = {
    headers: {
      'Content-Type': 'application/x-www-form-urlencoded',
    },
  }

  const res = http.post(url, payload, params)

  check(res, {
    'is status 200': (r) => r.status === 200,
    'response has Category': (r) => r.body.includes('Category'),
  })

  sleep(1)
}
