using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Security.Claims;
using ATeam_MVC_WebApp.Controllers;
using ATeam_MVC_WebApp.Interfaces;
using ATeam_MVC_WebApp.Models;
using ATeam_MVC_WebApp.ViewModels;
using Microsoft.AspNetCore.Http;

namespace Tests.Controllers
{
    public class VendorControllerTests
    {
        private readonly Mock<IFoodProductRepository> _mockFoodProductRepo;
        private readonly Mock<IFoodCategoryRepository> _mockFoodCategoryRepo;
        private readonly Mock<UserManager<IdentityUser>> _mockUserManager;
        private readonly VendorController _controller;

        public VendorControllerTests()
        {
            _mockFoodProductRepo = new Mock<IFoodProductRepository>();
            _mockFoodCategoryRepo = new Mock<IFoodCategoryRepository>();

            // Setup mock UserManager
            var userStore = new Mock<IUserStore<IdentityUser>>();
            _mockUserManager = new Mock<UserManager<IdentityUser>>(
                userStore.Object, null!, null!, null!, null!, null!, null!, null!, null!);

            _controller = new VendorController(
                _mockFoodProductRepo.Object,
                _mockFoodCategoryRepo.Object,
                _mockUserManager.Object
            );
        }

        [Fact]
        public async Task Index_ReturnsViewWithCorrectModel()
        {
            // Arrange
            var userId = "test-user-id";
            SetupUserContext(userId);

            var testProducts = new List<FoodProduct>
            {
                new FoodProduct
                {
                    FoodProductId = 1,
                    ProductName = "Test Product",
                    CreatedById = userId,
                    FoodCategory = new FoodCategory { CategoryName = "Test Category" }
                }
            };

            _mockFoodProductRepo.Setup(repo => repo.GetFoodProductsByVendorAsync(
                    userId, 1, 10, "productid", null))
                .ReturnsAsync(testProducts);

            _mockFoodProductRepo.Setup(repo => repo.GetFoodProductsByVendorCountAsync(userId))
                .ReturnsAsync(1);

            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<FoodProductListViewModel>(viewResult.Model);
            Assert.Single(model.FoodProducts);
            Assert.Equal("Test Product", model.FoodProducts.First().ProductName);
        }


        [Fact]
        public async Task Create_GET_ReturnsViewWithCategories()
        {
            // Arrange
            var categories = new List<FoodCategory>
            {
                new FoodCategory { FoodCategoryId = 1, CategoryName = "Category 1" },
                new FoodCategory { FoodCategoryId = 2, CategoryName = "Category 2" }
            };

            _mockFoodCategoryRepo.Setup(repo => repo.GetAllCategoriesAsync())
                .ReturnsAsync(categories);

            // Act
            var result = await _controller.Create();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.ViewData["Categories"]);
        }

        [Fact]
        public async Task Create_POST_ValidModel_RedirectsToIndex()
        {
            // Arrange
            var userId = "test-user-id";
            SetupUserContext(userId);

            var model = new CreateFoodProductViewModel
            {
                ProductName = "New Product",
                EnergyKcal = 100,
                FoodCategoryId = 1
            };

            _mockUserManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(userId);

            _mockFoodProductRepo.Setup(repo => repo.AddFoodProductAsync(It.IsAny<FoodProduct>()))
                .ReturnsAsync(new FoodProduct());

            // Set model state to valid
            _controller.ModelState.Clear();

            // Act
            var result = await _controller.Create(model);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(VendorController.Index), redirectResult.ActionName);
        }

        [Fact]
        public async Task Edit_GET_NonExistentProduct_ReturnsNotFound()
        {
            // Arrange
            var userId = "test-user-id";
            SetupUserContext(userId);

            _mockFoodProductRepo.Setup(repo => repo.GetFoodProductAsync(It.IsAny<int>()))!
                .ReturnsAsync((FoodProduct)null!);

            // Act
            var result = await _controller.Edit(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }


        [Fact]
        public async Task Edit_GET_UnauthorizedAccess_ReturnsUnauthorized()
        {
            // Arrange
            var userId = "test-user-id";
            var differentUserId = "different-user-id";
            SetupUserContext(userId);

            var product = new FoodProduct
            {
                FoodProductId = 1,
                CreatedById = differentUserId
            };

            _mockFoodProductRepo.Setup(repo => repo.GetFoodProductAsync(1))
                .ReturnsAsync(product);

            // Act
            var result = await _controller.Edit(1);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }


        [Fact]
        public async Task Delete_ValidProduct_RedirectsToIndex()
        {
            // Arrange
            var userId = "test-user-id";
            SetupUserContext(userId);

            var product = new FoodProduct
            {
                FoodProductId = 1,
                CreatedById = userId
            };

            _mockUserManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(userId);

            _mockFoodProductRepo.Setup(repo => repo.GetFoodProductAsync(1))
                .ReturnsAsync(product);

            _mockFoodProductRepo.Setup(repo => repo.DeleteFoodProductAsync(1))
                .ReturnsAsync(true);

            _controller.ModelState.Clear();

            // Act
            var result = await _controller.Delete(1);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(VendorController.Index), redirectResult.ActionName);
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task Index_NoUserId_ReturnsUnauthorized(string? userId)
        {
            // Arrange
            var claims = new List<Claim>();
            if (userId != null)
            {
                claims.Add(new Claim(ClaimTypes.NameIdentifier, userId));
            }

            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };

            // Act
            var result = await _controller.Index();

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }



        /// <summary>
        /// Sets up the user context for the controller; to be used in multiple tests.
        /// </summary>
        /// <param name="userId">The user ID to set up</param>
        private void SetupUserContext(string userId)
        {
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, userId),
        new Claim(ClaimTypes.Role, "Vendor") // Add role claim if needed
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

            // Setup UserManager mock for this context
            _mockUserManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(userId);
        }
    }
}