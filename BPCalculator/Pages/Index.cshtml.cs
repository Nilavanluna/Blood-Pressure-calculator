using BPCalculator.Models;
using BPCalculator.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BPCalculator.Pages
{
    public class BloodPressureModel : PageModel
    {
        private readonly TelemetryClient _telemetryClient;
        private readonly BPStatisticsService _statisticsService;

        [BindProperty]
        public BloodPressure BP { get; set; }

        // NEW: Properties for statistics display
        public List<BPReading> Readings { get; set; } = new();
        public int? AverageSystolic { get; set; }
        public int? AverageDiastolic { get; set; }
        public int? MedianSystolic { get; set; }
        public int? MedianDiastolic { get; set; }
        public int? HighestSystolic { get; set; }
        public int? LowestSystolic { get; set; }
        public int? ConsistencyScore { get; set; }

        public BloodPressureModel(TelemetryClient telemetryClient, BPStatisticsService statisticsService)
        {
            _telemetryClient = telemetryClient;
            _statisticsService = statisticsService;
        }

        public void OnGet()
        {
            BP = new BloodPressure() { Systolic = 100, Diastolic = 60 };
            LoadStatistics();
            _telemetryClient.TrackEvent("BP_Page_Loaded");
        }

        public IActionResult OnPost()
        {
            try
            {
                var properties = new Dictionary<string, string>
                {
                    { "Systolic", BP.Systolic.ToString() },
                    { "Diastolic", BP.Diastolic.ToString() }
                };
                _telemetryClient.TrackEvent("BP_Form_Submitted", properties);

                if (!(BP.Systolic > BP.Diastolic))
                {
                    ModelState.AddModelError("", "Systolic must be greater than Diastolic");
                    _telemetryClient.TrackEvent("BP_Validation_Failed", properties);
                    LoadStatistics();
                    return Page();
                }

                string category = CalculateCategory(BP.Systolic, BP.Diastolic);

                // NEW: Add reading to statistics
                _statisticsService.AddReading(BP.Systolic, BP.Diastolic, category);

                var resultProperties = new Dictionary<string, string>
                {
                    { "Systolic", BP.Systolic.ToString() },
                    { "Diastolic", BP.Diastolic.ToString() },
                    { "Category", category }
                };
                _telemetryClient.TrackEvent("BP_Category_Calculated", resultProperties);

                // NEW: Load updated statistics
                LoadStatistics();

                return Page();
            }
            catch (Exception ex)
            {
                _telemetryClient.TrackException(ex);
                throw;
            }
        }

        // NEW: Helper method to load statistics
        private void LoadStatistics()
        {
            Readings = _statisticsService.GetReadings();
            AverageSystolic = _statisticsService.GetAverageSystolic();
            AverageDiastolic = _statisticsService.GetAverageDiastolic();
            MedianSystolic = _statisticsService.GetMedianSystolic();
            MedianDiastolic = _statisticsService.GetMedianDiastolic();
            HighestSystolic = _statisticsService.GetHighestSystolic();
            LowestSystolic = _statisticsService.GetLowestSystolic();
            ConsistencyScore = _statisticsService.GetConsistencyScore();
        }

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