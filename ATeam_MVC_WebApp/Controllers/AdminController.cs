using Microsoft.AspNetCore.Mvc;
using ATeam_MVC_WebApp.Repositories;
using ATeam_MVC_WebApp.Models;
using ATeam_MVC_WebApp.ViewModels;
using ATeam_MVC_WebApp.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace ATeam_MVC_WebApp.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IFoodProductRepository _foodProductRepository;
    private readonly ILogger<AdminController> _logger;

    public AdminController(IFoodProductRepository foodProductRepository, ILogger<AdminController> logger)
    {
        _foodProductRepository = foodProductRepository;
        _logger = logger;
    }

    public async Task<IActionResult> Index(
        int pageNumber = 1,
        int pageSize = 10,
        string orderBy = "productname",
        bool? nokkelhull = null,
        string searchTerm = ""
    )
    {
        _logger.LogInformation("Admin Index accessed with parameters: PageNumber={PageNumber}, PageSize={PageSize}, OrderBy={OrderBy}, Nokkelhull={Nokkelhull}, SearchTerm={SearchTerm}",
            pageNumber, pageSize, orderBy, nokkelhull, searchTerm);

        var products = await _foodProductRepository.GetFoodProductsAsync(pageNumber, pageSize, orderBy, nokkelhull, searchTerm);

        // Store the products in a list to avoid multiple enumeration
        IEnumerable<FoodProduct> foodProducts = products.ToList(); // Added ToList() to avoid multiple enumeration
        var productsList = foodProducts.ToList();

        if (!productsList.Any())
        {
            return NotFound("No food products found.");
        }

        var foodProductViewModels = foodProducts.Select(fp => new FoodProductViewModel
        {
            ProductId = fp.FoodProductId,
            ProductName = fp.ProductName,
            EnergyKcal = fp.EnergyKcal,
            Fat = fp.Fat,
            Carbohydrates = fp.Carbohydrates,
            Protein = fp.Protein,
            Fiber = fp.Fiber,
            Salt = fp.Salt,
            NokkelhullQualified = fp.NokkelhullQualified,
            CreatedByUsername = fp.CreatedBy?.UserName ?? "Unknown", // Added null check
            CategoryName = fp.FoodCategory?.CategoryName ?? "Unknown" // Added null check
        }).ToList();

        var viewModel = new FoodProductListViewModel
        {
            FoodProducts = foodProductViewModels,
            Pagination = new PaginationViewModel
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalCount = await _foodProductRepository.GetFoodProductsCountAsync(searchTerm, nokkelhull)
            },
            OrderBy = orderBy,
            Nokkelhull = nokkelhull,
            SearchTerm = searchTerm
        };

        return View(viewModel);
    }
}