using ATeam_MVC_WebApp.Interfaces;
using ATeam_MVC_WebApp.Models;

namespace ATeam_MVC_WebApp.Repositories
{
  public class FoodCategoryRepository : IFoodCategoryRepository
  {
    // TEMPORARY IN-MEMORY VALUES UNTIL DB SET UP (Based on the Lovdata)
    private readonly List<FoodCategory> _categories = new List<FoodCategory>
    {
      new FoodCategory { CategoryID = 1, CategoryName = "Grønnsaker, frukt, bær og nøtter"},
      new FoodCategory { CategoryID = 2, CategoryName = "Grøt, brød og pasta"},
      new FoodCategory { CategoryID = 3, CategoryName = "Melk, syrnede melkeprodukter og vegetabilske alternativer"},
      new FoodCategory { CategoryID = 4, CategoryName = "Ost og vegetabilske alternativer"},
      new FoodCategory { CategoryID = 5, CategoryName = "Matfett og Oljer"},
      new FoodCategory { CategoryID = 6, CategoryName = "Fiskerivarer og produkter av fiskerivarer"},
      new FoodCategory { CategoryID = 7, CategoryName = "Kjøtt og produkter som inneholder kjøtt"},
      new FoodCategory { CategoryID = 8, CategoryName = "Helt, eller delvis vegetabilske produkter"},
      new FoodCategory { CategoryID = 9, CategoryName = "Ferdigretter"},
      new FoodCategory { CategoryID = 10, CategoryName = "Dressinger og sauser"},
    };

    // Asynchronously retrieves all food categories
    public async Task<IEnumerable<FoodCategory>> GetAllCategoriesAsync()
    {
      return await Task.FromResult(_categories);
    }

    // Asynchronously retrieves a food category by it's ID
    public async Task<FoodCategory> GetCategoryByIDAsync(int categoryId)
    {
      var category = _categories.FirstOrDefault(c => c.CategoryID == categoryId);
      return await Task.FromResult(category);
    }

    // Asynchronously adds a new food category to the list
    public async Task<bool> AddCategoryAsync(FoodCategory category)
    {
      _categories.Add(category);
      return await Task.FromResult(true);
    }

    // Asynchronously updates an existing food category
    public async Task<bool> UpdateCategoryAsync(FoodCategory category)
    {
      var existingCategory = await GetCategoryByIDAsync(category.CategoryID);
      if (existingCategory != null)
      {
        existingCategory.CategoryName = category.CategoryName;
        // Any other properties that are updated (if we add description)
        return await Task.FromResult(true);
      }
      return await Task.FromResult(false);
    }

    // Asynchronously deletes a food category by its ID
    public async Task<bool> DeleteCategoryAsync(int categoryId)
    {
      var category = await GetCategoryByIDAsync(categoryId);
      if (category != null)
      {
        _categories.Remove(category);
        return true;
      }
      return false;
    }

  }
}