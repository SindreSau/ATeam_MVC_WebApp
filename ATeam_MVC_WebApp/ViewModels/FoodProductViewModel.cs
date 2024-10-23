using System.ComponentModel.DataAnnotations;


namespace ATeam_MVC_WebApp.ViewModels
{
    // View Model for displaying paginated list of food products
    public class FoodProductListViewModel
    {
        public List<FoodProductViewModel> FoodProducts { get; set; } = new List<FoodProductViewModel>();

        public PaginationViewModel Pagination { get; set; } = new();

        public string OrderBy { get; set; } = string.Empty;

        public bool? Nokkelhull { get; set; }
    }

    public class FoodProductViewModel
    {
        public int ProductId { get; set; }
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
        [Required(ErrorMessage = "Product name is required")]
        [StringLength(100, ErrorMessage = "Product name cannot exceed 100 characters")]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Energy value is required")]
        [Range(0, 1000, ErrorMessage = "Energy must be between 0 and 1000 kcal")]
        [Display(Name = "Energy (kcal)")]
        public decimal EnergyKcal { get; set; }

        [Required(ErrorMessage = "Fat content is required")]
        [Range(0, 100, ErrorMessage = "Fat must be between 0 and 100g")]
        [Display(Name = "Fat (g)")]
        public decimal Fat { get; set; }

        [Required(ErrorMessage = "Carbohydrates content is required")]
        [Range(0, 100, ErrorMessage = "Carbohydrates must be between 0 and 100g")]
        [Display(Name = "Carbohydrates (g)")]
        public decimal Carbohydrates { get; set; }

        [Required(ErrorMessage = "Protein content is required")]
        [Range(0, 100, ErrorMessage = "Protein must be between 0 and 100g")]
        [Display(Name = "Protein (g)")]
        public decimal Protein { get; set; }

        [Required(ErrorMessage = "Fiber content is required")]
        [Range(0, 100, ErrorMessage = "Fiber must be between 0 and 100g")]
        [Display(Name = "Fiber (g)")]
        public decimal Fiber { get; set; }

        [Required(ErrorMessage = "Salt content is required")]
        [Range(0, 100, ErrorMessage = "Salt must be between 0 and 100g")]
        [Display(Name = "Salt (g)")]
        public decimal Salt { get; set; }

        [Required(ErrorMessage = "Please select a category")]
        [Display(Name = "Category")]
        public int FoodCategoryId { get; set; }
    }

    // View Model for Editing an Existing food product, inherits from Create
    public class EditFoodProductViewModel : CreateFoodProductViewModel
    {
        public int ProductId { get; set; }
    }
}

