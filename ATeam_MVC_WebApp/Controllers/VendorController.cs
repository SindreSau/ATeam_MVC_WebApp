using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ATeam_MVC_WebApp.Interfaces;
using ATeam_MVC_WebApp.Models;
using ATeam_MVC_WebApp.ViewModels;
using System.Security.Claims;
using Microsoft.CodeAnalysis.Differencing;

namespace ATeam_MVC_WebApp.Controllers;

// Controller for vendors to manage their food products - requires authentication
[Authorize]
public class VendorController : Controller
{
  // Repositories for Database operations:
  private readonly IFoodProductRepository _foodProductRepository;
  private readonly IFoodCategoryRepository _foodCategoryRepository;

  // Constructor to inject required repositories:
  public VendorController(IFoodProductRepository foodProductRepository, IFoodCategoryRepository foodCategoryRepository)
  {
    _foodProductRepository = foodProductRepository;
    _foodCategoryRepository = foodCategoryRepository;
  }

  // ======== DISPLAY ALL PRODUCTS ======== 
  // Displays a paginated list of food products owned by the current vendor
  public async Task<IActionResult> MyProducts(int pageNumber = 1, int pageSize = 10, string orderBy = "productname", bool? nokkelhull = null)
  {
    // Get the current user's ID from their claims
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    if (string.IsNullOrEmpty(userId))
    {
      return Unauthorized();
    }

    // Get products and filter to show only those created by the current vendor
    var products = await _foodProductRepository.GetFoodProductsAsync(pageNumber, pageSize, orderBy, nokkelhull);
    var vendorProducts = products.Where(p => p.CreatedById == userId).ToList();

    if (!vendorProducts.Any())
    {
      return View(new List<FoodProductViewModel>());
    }

    // Convert the products to view models for display
    var foodProductViewModels = vendorProducts.Select(fp => new FoodProductViewModel
    {
      ProductName = fp.ProductName,
      EnergyKcal = fp.EnergyKcal,
      Fat = fp.Fat,
      Carbohydrates = fp.Carbohydrates,
      Protein = fp.Protein,
      Fiber = fp.Fiber,
      Salt = fp.Salt,
      NokkelhullQualified = fp.NokkelhullQualified,
      CategoryName = fp.Category?.CategoryName ?? "Unknown", // Null check
      CreatedByUsername = fp.CreatedBy?.UserName ?? "Unknown" // Null check 
    }).ToList();

    return View(foodProductViewModels);
  }


  // ======== CREATE ======== 
  // Displays the form for creating a new food product
  public async Task<IActionResult> CreateProduct()
  {
    // Load categories for the dropdown
    var categories = await _foodCategoryRepository.GetAllCategoriesAsync();
    ViewBag.Categories = categories;
    return View(new CreateFoodProductViewModel());
  }

  // Handles the submission of a new food product
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> CreateProduct(CreateFoodProductViewModel model)
  {
    if (!ModelState.IsValid)
    {
      var categories = await _foodCategoryRepository.GetAllCategoriesAsync();
      ViewBag.Categories = categories;
      return View(model);
    }

    // Get the current user's ID for product ownership
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    if (string.IsNullOrEmpty(userId))
    {
      return Unauthorized();
    }

    // Create new food product from the view model
    var foodProduct = new FoodProduct
    {
      ProductName = model.ProductName,
      EnergyKcal = model.EnergyKcal,
      Fat = model.Fat, // Thick
      Carbohydrates = model.Carbohydrates,
      Protein = model.Protein,
      Fiber = model.Fiber,
      Salt = model.Salt,
      NokkelhullQualified = model.NokkelhullQualified,
      CategoryId = model.CategoryId,
      CreatedById = userId,
      CreatedAt = DateTime.UtcNow,
      UpdatedAt = DateTime.UtcNow
    };

    await _foodProductRepository.AddFoodProductAsync(foodProduct);
    // After adding new product, returns to view of MyProducts
    return RedirectToAction(nameof(MyProducts));
  }


  // ======== EDIT ======== 
  // Displays the form for editing an existing food product
  public async Task<IActionResult> EditProduct(int id)
  {
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    var product = await _foodProductRepository.GetFoodProductAsync(id);

    if (product == null)
    {
      return NotFound();
    }

    // Verify the vendor owns this product
    if (product.CreatedById != userId)
    {
      return Unauthorized();
    }

    // Load categories in drop down
    var categories = await _foodCategoryRepository.GetAllCategoriesAsync();
    ViewBag.Categories = categories;

    // Convert product to view model for editing
    var viewModel = new EditFoodProductViewModel
    {
      ProductId = product.FoodProductId,
      ProductName = product.ProductName,
      EnergyKcal = product.EnergyKcal,
      Fat = product.Fat,
      Carbohydrates = product.Carbohydrates,
      Protein = product.Protein,
      Fiber = product.Fiber,
      Salt = product.Salt,
      NokkelhullQualified = product.NokkelhullQualified,
      CategoryId = product.CategoryId,
    };

    return View(viewModel);
  }

  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> EditProduct(int id, EditFoodProductViewModel model)
  {
    if (id != model.ProductId)
    {
      return NotFound();
    }

    if (!ModelState.IsValid)
    {
      var categories = await _foodCategoryRepository.GetAllCategoriesAsync();
      ViewBag.Categories = categories;
      return View(model);
    }

    // Verify vendor owns the product
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    var existingProduct = await _foodProductRepository.GetFoodProductAsync(id);

    if (existingProduct == null || existingProduct.CreatedById != userId)
    {
      return Unauthorized();
    }

    // Update the existing product with new values 
    existingProduct.ProductName = model.ProductName;
    existingProduct.EnergyKcal = model.EnergyKcal;
    existingProduct.Fat = model.Fat;
    existingProduct.Carbohydrates = model.Carbohydrates;
    existingProduct.Protein = model.Protein;
    existingProduct.Fiber = model.Fiber;
    existingProduct.Salt = model.Salt;
    existingProduct.NokkelhullQualified = model.NokkelhullQualified;
    existingProduct.CategoryId = model.CategoryId;
    existingProduct.UpdatedAt = DateTime.UtcNow;

    await _foodProductRepository.UpdateFoodProductAsync(existingProduct);
    return RedirectToAction(nameof(MyProducts)); // Returns to MyProducts method, which sends Vendor back to seeing their products 
  }

  // ======== DELETE ======== 
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> DeleteProduct(int id)
  {
    // Verify vendor before deleting
    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    var product = await _foodProductRepository.GetFoodProductAsync(id);

    if (product == null)
    {
      return NotFound();
    }

    if (product.CreatedById != userId)
    {
      return Unauthorized();
    }

    await _foodProductRepository.DeleteFoodProductAsync(id);
    return RedirectToAction(nameof(MyProducts));
  }
}