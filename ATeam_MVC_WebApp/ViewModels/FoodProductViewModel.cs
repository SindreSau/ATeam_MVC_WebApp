
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace ATeam_MVC_WebApp.ViewModels
{
    public class FoodProductViewModel
    {
        public string ProductName { get; set; } = string.Empty;

        public decimal EnergyKcal { get; set; }

        public decimal Fat { get; set; }

        public decimal Carbohydrates { get; set; }

        public decimal Protein { get; set; }

        public decimal Fiber { get; set; }

        public decimal Salt { get; set; }

        public bool NokkelhullQualified { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public string CreatedByUsername { get; set; } = string.Empty;
    }

    // View Model for creating a new food product
    public class CreateFoodProductViewModel
    {
        [Required]
        [StringLength(100)]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; } = string.Empty;

        [Required]
        [Range(0, double.MaxValue)]
        [Display(Name = "Energy (kcal)")]
        public decimal EnergyKcal { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Fat { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Carbohydrates { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Protein { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Fiber { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Salt { get; set; }

        [Display(Name = "Nøkkelhull Qualified")]
        public bool NokkelhullQualified { get; set; }

        [Required]
        [Display(Name = "Category")]
        public string CategoryName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "CategoryId")]
        public int FoodCategoryId { get; set; }
    }

    // View Model for Editing an Existing food product, inherits from Create
    public class EditFoodProductViewModel : CreateFoodProductViewModel
    {
        public int ProductId { get; set; }
    }
}

