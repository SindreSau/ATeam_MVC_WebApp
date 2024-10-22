using System.Security.Cryptography.X509Certificates;
using ATeam_MVC_WebApp.Models;

namespace ATeam_MVC_WebApp.ViewModels;
public class CreateFoodProductViewModel
{
    public string ProductName { get; set; } = string.Empty;
    public decimal EnergyKcal { get; set; }
    public decimal Fat { get; set; }
    public decimal Protein { get; set; }
    public decimal Fiber { get; set; }
    public decimal Salt { get; set; }
    public bool NokkelhullQualified { get; set; }
    public int CategoryId { get; set; }
    //public virtual FoodCategory? Category { get; set; }
    //public string CreatedById { get; set; } = string.Empty;
    //public virtual ApplicationUser? CreatedBy { get; set; }

}





      