Feature: Blood Pressure Category Classification
  As a patient
  I want to know my blood pressure category
  So that I can understand my health status

  Scenario: Patient with low blood pressure
    Given a patient with systolic pressure of 85
    And diastolic pressure of 55
    When I calculate the blood pressure category
    Then the category should be "Low Blood Pressure"

  Scenario: Patient with ideal blood pressure
    Given a patient with systolic pressure of 110
    And diastolic pressure of 70
    When I calculate the blood pressure category
    Then the category should be "Ideal Blood Pressure"

  Scenario: Patient with pre-high blood pressure
    Given a patient with systolic pressure of 130
    And diastolic pressure of 85
    When I calculate the blood pressure category
    Then the category should be "Pre-High Blood Pressure"

  Scenario: Patient with high blood pressure
    Given a patient with systolic pressure of 150
    And diastolic pressure of 95
    When I calculate the blood pressure category
    Then the category should be "High Blood Pressure"

  Scenario: Systolic at boundary (140 = High)
    Given a patient with systolic pressure of 140
    And diastolic pressure of 80
    When I calculate the blood pressure category
    Then the category should be "High Blood Pressure"

  Scenario: Validation fails when systolic not greater than diastolic
    Given a patient with systolic pressure of 100
    And diastolic pressure of 120
    When I calculate the blood pressure category
    Then an validation error should occur