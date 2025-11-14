using Xunit;
using System.ComponentModel.DataAnnotations;

namespace BPCalculator.Tests
{
    public class BloodPressureTests
    {
        // ========== CATEGORY CLASSIFICATION TESTS ==========
        
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

        // ========== BOUNDARY TESTS ==========
        
        [Theory]
        [InlineData(89, 59, BPCategory.Low)]    // Just below Ideal threshold
        [InlineData(90, 60, BPCategory.Ideal)]  // Ideal boundary (inclusive)
        public void Category_LowBoundary(int systolic, int diastolic, BPCategory expected)
        {
            var bp = new BloodPressure { Systolic = systolic, Diastolic = diastolic };
            Assert.Equal(expected, bp.Category);
        }

        [Theory]
        [InlineData(119, 79, BPCategory.Ideal)]   // Ideal boundary (inclusive)
        [InlineData(120, 80, BPCategory.PreHigh)]   // PreHigh boundary
        public void Category_IdealBoundary(int systolic, int diastolic, BPCategory expected)
        {
            var bp = new BloodPressure { Systolic = systolic, Diastolic = diastolic };
            Assert.Equal(expected, bp.Category);
        }

        [Theory]
        [InlineData(139, 89, BPCategory.PreHigh)]   // PreHigh boundary (inclusive)
        [InlineData(140, 90, BPCategory.High)]      // High boundary (inclusive)
        public void Category_PreHighAndHighBoundary(int systolic, int diastolic, BPCategory expected)
        {
            var bp = new BloodPressure { Systolic = systolic, Diastolic = diastolic };
            Assert.Equal(expected, bp.Category);
        }

        // ========== EDGE CASES ==========
        
        [Fact]
        public void Category_MinimumValidValues()
        {
            var bp = new BloodPressure { Systolic = 70, Diastolic = 40 };
            Assert.Equal(BPCategory.Low, bp.Category);
        }

        [Fact]
        public void Category_MaximumValidValues()
        {
            var bp = new BloodPressure { Systolic = 190, Diastolic = 100 };
            Assert.Equal(BPCategory.High, bp.Category);
        }

        // ========== VALIDATION TESTS ==========
        
        [Fact]
        public void Category_Throws_WhenSystolicNotGreaterThanDiastolic()
        {
            var bp = new BloodPressure { Systolic = 80, Diastolic = 90 };
            Assert.Throws<ValidationException>(() => _ = bp.Category);
        }

        [Fact]
        public void Category_Throws_WhenSystolicEqualsDiastolic()
        {
            var bp = new BloodPressure { Systolic = 100, Diastolic = 100 };
            Assert.Throws<ValidationException>(() => _ = bp.Category);
        }

        [Fact]
        public void Category_Throws_WhenSystolicLessThanDiastolic()
        {
            var bp = new BloodPressure { Systolic = 70, Diastolic = 120 };
            Assert.Throws<ValidationException>(() => _ = bp.Category);
        }

        // ========== RANGE VALIDATION TESTS ==========
        
        [Theory]
        [InlineData(69, 50)]    // Systolic below minimum
        public void Systolic_BelowMinimum_FailsValidation(int systolic, int diastolic)
        {
            var bp = new BloodPressure { Systolic = systolic, Diastolic = diastolic };
            var context = new ValidationContext(bp);
            var results = new System.Collections.Generic.List<ValidationResult>();
            
            bool isValid = Validator.TryValidateObject(bp, context, results, validateAllProperties: true);
            
            Assert.False(isValid);
            Assert.NotEmpty(results);
        }

        [Theory]
        [InlineData(191, 90)]   // Systolic above maximum
        public void Systolic_AboveMaximum_FailsValidation(int systolic, int diastolic)
        {
            var bp = new BloodPressure { Systolic = systolic, Diastolic = diastolic };
            var context = new ValidationContext(bp);
            var results = new System.Collections.Generic.List<ValidationResult>();
            
            bool isValid = Validator.TryValidateObject(bp, context, results, validateAllProperties: true);
            
            Assert.False(isValid);
            Assert.NotEmpty(results);
        }

        [Theory]
        [InlineData(100, 39)]   // Diastolic below minimum
        public void Diastolic_BelowMinimum_FailsValidation(int systolic, int diastolic)
        {
            var bp = new BloodPressure { Systolic = systolic, Diastolic = diastolic };
            var context = new ValidationContext(bp);
            var results = new System.Collections.Generic.List<ValidationResult>();
            
            bool isValid = Validator.TryValidateObject(bp, context, results, validateAllProperties: true);
            
            Assert.False(isValid);
            Assert.NotEmpty(results);
        }

        [Theory]
        [InlineData(100, 101)]  // Diastolic above maximum
        public void Diastolic_AboveMaximum_FailsValidation(int systolic, int diastolic)
        {
            var bp = new BloodPressure { Systolic = systolic, Diastolic = diastolic };
            var context = new ValidationContext(bp);
            var results = new System.Collections.Generic.List<ValidationResult>();
            
            bool isValid = Validator.TryValidateObject(bp, context, results, validateAllProperties: true);
            
            Assert.False(isValid);
            Assert.NotEmpty(results);
        }

        // ========== VALID RANGE TESTS ==========
        
        [Theory]
        [InlineData(70, 40)]    // Minimum valid
        [InlineData(130, 80)]   // Mid-range
        [InlineData(190, 100)]  // Maximum valid
        public void ValidValues_PassValidation(int systolic, int diastolic)
        {
            var bp = new BloodPressure { Systolic = systolic, Diastolic = diastolic };
            var context = new ValidationContext(bp);
            var results = new System.Collections.Generic.List<ValidationResult>();
            
            bool isValid = Validator.TryValidateObject(bp, context, results, validateAllProperties: true);
            
            Assert.True(isValid);
        }

        // ========== SPECIFIC CATEGORY TESTS ==========
        
        [Theory]
        [InlineData(70, 40)]
        [InlineData(85, 55)]
        [InlineData(89, 59)]
        public void Low_Category_CorrectClassification(int systolic, int diastolic)
        {
            var bp = new BloodPressure { Systolic = systolic, Diastolic = diastolic };
            Assert.Equal(BPCategory.Low, bp.Category);
        }

        [Theory]
        [InlineData(90, 60)]
        [InlineData(110, 70)]
        [InlineData(119, 79)]
        public void Ideal_Category_CorrectClassification(int systolic, int diastolic)
        {
            var bp = new BloodPressure { Systolic = systolic, Diastolic = diastolic };
            Assert.Equal(BPCategory.Ideal, bp.Category);
        }

        [Theory]
        [InlineData(120, 70)]
        [InlineData(130, 85)]
        [InlineData(139, 89)]
        public void PreHigh_Category_CorrectClassification(int systolic, int diastolic)
        {
            var bp = new BloodPressure { Systolic = systolic, Diastolic = diastolic };
            Assert.Equal(BPCategory.PreHigh, bp.Category);
        }

        [Theory]
        [InlineData(140, 90)]
        [InlineData(150, 100)]
        [InlineData(190, 100)]
        public void High_Category_CorrectClassification(int systolic, int diastolic)
        {
            var bp = new BloodPressure { Systolic = systolic, Diastolic = diastolic };
            Assert.Equal(BPCategory.High, bp.Category);
        }

        // ========== PROPERTY TESTS ==========
        
        [Fact]
        public void Properties_CanBeSet()
        {
            var bp = new BloodPressure { Systolic = 120, Diastolic = 80 };
            
            Assert.Equal(120, bp.Systolic);
            Assert.Equal(80, bp.Diastolic);
        }

        [Fact]
        public void Constants_AreCorrect()
        {
            Assert.Equal(70, BloodPressure.SystolicMin);
            Assert.Equal(190, BloodPressure.SystolicMax);
            Assert.Equal(40, BloodPressure.DiastolicMin);
            Assert.Equal(100, BloodPressure.DiastolicMax);
        }
    }
}