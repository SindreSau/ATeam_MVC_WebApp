using System.ComponentModel.DataAnnotations;
using ATeam_MVC_WebApp.Models;

namespace ATeam_MVC_WebApp.ViewModels
{
  public class FoodCategoryViewModel
  {
    public IEnumerable<FoodCategory> Categories { get; set; } = new List<FoodCategory>();
    public FoodCategory? CategoryToEdit { get; set; }
  }
}