using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;

// page model

namespace BPCalculator.Pages
{
    public class BloodPressureModel : PageModel
    {
        private readonly TelemetryClient _telemetryClient;

        [BindProperty]                              // bound on POST
        public BloodPressure BP { get; set; }

        public BloodPressureModel(TelemetryClient telemetryClient)
        {
            _telemetryClient = telemetryClient;
        }

        // setup initial data
        public void OnGet()
        {
            BP = new BloodPressure() { Systolic = 100, Diastolic = 60 };
            _telemetryClient.TrackEvent("BP_Page_Loaded");
        }

        // POST, validate
        public IActionResult OnPost()
        {
            try
            {
                // Log input
                var properties = new Dictionary<string, string>
                {
                    { "Systolic", BP.Systolic.ToString() },
                    { "Diastolic", BP.Diastolic.ToString() }
                };
                _telemetryClient.TrackEvent("BP_Form_Submitted", properties);

                // extra validation
                if (!(BP.Systolic > BP.Diastolic))
                {
                    ModelState.AddModelError("", "Systolic must be greater than Diastolic");
                    _telemetryClient.TrackEvent("BP_Validation_Failed", properties);
                    return Page();
                }

                // Calculate category
                string category = CalculateCategory(BP.Systolic, BP.Diastolic);

                // Log result
                var resultProperties = new Dictionary<string, string>
                {
                    { "Systolic", BP.Systolic.ToString() },
                    { "Diastolic", BP.Diastolic.ToString() },
                    { "Category", category }
                };
                _telemetryClient.TrackEvent("BP_Category_Calculated", resultProperties);

                return Page();
            }
            catch (Exception ex)
            {
                _telemetryClient.TrackException(ex);
                throw;
            }
        }

        // Helper method to calculate blood pressure category
        private string CalculateCategory(int systolic, int diastolic)
        {
            if (systolic < 90 && diastolic < 60)
                return "Low Blood Pressure";
            else if (systolic <= 119 && diastolic <= 79)
                return "Ideal Blood Pressure";
            else if (systolic <= 139 || diastolic <= 89)
                return "Pre-High Blood Pressure";
            else
                return "High Blood Pressure";
        }
    }
}