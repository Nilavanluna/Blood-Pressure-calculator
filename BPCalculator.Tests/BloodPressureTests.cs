using Xunit;

namespace BPCalculator.Tests
{
    public class BloodPressureTests
    {
        [Theory]
        [InlineData(85, 55, BPCategory.Low)]
        [InlineData(115, 75, BPCategory.Ideal)]
        [InlineData(130, 85, BPCategory.PreHigh)]
        [InlineData(145, 95, BPCategory.High)]
        public void Category_ReturnsExpected(int systolic, int diastolic, BPCategory expected)
        {
            var bp = new BloodPressure { Systolic = systolic, Diastolic = diastolic };
            Assert.Equal(expected, bp.Category);
        }

        [Fact]
        public void Category_Throws_WhenSystolicNotGreaterThanDiastolic()
        {
            var bp = new BloodPressure { Systolic = 80, Diastolic = 90 };
            Assert.Throws<System.ComponentModel.DataAnnotations.ValidationException>(() => _ = bp.Category);
        }
    }
}
