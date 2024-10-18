using ATeam_MVC_WebApp.Models;

namespace ATeam_MVC_WebApp.ViewModels
{
    public class FoodProductViewModel
    {
        public IEnumerable<FoodProduct> FoodProducts { get; set; }
        public string? CurrentViewName { get; set; }

        public FoodProductViewModel(IEnumerable<FoodProduct> foodProducts, string? currentViewName)
        {
            FoodProducts = foodProducts;
            CurrentViewName = currentViewName;
        }

    }
}