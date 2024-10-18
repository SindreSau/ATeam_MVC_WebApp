using ATeam_MVC_WebApp.Models;

namespace ATeam_MVC_WebApp.Interfaces
{
  public interface IFoodCategoryRepository
  {
    Task<IEnumerable<FoodCategory>> GetAllCategoriesAsync(); // GET ALL
    Task<FoodCategory> GetCategoryByID(int categoryId); // GET ONE
    Task<bool> AddCategoryAsync(FoodCategory category); // CREATE
    Task<bool> UpdateCategoryAsync(FoodCategory category); // UPDATE
    Task<bool> DeleteCategoryAsync(int categoryId); // DELETE

  }
}