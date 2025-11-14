using Xunit;
using System.ComponentModel.DataAnnotations;

namespace BPCalculator.Tests
{
    public class BloodPressureTests
    {
        // ========== CATEGORY CLASSIFICATION TESTS ==========
        // Smoke tests – core functionality
        
        [Theory]
        [InlineData(85, 55, BPCategory.Low)]
        [InlineData(115, 75, BPCategory.Ideal)]
        [InlineData(130, 85, BPCategory.PreHigh)]
        [InlineData(145, 95, BPCategory.High)]
        [Trait("Category", "Smoke")]
        public void Category_ReturnsExpected(int systolic, int diastolic, BPCategory expected)
        {
            var bp = new BloodPressure { Systolic = systolic, Diastolic = diastolic };
            Assert.Equal(expected, bp.Category);
        }

        // ========== BOUNDARY TESTS ==========
        // Smoke tests
        
        [Theory]
        [InlineData(89, 59, BPCategory.Low)]
        [InlineData(90, 60, BPCategory.Ideal)]
        [Trait("Category", "Smoke")]
        public void Category_LowBoundary(int systolic, int diastolic, BPCategory expected)
        {
            var bp = new BloodPressure { Systolic = systolic, Diastolic = diastolic };
            Assert.Equal(expected, bp.Category);
        }

        [Theory]
        [InlineData(119, 79, BPCategory.Ideal)]
        [InlineData(120, 80, BPCategory.PreHigh)]
        [Trait("Category", "Smoke")]
        public void Category_IdealBoundary(int systolic, int diastolic, BPCategory expected)
        {
            var bp = new BloodPressure { Systolic = systolic, Diastolic = diastolic };
            Assert.Equal(expected, bp.Category);
        }

        [Theory]
        [InlineData(139, 89, BPCategory.PreHigh)]
        [InlineData(140, 90, BPCategory.High)]
        [Trait("Category", "Smoke")]
        public void Category_PreHighAndHighBoundary(int systolic, int diastolic, BPCategory expected)
        {
            var bp = new BloodPressure { Systolic = systolic, Diastolic = diastolic };
            Assert.Equal(expected, bp.Category);
        }

        // ========== EDGE CASES ==========
        // Smoke tests
        
        [Fact]
        [Trait("Category", "Smoke")]
        public void Category_MinimumValidValues()
        {
            var bp = new BloodPressure { Systolic = 70, Diastolic = 40 };
            Assert.Equal(BPCategory.Low, bp.Category);
        }

        [Fact]
        [Trait("Category", "Smoke")]
        public void Category_MaximumValidValues()
        {
            var bp = new BloodPressure { Systolic = 190, Diastolic = 100 };
            Assert.Equal(BPCategory.High, bp.Category);
        }

        // ========== VALIDATION TESTS ==========
        // PostDeploy tests – after deployment checks
        
        [Fact]
        [Trait("Category", "PostDeploy")]
        public void Category_Throws_WhenSystolicNotGreaterThanDiastolic()
        {
            var bp = new BloodPressure { Systolic = 80, Diastolic = 90 };
            Assert.Throws<ValidationException>(() => _ = bp.Category);
        }

        [Fact]
        [Trait("Category", "PostDeploy")]
        public void Category_Throws_WhenSystolicEqualsDiastolic()
        {
            var bp = new BloodPressure { Systolic = 100, Diastolic = 100 };
            Assert.Throws<ValidationException>(() => _ = bp.Category);
        }

        [Fact]
        [Trait("Category", "PostDeploy")]
        public void Category_Throws_WhenSystolicLessThanDiastolic()
        {
            var bp = new BloodPressure { Systolic = 70, Diastolic = 120 };
            Assert.Throws<ValidationException>(() => _ = bp.Category);
        }

        // ========== RANGE VALIDATION TESTS ==========
        // PostDeploy tests
        
        [Theory]
        [InlineData(69, 50)]
        [Trait("Category", "PostDeploy")]
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
        [InlineData(191, 90)]
        [Trait("Category", "PostDeploy")]
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
        [InlineData(100, 39)]
        [Trait("Category", "PostDeploy")]
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
        [InlineData(100, 101)]
        [Trait("Category", "PostDeploy")]
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
        // Smoke tests
        
        [Theory]
        [InlineData(70, 40)]
        [InlineData(130, 80)]
        [InlineData(190, 100)]
        [Trait("Category", "Smoke")]
        public void ValidValues_PassValidation(int systolic, int diastolic)
        {
            var bp = new BloodPressure { Systolic = systolic, Diastolic = diastolic };
            var context = new ValidationContext(bp);
            var results = new System.Collections.Generic.List<ValidationResult>();
            
            bool isValid = Validator.TryValidateObject(bp, context, results, validateAllProperties: true);
            
            Assert.True(isValid);
        }

        // ========== SPECIFIC CATEGORY TESTS ==========
        // E2E tests
        
        [Theory]
        [InlineData(70, 40)]
        [InlineData(85, 55)]
        [InlineData(89, 59)]
        [Trait("Category", "E2E")]
        public void Low_Category_CorrectClassification(int systolic, int diastolic)
        {
            var bp = new BloodPressure { Systolic = systolic, Diastolic = diastolic };
            Assert.Equal(BPCategory.Low, bp.Category);
        }

        [Theory]
        [InlineData(90, 60)]
        [InlineData(110, 70)]
        [InlineData(119, 79)]
        [Trait("Category", "E2E")]
        public void Ideal_Category_CorrectClassification(int systolic, int diastolic)
        {
            var bp = new BloodPressure { Systolic = systolic, Diastolic = diastolic };
            Assert.Equal(BPCategory.Ideal, bp.Category);
        }

        [Theory]
        [InlineData(120, 70)]
        [InlineData(130, 85)]
        [InlineData(139, 89)]
        [Trait("Category", "E2E")]
        public void PreHigh_Category_CorrectClassification(int systolic, int diastolic)
        {
            var bp = new BloodPressure { Systolic = systolic, Diastolic = diastolic };
            Assert.Equal(BPCategory.PreHigh, bp.Category);
        }

        [Theory]
        [InlineData(140, 90)]
        [InlineData(150, 100)]
        [InlineData(190, 100)]
        [Trait("Category", "E2E")]
        public void High_Category_CorrectClassification(int systolic, int diastolic)
        {
            var bp = new BloodPressure { Systolic = systolic, Diastolic = diastolic };
            Assert.Equal(BPCategory.High, bp.Category);
        }

        // ========== PROPERTY TESTS ==========
        // Smoke tests
        
        [Fact]
        [Trait("Category", "Smoke")]
        public void Properties_CanBeSet()
        {
            var bp = new BloodPressure { Systolic = 120, Diastolic = 80 };
            
            Assert.Equal(120, bp.Systolic);
            Assert.Equal(80, bp.Diastolic);
        }

        [Fact]
        [Trait("Category", "Smoke")]
        public void Constants_AreCorrect()
        {
            Assert.Equal(70, BloodPressure.SystolicMin);
            Assert.Equal(190, BloodPressure.SystolicMax);
            Assert.Equal(40, BloodPressure.DiastolicMin);
            Assert.Equal(100, BloodPressure.DiastolicMax);
        }
    }
}
