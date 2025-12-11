using Xunit;
using BPCalculator.Services;
using BPCalculator.Models;

namespace BPCalculator.Tests
{
    public class BPStatisticsServiceTests
    {
        [Fact]
        public void AddReading_AddsReadingToList()
        {
            var service = new BPStatisticsService();
            service.AddReading(120, 80, "Ideal");
            
            Assert.Equal(1, service.GetReadings().Count);
        }

        [Fact]
        public void AddReading_KeepsMaximum10Readings()
        {
            var service = new BPStatisticsService();
            for (int i = 0; i < 15; i++)
            {
                service.AddReading(120 + i, 80 + i, "Ideal");
            }
            
            Assert.Equal(10, service.GetReadings().Count);
        }

        [Fact]
        public void GetAverageSystolic_CalculatesCorrectly()
        {
            var service = new BPStatisticsService();
            service.AddReading(100, 60, "Low");
            service.AddReading(120, 80, "Ideal");
            service.AddReading(140, 90, "High");
            
            Assert.Equal(120, service.GetAverageSystolic());
        }

        [Fact]
        public void GetHighestSystolic_ReturnsMaxValue()
        {
            var service = new BPStatisticsService();
            service.AddReading(100, 60, "Low");
            service.AddReading(150, 90, "High");
            service.AddReading(120, 80, "Ideal");
            
            Assert.Equal(150, service.GetHighestSystolic());
        }

        [Fact]
        public void GetLowestSystolic_ReturnsMinValue()
        {
            var service = new BPStatisticsService();
            service.AddReading(100, 60, "Low");
            service.AddReading(150, 90, "High");
            service.AddReading(120, 80, "Ideal");
            
            Assert.Equal(100, service.GetLowestSystolic());
        }

        [Fact]
        public void GetConsistencyScore_HighConsistency_HighScore()
        {
            var service = new BPStatisticsService();
            service.AddReading(120, 80, "Ideal");
            service.AddReading(121, 81, "Ideal");
            service.AddReading(119, 79, "Ideal");
            
            var score = service.GetConsistencyScore();
            Assert.NotNull(score);
            Assert.True(score > 80);
        }

        [Fact]
        public void GetAverageSystolic_NoReadings_ReturnsNull()
        {
            var service = new BPStatisticsService();
            Assert.Null(service.GetAverageSystolic());
        }

        [Fact]
        public void GetReadings_ReturnsCorrectOrder()
        {
            var service = new BPStatisticsService();
            service.AddReading(100, 60, "Low");
            service.AddReading(120, 80, "Ideal");
            
            var readings = service.GetReadings();
            Assert.Equal(2, readings.Count);
            Assert.Equal(100, readings[0].Systolic);
            Assert.Equal(120, readings[1].Systolic);
        }
    }
}