using ATeam_MVC_WebApp.Models;
using ATeam_MVC_WebApp.Interfaces;
using ATeam_MVC_WebApp.Services;
using Moq;


namespace Tests.Repositories
{
    public class FoodProductRepositoryTests
    {
        [Theory]
        [InlineData(40, 3, 35, 5, 7, 0.5, true, "Should qualify for Nokkelhull")]
        [InlineData(40, 3, 35, 5, 7, 1.1, false, "Should not qualify for Nokkelhull (too much salt)")]
        [InlineData(40, 3, 35, 2, 7, 0.5, false, "Should not qualify for Nokkelhull (too little protein)")]
        public async Task AddFoodProduct_ShouldCalculateNokkelhullQualification(
            decimal energyKcal,
            decimal fat,
            decimal carbohydrates,
            decimal protein,
            decimal fiber,
            decimal salt,
            bool expectedNokkelhullStatus,
            string scenarioDescription)
        {
            // Arrange
            var mockRepo = new Mock<IFoodProductRepository>();

            var testProduct = new FoodProduct
            {
                ProductName = "Test Product",
                EnergyKcal = energyKcal,
                Fat = fat,
                Carbohydrates = carbohydrates,
                Protein = protein,
                Fiber = fiber,
                Salt = salt,
                FoodCategoryId = 1,
                CreatedById = "test-user-id",
                CreatedAt = DateTime.UtcNow
            };

            mockRepo.Setup(repo => repo.AddFoodProductAsync(It.IsAny<FoodProduct>()))
                .ReturnsAsync((FoodProduct fp) =>
                {
                    fp.NokkelhullQualified = NutritionCalculatorService.IsNokkelhullQualified(
                        (float)fp.EnergyKcal,
                        (float)fp.Protein,
                        (float)fp.Carbohydrates,
                        (float)fp.Fat,
                        (float)fp.Fiber,
                        (float)fp.Salt
                    );
                    return fp;
                });

            // Act
            var result = await mockRepo.Object.AddFoodProductAsync(testProduct);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(testProduct.ProductName, result.ProductName);
            Assert.Equal(expectedNokkelhullStatus, result.NokkelhullQualified); // Check Nokkelhull status
            mockRepo.Verify(repo => repo.AddFoodProductAsync(It.IsAny<FoodProduct>()), Times.Once(), scenarioDescription);
            mockRepo.VerifyNoOtherCalls();
        }


        [Fact]
        public async Task AddFoodProduct_NullProduct_ShouldThrowArgumentNullException()
        {
            // Arrange
            var mockRepo = new Mock<IFoodProductRepository>();

            mockRepo.Setup(repo => repo.AddFoodProductAsync(null!))
                // ReSharper disable once NotResolvedInText
                .ThrowsAsync(new ArgumentNullException("foodProduct"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                async () => await mockRepo.Object.AddFoodProductAsync(null!)
            );

            Assert.Equal("foodProduct", exception.ParamName);
        }

        [Fact]
        public async Task UpdateFoodProduct_ShouldRecalculateNokkelhullQualification()
        {
            // Arrange
            var mockRepo = new Mock<IFoodProductRepository>();
            var product = new FoodProduct
            {
                FoodProductId = 1,
                ProductName = "Test Product",
                EnergyKcal = 40,
                Fat = 3,
                Carbohydrates = 35,
                Protein = 5,
                Fiber = 7,
                Salt = 0.5m,
                NokkelhullQualified = false // Initially set to false
            };

            mockRepo.Setup(repo => repo.UpdateFoodProductAsync(It.IsAny<FoodProduct>()))
                .ReturnsAsync((FoodProduct fp) =>
                {
                    fp.NokkelhullQualified = NutritionCalculatorService.IsNokkelhullQualified(
                        (float)fp.EnergyKcal,
                        (float)fp.Protein,
                        (float)fp.Carbohydrates,
                        (float)fp.Fat,
                        (float)fp.Fiber,
                        (float)fp.Salt
                    );
                    return fp;
                });

            // Act
            var result = await mockRepo.Object.UpdateFoodProductAsync(product);

            // Assert
            Assert.True(result.NokkelhullQualified); // Should be true based on values
            mockRepo.Verify(repo => repo.UpdateFoodProductAsync(It.IsAny<FoodProduct>()), Times.Once());
        }

        [Fact]
        public async Task GetFoodProductsByVendor_ShouldReturnCorrectProducts()
        {
            // Arrange
            var mockRepo = new Mock<IFoodProductRepository>();
            var vendorId = "test-vendor";
            var products = new List<FoodProduct>
            {
                new FoodProduct { CreatedById = vendorId },
                new FoodProduct { CreatedById = vendorId }
            };

            mockRepo.Setup(repo => repo.GetFoodProductsByVendorAsync(
                    vendorId, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool?>()))
                .ReturnsAsync(products);

            // Act
            var result = await mockRepo.Object.GetFoodProductsByVendorAsync(vendorId, 1, 10, "productname", null);

            // Assert
            IEnumerable<FoodProduct> foodProducts = result.ToList(); // Turn into list to avoid "possible multiple enumeration" warning
            Assert.Equal(2, foodProducts.Count());
            Assert.All(foodProducts, item => Assert.Equal(vendorId, item.CreatedById));
        }
    }
}