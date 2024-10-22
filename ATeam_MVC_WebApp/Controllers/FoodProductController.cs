using ATeam_MVC_WebApp.Interfaces;
using ATeam_MVC_WebApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using ATeam_MVC_WebApp.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Authorization;
using ATeam_MVC_WebApp.Models;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace ATeam_MVC_WebApp.Controllers;
public class FoodProductController : Controller
{
    private readonly IFoodProductRepository _foodProductRepository;

    public FoodProductController(IFoodProductRepository foodProductRepository)
        {
            _foodProductRepository = foodProductRepository;
        }
        //oskar
        public async Task<IActionResult> ListFoodProducts()
        {
            IEnumerable<FoodProduct> foodProducts = await _foodProductRepository.GetAll();
            return View(foodProducts);
        }
        [HttpGet]
        public IActionResult Create()
        {
           return View();
        }
        /*
        [HttpPost]
        public async Task<IActionResult> Create(CreateFoodProductViewModel foodproduct)
        {
            if (ModelState.IsValid)
            {
                var result = await _
                
            }
            
        }*/

}