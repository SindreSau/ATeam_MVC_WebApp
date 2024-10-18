using ATeam_MVC_WebApp.Models;

namespace ATeam_MVC_WebApp.ViewModels
{
  public class FoodCategoryViewModel
  {
    public IEnumerable<FoodCategory> FoodCategories { get; set; }
    public string? CurrentViewName { get; set; }

    public FoodCategoryViewModel(IEnumerable<FoodCategory> foodCategories, string? currentViewName)
    {
      FoodCategories = foodCategories;
      CurrentViewName = currentViewName;
    }

  }
}