using System;
using System.Collections.Generic;
using System.Linq;
using BPCalculator.Models;

namespace BPCalculator.Services
{
    public class BPStatisticsService
    {
        private List<BPReading> _readings = new List<BPReading>();

        public void AddReading(int systolic, int diastolic, string category)
        {
            _readings.Add(new BPReading(systolic, diastolic, category));
            if (_readings.Count > 10)
                _readings.RemoveAt(0);
        }

        public List<BPReading> GetReadings() => _readings;

        public int? GetAverageSystolic() => _readings.Count > 0 ? (int)_readings.Average(r => r.Systolic) : null;

        public int? GetAverageDiastolic() => _readings.Count > 0 ? (int)_readings.Average(r => r.Diastolic) : null;

        public int? GetMedianSystolic()
        {
            if (_readings.Count == 0) return null;
            var sorted = _readings.OrderBy(r => r.Systolic).ToList();
            int mid = sorted.Count / 2;
            return sorted.Count % 2 == 0 ? (sorted[mid - 1].Systolic + sorted[mid].Systolic) / 2 : sorted[mid].Systolic;
        }

        public int? GetMedianDiastolic()
        {
            if (_readings.Count == 0) return null;
            var sorted = _readings.OrderBy(r => r.Diastolic).ToList();
            int mid = sorted.Count / 2;
            return sorted.Count % 2 == 0 ? (sorted[mid - 1].Diastolic + sorted[mid].Diastolic) / 2 : sorted[mid].Diastolic;
        }

        public int? GetHighestSystolic() => _readings.Count > 0 ? _readings.Max(r => r.Systolic) : null;

        public int? GetLowestSystolic() => _readings.Count > 0 ? _readings.Min(r => r.Systolic) : null;

        public int? GetConsistencyScore()
        {
            if (_readings.Count < 2) return null;
            var avgSys = GetAverageSystolic();
            var stdDev = Math.Sqrt(_readings.Average(r => Math.Pow((double)(r.Systolic - avgSys), 2)));
            return (int)(100 - Math.Min(stdDev * 2, 100));
        }
    }
}