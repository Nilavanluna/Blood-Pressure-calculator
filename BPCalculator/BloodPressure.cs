using System;

namespace BPCalculator
{
    public class BloodPressure
    {
        public const int SystolicMin = 70;
        public const int SystolicMax = 190;
        public const int DiastolicMin = 40;
        public const int DiastolicMax = 100;

        public int Systolic { get; set; }
        public int Diastolic { get; set; }

        public BPCategory Category
        {
            get
            {
                // Validate ranges first
                if (Systolic < SystolicMin || Systolic > SystolicMax ||
                    Diastolic < DiastolicMin || Diastolic > DiastolicMax)
                {
                    throw new ArgumentOutOfRangeException("Blood pressure values out of range");
                }

                // Validate systolic > diastolic
                if (Systolic <= Diastolic)
                {
                    throw new ArgumentException("Systolic must be greater than Diastolic");
                }

                // Check High first (>=140 OR >=90) - MOST IMPORTANT
                if (Systolic >= 140 || Diastolic >= 90)
                    return BPCategory.High;

                // Check PreHigh (120-139 OR 80-89) - Fixed boundary
                if ((Systolic >= 120 && Systolic < 140) || (Diastolic >= 80 && Diastolic < 90))
                    return BPCategory.PreHigh;

                // Check Low (<90 AND <60)
                if (Systolic < 90 && Diastolic < 60)
                    return BPCategory.Low;

                // Otherwise Ideal (90-119 AND 60-79)
                return BPCategory.Ideal;
            }
        }
    }

    public enum BPCategory
    {
        Low,
        Ideal,
        PreHigh,
        High
    }
}