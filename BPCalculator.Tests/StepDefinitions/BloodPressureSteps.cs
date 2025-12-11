using System;
using TechTalk.SpecFlow;
using Xunit;
using BPCalculator;

namespace BPCalculator.Tests.StepDefinitions
{
    [Binding]
    public class BloodPressureSteps
    {
        private BloodPressure _bloodPressure;
        private string _category;
        private Exception _exception;

        [Given(@"a patient with systolic pressure of (.*)")]
        public void GivenPatientWithSystolicPressure(int systolic)
        {
            _bloodPressure = new BloodPressure { Systolic = systolic };
        }

        [Given(@"diastolic pressure of (.*)")]
        public void GivenDiastolicPressure(int diastolic)
        {
            _bloodPressure.Diastolic = diastolic;
        }

        [When(@"I calculate the blood pressure category")]
        public void WhenICalculateCategory()
        {
            try
            {
                _category = _bloodPressure.Category.ToString();
                _exception = null;
            }
            catch (Exception ex)
            {
                _exception = ex;
                _category = null;
            }
        }

        [Then(@"the category should be ""(.*)""")]
        public void ThenCategoryShouldBe(string expectedCategory)
        {
            Assert.Null(_exception);
            Assert.NotNull(_category);
            
            // Map enum name to display name
            string actualCategory = _bloodPressure.Category.ToString();
            string expected = expectedCategory switch
            {
                "Low Blood Pressure" => "Low",
                "Ideal Blood Pressure" => "Ideal",
                "Pre-High Blood Pressure" => "PreHigh",
                "High Blood Pressure" => "High",
                _ => expectedCategory
            };

            Assert.Equal(expected, actualCategory);
        }

        [Then(@"an validation error should occur")]
        public void ThenValidationErrorOccurs()
        {
            Assert.NotNull(_exception);
            Assert.IsType<System.ComponentModel.DataAnnotations.ValidationException>(_exception);
        }
    }
}