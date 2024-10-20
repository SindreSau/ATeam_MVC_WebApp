using System.ComponentModel.DataAnnotations;
using ATeam_MVC_WebApp.Models;

namespace ATeam_MVC_WebApp.ViewModels
{
  public class FoodCategoryViewModel
  {
    public IEnumerable<FoodCategoryItemViewModel> Categories { get; set; }
    public string? CurrentViewName { get; set; }
    // public bool showDescription { get; set; } // If we decide to add Category Descriptions

    public FoodCategoryViewModel(IEnumerable<FoodCategoryItemViewModel> categories, string? currentViewName)
    {
      Categories = categories;
      CurrentViewName = currentViewName;
      //ShowDescription = false;
    }
  }

  // The 'DTO'
  public class FoodCategoryItemViewModel
  {
    public int Id { get; set; }

    [Display(Name = "Category")]
    public required string CategoryName { get; set; }

    // public string Description { get; set; }
  }
}