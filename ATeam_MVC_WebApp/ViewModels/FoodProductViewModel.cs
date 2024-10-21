using System;
using System.ComponentModel.DataAnnotations;

namespace ATeam_MVC_WebApp.ViewModels
{
    public class FoodProductViewModel
    {

        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(100, ErrorMessage = "Product name cannot exceed 100 characters.")]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Energy value is required.")]
        [Range(0, 10000, ErrorMessage = "Energy (kcal) must be between 0 and 10,000.")]
        [Display(Name = "Energy (kcal)")]
        public decimal EnergyKcal { get; set; }

        [Required(ErrorMessage = "Fat value is required.")]
        [Range(0, 100, ErrorMessage = "Fat must be between 0 and 100 grams.")]
        [Display(Name = "Fat (g)")]
        public decimal Fat { get; set; }

        [Required(ErrorMessage = "Carbohydrates value is required.")]
        [Range(0, 100, ErrorMessage = "Carbohydrates must be between 0 and 100 grams.")]
        [Display(Name = "Carbohydrates (g)")]
        public decimal Carbohydrates { get; set; }

        [Required(ErrorMessage = "Protein value is required.")]
        [Range(0, 100, ErrorMessage = "Protein must be between 0 and 100 grams.")]
        [Display(Name = "Protein (g)")]
        public decimal Protein { get; set; }

        [Required(ErrorMessage = "Fiber value is required.")]
        [Range(0, 100, ErrorMessage = "Fiber must be between 0 and 100 grams.")]
        [Display(Name = "Fiber (g)")]
        public decimal Fiber { get; set; }

        [Required(ErrorMessage = "Salt value is required.")]
        [Range(0, 10, ErrorMessage = "Salt must be between 0 and 10 grams.")]
        [Display(Name = "Salt (g)")]
        public decimal Salt { get; set; }

        [Display(Name = "Nøkkelhull Qualified")]
        public bool NokkelhullQualified { get; set; }

        [Required]
        [Display(Name = "Category")]
        public virtual FoodCategoryItemViewModel? Category { get; set; }

        [Required]
        [Display(Name = "Created By")]
        [StringLength(450)] 
        public string CreatedByUsername { get; set; } = string.Empty;
    }
}
