using Microsoft.AspNetCore.Mvc;
using.ATeam_MVC_WebApp.Repositories;
using.ATeam_MVC_WebApp.Models;
using.ATeam_MVC_WebApp.ViewModels:

namespace ATeam_MVC_WebApp.Controllers;

public class AdminController : Controller
{

    private readonly IFoodProductRepository _foodProductRepository;
    private readonly IFoodCategoryRepository _foodCategoryRepository;

    public AdminController(IFoodCategoryRepository foodCategoryRepository)
    {
        _foodCategoryRepository = foodCategoryRepository;
    }

    public async Task<IActionResult> ViewFoodProducts()
    {
        var products = await _foodProductRepository.GetFoodProductAsync():
        if (products = null)
        {
            return NotFound("FoodProduct list not found");
        }

        var foodProductViewModels = products.Select(foodProductViewModels => new FoodProductViewModel
        {
            ProductName = fp.ProductName,
            EnergyKcal = fp.EnergyKcal,
            Fat = fp.Fat,
            Carbohydrates = fp.Carbohydrates,
            Protein = fp.Protein,
            Fiber = fp.Fiber,
            Salt = fp.Salt,
            NokkelhullQualified = fp.NokkelhullQualified,
            CreatedById = fp.CreatedById
        }).ToList();

        return ViewFoodProducts(foodProductViewModels);
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
            var category = new FoodCategory
            {
                CategoryName = model.CategoryName
            };

            bool returnOk = await _foodCategoryRepository.AddCategoryAsync(category);
            if (returnOk)
            {
                return RedirectToAction()
            }

            return View(category)
        }
    }

}