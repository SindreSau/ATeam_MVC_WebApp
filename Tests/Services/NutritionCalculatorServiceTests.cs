using ATeam_MVC_WebApp.Services;
using Xunit;

namespace Tests.Services
{
    public class NutritionCalculatorServiceTests
    {
        [Theory]
        [InlineData(35, 4, 35, 2.5, 7, 0.8, true)]    // All values within limits
        [InlineData(45, 4, 35, 2.5, 7, 0.8, false)]   // Energy too high
        [InlineData(35, 2, 35, 2.5, 7, 0.8, false)]   // Protein too low
        [InlineData(35, 4, 45, 2.5, 7, 0.8, false)]   // Carbs too high
        [InlineData(40, 3, 40, 3, 6, 1, true)]        // Boundary values (max/min limits)
        public void IsNokkelhullQualified_VariousNutrients_ReturnsExpectedResult(
            float energyKcal, float protein, float carbohydrates,
            float fat, float fiber, float salt, bool expectedResult)
        {
            // Act
            bool result = NutritionCalculatorService.IsNokkelhullQualified(
                energyKcal, protein, carbohydrates, fat, fiber, salt);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(-1, 4, 35, 2.5f, 7, 0.8f)]     // Negative energy
        [InlineData(35, -2, 35, 2.5f, 7, 0.8f)]    // Negative protein
        [InlineData(35, 4, -35, 2.5f, 7, 0.8f)]    // Negative carbs
        [InlineData(35, 4, 35, -2.5f, 7, 0.8f)]    // Negative fat
        [InlineData(35, 4, 35, 2.5f, -7, 0.8f)]    // Negative fiber
        [InlineData(35, 4, 35, 2.5f, 7, -0.8f)]    // Negative salt
        public void IsNokkelhullQualified_NegativeValues_ReturnsFalse(
            float energyKcal, float protein, float carbohydrates,
            float fat, float fiber, float salt)
        {
            // Act
            bool result = NutritionCalculatorService.IsNokkelhullQualified(
                energyKcal, protein, carbohydrates, fat, fiber, salt);

            // Assert
            Assert.False(result);
        }
    }
}