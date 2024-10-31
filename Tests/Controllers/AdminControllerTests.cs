using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ATeam_MVC_WebApp.Controllers;
using ATeam_MVC_WebApp.Interfaces;
using ATeam_MVC_WebApp.Models;
using ATeam_MVC_WebApp.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Tests.Controllers
{
  public class AdminControllerTests
  {
    // Mocks
    private readonly Mock<IFoodProductRepository> _mockFoodProductRepo;
    private readonly Mock<ILogger<AdminController>> _mockLogger;
    // What we're testing
    private readonly AdminController _controller;

    public AdminControllerTests()
    {
      _mockFoodProductRepo = new Mock<IFoodProductRepository>();
      _mockLogger = new Mock<ILogger<AdminController>>();

      _controller = new AdminController(
          _mockFoodProductRepo.Object,
          _mockLogger.Object
      );

      // Set up basic authentication + context for the controller
      SetupControllerContext();
    }


    // Tests that Index action returns a view with correct data when products exist
    [Fact]
    public async Task Index_ReturnsViewWithCorrectModel()
    {
      // Arrange: Create test product with the required propreties
      var testProducts = new List<FoodProduct>
            {
                new FoodProduct
                {
                    FoodProductId = 1,
                    ProductName = "Test Product",
                    EnergyKcal = 100,
                    FoodCategory = new FoodCategory { CategoryName = "Test Category" },
                    CreatedBy = new Microsoft.AspNetCore.Identity.IdentityUser { UserName = "testuser" }
                }
            };

      // Set up mock repo to return the test products
      _mockFoodProductRepo.Setup(repo => repo.GetFoodProductsAsync(
              1, 10, "productname", null, ""))
          .ReturnsAsync(testProducts);

      _mockFoodProductRepo.Setup(repo => repo.GetFoodProductsCountAsync("", null))
          .ReturnsAsync(1);

      // Act: Call Index action
      var result = await _controller.Index();

      // Assert: Verify that the returned view and it's content matches the expected
      var viewResult = Assert.IsType<ViewResult>(result);
      var model = Assert.IsType<FoodProductListViewModel>(viewResult.Model);
      Assert.Single(model.FoodProducts);
      Assert.Equal("Test Product", model.FoodProducts.First().ProductName);
      Assert.Equal("Test Category", model.FoodProducts.First().CategoryName);
      Assert.Equal("testuser", model.FoodProducts.First().CreatedByUsername);
    }

    // Tests that Index action handles empty results correctly
    [Fact]
    public async Task Index_NoProducts_ReturnsViewWithEmptyModel()
    {
      // Arrange: Set up repo to return no products
      var emptyProducts = new List<FoodProduct>();

      _mockFoodProductRepo.Setup(repo => repo.GetFoodProductsAsync(
              1, 10, "productname", null, ""))
          .ReturnsAsync(emptyProducts);

      SetupTempData();

      // Act : Call Index
      var result = await _controller.Index();

      // Assert: Verify empty model and that error message is present
      var viewResult = Assert.IsType<ViewResult>(result);
      var model = Assert.IsType<FoodProductListViewModel>(viewResult.Model);
      Assert.Empty(model.FoodProducts);
      Assert.Equal("No items match your search. Please try again.", _controller.TempData["ErrorMessage"]);
    }

    // Tests that Index handles different search param's correctly
    [Theory]
    [InlineData("test", true, "othersort")]
    [InlineData("", null, "productname")]
    public async Task Index_WithDifferentParameters_ReturnsCorrectModel(string searchTerm, bool? nokkelhull, string orderBy)
    {
      // Arrange: Set up repo with test data... again! 
      var testProducts = new List<FoodProduct>
            {
                new FoodProduct { FoodProductId = 1, ProductName = "Test Product" }
            };

      _mockFoodProductRepo.Setup(repo => repo.GetFoodProductsAsync(
              1, 10, orderBy, nokkelhull, searchTerm))
          .ReturnsAsync(testProducts);

      _mockFoodProductRepo.Setup(repo => repo.GetFoodProductsCountAsync(searchTerm, nokkelhull))
          .ReturnsAsync(1);

      // Act : Call index with different params 
      var result = await _controller.Index(1, 10, orderBy, nokkelhull, searchTerm);

      // Assert : Verify model reflects input
      var viewResult = Assert.IsType<ViewResult>(result);
      var model = Assert.IsType<FoodProductListViewModel>(viewResult.Model);
      Assert.Equal(searchTerm, model.SearchTerm);
      Assert.Equal(nokkelhull, model.Nokkelhull);
      Assert.Equal(orderBy, model.OrderBy);
    }

    // Tests that Index logs info correctly
    [Fact]
    public async Task Index_LogsInformation()
    {
      // Arrange: Set up repo
      var testProducts = new List<FoodProduct> { new FoodProduct { FoodProductId = 1 } };

      _mockFoodProductRepo.Setup(repo => repo.GetFoodProductsAsync(
              It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool?>(), It.IsAny<string>()))
          .ReturnsAsync(testProducts);

      // Act: Call Index
      await _controller.Index(1, 10, "productname", null, "test");

      // Assert: Verify Logging occured
      _mockLogger.Verify(
          x => x.Log(
              LogLevel.Information,
              It.IsAny<EventId>(),
              It.Is<It.IsAnyType>((o, t) => true),
              It.IsAny<Exception>(),
              It.Is<Func<It.IsAnyType, Exception?, string>>((o, t) => true)),
          Times.Once);
    }

    // Helper method like we have in VendorControllerTests to set up controller context with admin role
    private void SetupControllerContext()
    {
      var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "Admin")
            };
      var identity = new ClaimsIdentity(claims, "TestAuth");
      var claimsPrincipal = new ClaimsPrincipal(identity);

      var httpContext = new DefaultHttpContext
      {
        User = claimsPrincipal
      };

      _controller.ControllerContext = new ControllerContext
      {
        HttpContext = httpContext
      };
    }

    // Helper method to set up TempData for error messages
    private void SetupTempData()
    {
      var httpContext = new DefaultHttpContext();
      _controller.TempData = new TempDataDictionary(
          httpContext,
          Mock.Of<ITempDataProvider>()
      );
    }
  }
}