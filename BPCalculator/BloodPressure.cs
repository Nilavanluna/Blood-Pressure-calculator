using System;
using System.ComponentModel.DataAnnotations;

namespace BPCalculator
{
    // Blood Pressure categories
    public enum BPCategory
    {
        [Display(Name = "Low Blood Pressure")] Low,
        [Display(Name = "Ideal Blood Pressure")] Ideal,
        [Display(Name = "Pre-High Blood Pressure")] PreHigh,
        [Display(Name = "High Blood Pressure")] High
    };

    public class BloodPressure
    {
        // Allowed ranges
        public const int SystolicMin = 70;
        public const int SystolicMax = 190;
        public const int DiastolicMin = 40;
        public const int DiastolicMax = 100;

        [Range(SystolicMin, SystolicMax, ErrorMessage = "Invalid Systolic Value")]
        public int Systolic { get; set; }  // mmHg

        [Range(DiastolicMin, DiastolicMax, ErrorMessage = "Invalid Diastolic Value")]
        public int Diastolic { get; set; } // mmHg

        // Calculates the BP category based on systolic/diastolic values
        public BPCategory Category
        {
            get
            {
                // Validation: systolic must always be higher than diastolic
                if (Systolic <= Diastolic)
                {
                    throw new ValidationException("Systolic pressure must be greater than diastolic pressure.");
                }

                // ---- Blood Pressure Classification ----
                // Low Blood Pressure
                if (Systolic < 90 && Diastolic < 60)
                {
                    return BPCategory.Low;
                }

                // Ideal Blood Pressure
                else if ((Systolic >= 90 && Systolic <= 119) && (Diastolic >= 60 && Diastolic <= 79))
                {
                    return BPCategory.Ideal;
                }

                // Pre-High Blood Pressure
                else if ((Systolic >= 120 && Systolic <= 139) || (Diastolic >= 80 && Diastolic <= 89))
                {
                    return BPCategory.PreHigh;
                }

                // High Blood Pressure
                else
                {
                    return BPCategory.High;
                }
            }
        }
    }
}