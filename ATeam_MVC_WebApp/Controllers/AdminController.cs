using Microsoft.AspNetCore.Mvc;
using ATeam_MVC_WebApp.Repositories;
using ATeam_MVC_WebApp.Models;
using ATeam_MVC_WebApp.ViewModels;
using ATeam_MVC_WebApp.Interfaces;

namespace ATeam_MVC_WebApp.Controllers;

public class AdminController : Controller
{

    private readonly IFoodProductRepository _foodProductRepository;
    private readonly IFoodCategoryRepository _foodCategoryRepository;

    public AdminController(IFoodProductRepository foodProductRepository, IFoodCategoryRepository foodCategoryRepository)
    {
        _foodProductRepository = foodProductRepository;
        _foodCategoryRepository = foodCategoryRepository;
    }

    public async Task<IActionResult> ViewFoodProducts(int pageNumber = 1, int pageSize = 10, string orderBy = "productname", bool nokkelhull = false)
    {
        var products = await _foodProductRepository.GetFoodProductsAsync(pageNumber, pageSize, orderBy, nokkelhull);
        if (products == null || !products.Any())
        {
            return NotFound("No food products found.");
        }

        var foodProductViewModels = products.Select(fp => new FoodProductViewModel
        {
            ProductName = fp.ProductName,
            EnergyKcal = fp.EnergyKcal,
            Fat = fp.Fat,
            Carbohydrates = fp.Carbohydrates,
            Protein = fp.Protein,
            Fiber = fp.Fiber,
            Salt = fp.Salt,
            NokkelhullQualified = fp.NokkelhullQualified,
            CreatedByUsername = fp.CreatedBy.UserName,
            Category = new FoodCategoryItemViewModel
        {
            CategoryName = fp.Category.CategoryName
        }

        }).ToList();

        return View(foodProductViewModels);
    }


    [HttpGet]
    public IActionResult CreateCategory()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory(FoodCategoryViewModel model)
    {
        if (ModelState.IsValid)
        {
            var category = new FoodCategory;
            {
                CategoryName = model.CategoryName;
            };

            bool returnOk = await _foodCategoryRepository.AddCategoryAsync(category);
            if (returnOk)
            {
                return RedirectToAction();
            }

            return View(category);
        }
    }

}