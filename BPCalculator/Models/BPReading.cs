using System;

namespace BPCalculator.Models
{
    public class BPReading
    {
        public int Systolic { get; set; }
        public int Diastolic { get; set; }
        public DateTime Timestamp { get; set; }
        public string Category { get; set; }

        public BPReading(int systolic, int diastolic, string category)
        {
            Systolic = systolic;
            Diastolic = diastolic;
            Category = category;
            Timestamp = DateTime.Now;
        }
    }
}