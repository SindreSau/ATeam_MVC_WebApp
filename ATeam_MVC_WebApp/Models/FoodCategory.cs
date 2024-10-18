using System.ComponentModel.DataAnnotations;

namespace ATeam_MVC_WebApp.Models
{
  class FoodCategory
  {
    public int CategoryID { get; set; } // Unique ID for category

    [Required]
    public string CategoryName { get; set; } // Can't be null

    // public string Description { get; set; } = string.Empty; // In case we want to add brief descriptions of the Categories (i.e. Example products that are in each category)
  }
}