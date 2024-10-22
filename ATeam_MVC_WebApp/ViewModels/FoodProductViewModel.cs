
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
}
