using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATeam_MVC_WebApp.Models
{
	public class FoodProduct : BaseEntity
    {
	    [Key]
        public int Id { get; set; }  // Primary Key

        [Required]
        [StringLength(100, ErrorMessage = "Product name cannot exceed 100 characters.")]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Energy (kcal) must be a positive value.")]
        [Display(Name = "Energy (kcal)")]
        public decimal EnergyKcal { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Fat must be a positive value.")]
        public decimal Fat { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Carbohydrates must be a positive value.")]
        public decimal Carbohydrates { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Protein must be a positive value.")]
        public decimal Protein { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Fiber must be a positive value.")]
        public decimal Fiber { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Salt must be a positive value.")]
        public decimal Salt { get; set; }

        public bool NokkelhullQualified { get; set; }  // Indicates if the product is nøkkelhull certified

        // Relationship to FoodCategory
        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [ForeignKey("Id")]
        public virtual FoodCategory? Category { get; set; }

        // Relationship to User
        [Required]
        [Display(Name = "Created By")]
        public string CreatedById { get; set; } = string.Empty;

        [ForeignKey("Id")]
        public virtual User? CreatedBy { get; set; }
    }
}