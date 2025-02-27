using ATeam_MVC_WebApp.Interfaces;
using ATeam_MVC_WebApp.Models;
using ATeam_MVC_WebApp.Data;
using Microsoft.EntityFrameworkCore;

namespace ATeam_MVC_WebApp.Repositories
{
    public class FoodCategoryRepository : IFoodCategoryRepository
    {
        private readonly ApplicationDbContext _context;

        // Constructor to initialize repo with database context
        public FoodCategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Asynchronously retrieves all food categories
        public async Task<IEnumerable<FoodCategory>> GetAllCategoriesAsync()
        {
            return await _context.FoodCategories.ToListAsync();
        }

        // Asynchronously retrieves a food category by it's ID
        public async Task<FoodCategory> GetCategoryByIDAsync(int categoryId)
        {
            // Finds and returns specific food category by ID
            var category = await _context.FoodCategories.FindAsync(categoryId);
            if (category == null)
            {
                throw new KeyNotFoundException($"FoodCategory with ID {categoryId} not found.");
            }

            return category;
        }

        // Asynchronously adds a new food category to the list
        public async Task<bool> AddCategoryAsync(FoodCategory category)
        {
            // Adds new food category to the context
            _context.FoodCategories.Add(category);
            // Saves changes to database
            await _context.SaveChangesAsync();
            return true;
        }

        // Asynchronously updates an existing food category
        public async Task<FoodCategory> UpdateCategoryAsync(FoodCategory category)
        {
            var existingCategory = await _context.FoodCategories.FindAsync(category.FoodCategoryId);
            if (existingCategory == null)
            {
                throw new KeyNotFoundException(
                    $"FoodCategory with ID {category.FoodCategoryId} not found. Could not update.");
            }

            // Updates name of category in context
            existingCategory.CategoryName = category.CategoryName;
            existingCategory.UpdatedAt = category.UpdatedAt;

            _context.FoodCategories.Update(existingCategory);
            // Saves changes to database
            await _context.SaveChangesAsync();
            return existingCategory;
        }

        // Create custom exception for deletion of food category with associated products
        public class CategoryInUseException(string categoryName, int productCount) : Exception(
            $"Cannot delete category '{categoryName}' because it is being used by {productCount} food products.");

        // Asynchronously deletes a food category by its ID
        public async Task<bool> DeleteCategoryAsync(int categoryId)
        {
            var category = await _context.FoodCategories
                .Include(c => c.FoodProducts)
                .FirstOrDefaultAsync(c => c.FoodCategoryId == categoryId);

            if (category == null)
            {
                throw new KeyNotFoundException($"FoodCategory with ID {categoryId} not found. Could not delete.");
            }

            if (category.FoodProducts.Any())
            {
                throw new CategoryInUseException(category.CategoryName, category.FoodProducts.Count);
            }

            // Delete category from context
            _context.FoodCategories.Remove(category);
            // Save changes to database
            await _context.SaveChangesAsync();
            return true;
        }
    }
}