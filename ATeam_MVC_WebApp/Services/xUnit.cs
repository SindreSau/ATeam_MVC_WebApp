using Xunit;

public class NutritionCalculatorServiceTest
{
    [Fact]
    public void Test_IsNokkelhullQualified()
    {
        // Arrange
        float energyKcal = 35;
        float fat = 2;
        float carbohydrates = 35;
        float protein = 4;
        float fiber = 7;
        float salt = 0.8f;

        // Act
        bool result = NutritionCalculatorService.IsNokkelhullQualified(energyKcal, fat, carbohydrates, protein, fiber, salt);

        // Assert
        Assert.True(result, "Product should be Nokkelhull qualified");
    }
}