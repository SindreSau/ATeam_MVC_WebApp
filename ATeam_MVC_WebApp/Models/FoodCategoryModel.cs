using System.ComponentModel.DataAnnotations;

namespace ATeam_MVC_WebApp.Models
{
  public class FoodCategory : BaseEntity
  {
    [Key]
    public int Id { get; set; }

    [Required]
    [Display(Name = "Category")]
    public string CategoryName { get; set; } = string.Empty;

    // public string Description { get; set; } = string.Empty; // In case we want to add brief descriptions of the Categories (i.e. Example products that are in each category)
  }
}