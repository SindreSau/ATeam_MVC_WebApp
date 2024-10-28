using ATeam_MVC_WebApp.Interfaces;
using ATeam_MVC_WebApp.Models;
using ATeam_MVC_WebApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ATeam_MVC_WebApp.Controllers;

[Authorize(Roles = "Admin")]
[Route("Admin/[controller]")]
public class CategoryController : Controller
{
    private readonly IFoodCategoryRepository _foodCategoryRepository;
    private readonly ILogger<CategoryController> _logger;

    public CategoryController(IFoodCategoryRepository foodCategoryRepository, ILogger<CategoryController> logger)
    {
        _foodCategoryRepository = foodCategoryRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            var categories = await _foodCategoryRepository.GetAllCategoriesAsync();
            var viewModel = new FoodCategoryViewModel
            {
                Categories = categories
            };
            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching categories");
            TempData["Error"] = "Failed to load categories.";
            return View(new FoodCategoryViewModel());
        }
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create(FoodCategory category)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid category data.";
                return RedirectToAction(nameof(Index));
            }

            category.CreatedAt = DateTime.UtcNow;
            category.UpdatedAt = DateTime.UtcNow;
            await _foodCategoryRepository.AddCategoryAsync(category);

            TempData["Success"] = "Category created successfully.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating category");
            TempData["Error"] = "Failed to create category.";
            return RedirectToAction(nameof(Index));
        }
    }

    // GET: Admin/Category/Edit/5
    [HttpGet("Edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var category = await _foodCategoryRepository.GetCategoryByIDAsync(id);

            return View(category);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching category for edit");
            TempData["Error"] = "Failed to load category for editing.";
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost("Update")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(FoodCategory category)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", category);
            }

            var existingCategory = await _foodCategoryRepository.GetCategoryByIDAsync(category.FoodCategoryId);
            if (existingCategory == null)
            {
                return NotFound();
            }

            category.UpdatedAt = DateTime.UtcNow;
            await _foodCategoryRepository.UpdateCategoryAsync(category);

            TempData["Success"] = "Category updated successfully.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating category");
            TempData["Error"] = "Failed to update category.";
            return View("Edit", category);
        }
    }

    [HttpPost("Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _foodCategoryRepository.DeleteCategoryAsync(id);
            TempData["Success"] = "Category deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting category");
            TempData["Error"] = "Failed to delete category.";
            return RedirectToAction(nameof(Index));
        }
    }
}