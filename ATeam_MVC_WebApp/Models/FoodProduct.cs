using System.ComponentModel.DataAnnotations;

namespace ATeam_MVC_WebApp.Models
{
	public class FoodProduct
    {
	    [Key]
        public int Id { get; set; }  // Primary Key

        [Required]
        public string ProducerId { get; set; }  // Foreign Key to the producer table

        [Required]
        [StringLength(100, ErrorMessage = "Product name cannot exceed 100 characters.")]
        public string ProductName { get; set; }

        [Required]
        public int CategoryId { get; set; }  // Foreign Key to the category table

        [Range(0, double.MaxValue, ErrorMessage = "Energy (kcal) must be a positive value.")]
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
    }
}