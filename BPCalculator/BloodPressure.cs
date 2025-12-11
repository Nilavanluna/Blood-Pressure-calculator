using System;
using System.ComponentModel.DataAnnotations;

namespace BPCalculator
{
    public class BloodPressure
    {
        public const int SystolicMin = 70;
        public const int SystolicMax = 190;
        public const int DiastolicMin = 40;
        public const int DiastolicMax = 100;

        [Range(SystolicMin, SystolicMax)]
        public int Systolic { get; set; }

        [Range(DiastolicMin, DiastolicMax)]
        public int Diastolic { get; set; }

        public BPCategory Category
        {
            get
            {
                if (Systolic <= Diastolic)
                    throw new ValidationException("Systolic must be greater than Diastolic.");

                if (Systolic < 90 && Diastolic < 60)
                    return BPCategory.Low;
                else if (Systolic <= 119 && Diastolic <= 79)
                    return BPCategory.Ideal;
                else if (Systolic <= 139 && Diastolic <= 89)
                    return BPCategory.PreHigh;
                else
                    return BPCategory.High;
            }
        }
    }
}
