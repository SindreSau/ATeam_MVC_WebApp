using ATeam_MVC_WebApp.Interfaces;
using ATeam_MVC_WebApp.Models;
using ATeam_MVC_WebApp.Repositories;
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
        var categories = await _foodCategoryRepository.GetAllCategoriesAsync();
        var viewModel = new FoodCategoryViewModel
        {
            Categories = categories
        };
        return View(viewModel);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create(FoodCategory category)
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

    // GET: Admin/Category/Edit/5
    [HttpGet("Edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        var category = await _foodCategoryRepository.GetCategoryByIDAsync(id);

        return View(category);
    }

    [HttpPost("Update")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(FoodCategory category)
    {
        if (!ModelState.IsValid)
        {
            return View("Edit", category);
        }

        var existingCategory = await _foodCategoryRepository.GetCategoryByIDAsync(category.FoodCategoryId);
        if (existingCategory == null)
        {
            TempData["Error"] = "Category not found.";
            return NotFound();
        }

        category.UpdatedAt = DateTime.UtcNow;
        await _foodCategoryRepository.UpdateCategoryAsync(category);

        TempData["Success"] = "Category updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _foodCategoryRepository.DeleteCategoryAsync(id);
            TempData["Success"] = "Category deleted successfully.";
        }
        catch (FoodCategoryRepository.CategoryInUseException ex)
        {
            TempData["Error"] = ex.Message;
        }
        catch (KeyNotFoundException ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }
}